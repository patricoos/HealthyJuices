using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using HealthyJuices.Application.Auth;
using HealthyJuices.Common.Auth;
using HealthyJuices.Common.Exceptions;
using HealthyJuices.Domain.Models.Users;
using HealthyJuices.Domain.Models.Users.DataAccess;
using HealthyJuices.Domain.Services;
using HealthyJuices.Shared.Dto;
using HealthyJuices.Shared.Enums;
using Nexus.Application.Mappers;

namespace HealthyJuices.Application.Controllers
{
    public class AuthorizationController
    {
        private readonly IUserRepository _userRepository;
        private readonly SimpleTokenProvider _tokenProvider;
        private readonly ITimeProvider _timeProvider;
        private readonly EmailService _emailService;

        public AuthorizationController(IUserRepository userRepository, SimpleTokenProvider tokenProvider, ITimeProvider timeProvider, EmailService emailService)
        {
            _userRepository = userRepository;
            _tokenProvider = tokenProvider;
            _timeProvider = timeProvider;
            _emailService = emailService;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginDto dto)
        {
            var user = await _userRepository.Query()
                .ByEmail(dto.Email)
                .IsNotRemoved()
                .OnlyActive()
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (user == null)
                throw new NotFoundException($"User with email '{dto.Email}' not found");

            if (!user.CheckPasswordValidity(dto.Password))
                throw new UnauthorizedException($"Wrong password for user '{dto.Email}'");

            return GenerateLoginResponse(user);
        }

        public async Task RegisterAsync(RegisterUserDto dto)
        {
            var existing = await _userRepository.IsExistingAsync(dto.Email);
            if (existing)
                throw new NotFoundException($"User with email '{dto.Email}' already exists");

            var user = new User(dto.Email, dto.Password, UserRole.Customer)
            {
                ResetPermissionsToken = new Guid().ToString(),
                ResetPermissionsTokenExpiration = _timeProvider.UtcNow.AddDays(1)
            };

            // TODO: get current url

            var isSend = await _emailService.SendRegisterCodeEmail(user.Email, "", user.ResetPermissionsToken);
            if (!isSend)
                throw new BadRequestException($"Sending Email Failed To Address '{user.Email}'");

            await _userRepository.Insert(user).SaveChangesAsync();
        }

        public async Task ConfirmRegisterAsync(string email, string token)
        {
            var user = await _userRepository.Query()
                .ByEmail(email)
                .IsNotRemoved()
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (user == null)
                throw new NotFoundException($"User with email '{email}' not found");

            VerifyResetPermissionsToken(user, token);

            user.Activate();
            await _userRepository.Update(user).SaveChangesAsync();
        }

        public async Task ForgotPasswordAsync(ForgotPasswordDto dto)
        {
            var user = await _userRepository.Query()
                .ByEmail(dto.Email)
                .OnlyActive()
                .FirstOrDefaultAsync();

            if (user == null)
                throw new NotFoundException($"User with email '{dto.Email}' not found");

            var generator = new Random();
            user.ResetPermissionsToken = generator.Next(0, 999999).ToString("D6");
            user.ResetPermissionsTokenExpiration = _timeProvider.UtcNow.AddHours(2);

            await _userRepository.Update(user).SaveChangesAsync();

            var isSend = await _emailService.SendForgotPasswordEmail(user.Email, user.ResetPermissionsToken);

            if (!isSend)
                throw new UnhandledException($"Sending Email Failed To Address '{user.Email}'");
        }

        public async Task ResetPasswordAsync(ResetPasswordDto dto)
        {
            var user = await _userRepository.Query()
                .ByEmail(dto.Email)
                .OnlyActive()
                .FirstOrDefaultAsync();

            if (user == null)
                throw new NotFoundException($"User with email '{dto.Email}' not found");

            VerifyResetPermissionsToken(user, dto.Token);

            user.SetPassword(dto.Password);
            user.ResetPermissionsTokenExpiration = _timeProvider.UtcNow;

            await _userRepository.Update(user).SaveChangesAsync();
        }

        private LoginResponseDto GenerateLoginResponse(User user)
        {
            var customClaims = new List<Claim>();
            customClaims.Add(new Claim(CustomClaimTypes.UserId, user.Id.ToString()));

            var token = _tokenProvider.Create(user.Id.ToString(), _timeProvider, user.RolesList, customClaims.ToArray());

            var result = new LoginResponseDto(user.ToDto(), token);

            return result;
        }

        private void VerifyResetPermissionsToken(User user, string token)
        {
            if (user.ResetPermissionsTokenExpiration < _timeProvider.UtcNow)
                throw new BadRequestException("Token Expiration Time Is Up");

            if (string.IsNullOrWhiteSpace(token) || user.ResetPermissionsToken != token)
                throw new BadRequestException("Invalid Token");
        }
    }
}