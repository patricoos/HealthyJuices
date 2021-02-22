using System;
using System.Threading.Tasks;
using HealthyJuices.Application.Auth;
using HealthyJuices.Application.Mappers;
using HealthyJuices.Common.Contracts;
using HealthyJuices.Common.Exceptions;
using HealthyJuices.Domain.Models.Companies.DataAccess;
using HealthyJuices.Domain.Models.Users;
using HealthyJuices.Domain.Models.Users.DataAccess;
using HealthyJuices.Domain.Providers;
using HealthyJuices.Shared.Dto;
using HealthyJuices.Shared.Dto.Auth;
using HealthyJuices.Shared.Enums;

namespace HealthyJuices.Application.Services
{
    public class AuthorizationService
    {
        private readonly IUserRepository _userRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly SimpleTokenProvider _tokenProvider;
        private readonly ITimeProvider _timeProvider;
        private readonly EmailProvider _emailProvider;

        public AuthorizationService(IUserRepository userRepository, SimpleTokenProvider tokenProvider, ITimeProvider timeProvider, EmailProvider emailProvider, ICompanyRepository companyRepository)
        {
            _userRepository = userRepository;
            _tokenProvider = tokenProvider;
            _timeProvider = timeProvider;
            _emailProvider = emailProvider;
            _companyRepository = companyRepository;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginDto dto)
        {
            var user = await _userRepository.Query()
                .ByEmail(dto.Email)
                .IsNotRemoved()
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (user == null)
                throw new BadRequestException($"User with email '{dto.Email}' not found");

            if (!user.IsActive)
                throw new BadRequestException($"User with email '{dto.Email}' is not activated");

            if (!user.CheckPasswordValidity(dto.Password))
                throw new UnauthorizedException($"Wrong password for user '{dto.Email}'");

            var token = _tokenProvider.Create(user.Id.ToString(), _timeProvider, user.RolesList);
            return new LoginResponseDto(user.ToDto(), token);
        }

        public async Task<LoginResponseDto> LoginWithRefreshTokenAsync(LoginDto dto, string ipAddress)
        {
            var result = await this.LoginAsync(dto);
            var refreshToken = _tokenProvider.CreateRefreshToken(ipAddress);
            result.RefreshToken = refreshToken.Token;

            //  var user = await _userRepository.Query().ByEmail(dto.Email).FirstOrDefaultAsync();
            // save refresh token
            //  user.RefreshTokens.Add(refreshToken);
            //  await _userRepository.Update(user).SaveChangesAsync();

            return result;
        }

        public async Task<LoginResponseDto> RefreshTokenAsync(string refreshToken, string ipAddress)
        {
            var user = await _userRepository.Query()
                // .SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));
                .IsNotRemoved()
                .IsActive()
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (user == null)
                throw new UnauthorizedException("Token not found");

            //var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

            //if (!refreshToken.IsActive) return null;
            //if (user == null)
            //    throw new NotFoundException("Token not found");

            //var newRefreshToken = _tokenProvider.CreateRefreshToken(ipAddress);

            //refreshToken.Revoked = DateTime.UtcNow;
            //refreshToken.RevokedByIp = ipAddress;
            //refreshToken.ReplacedByToken = newRefreshToken.Token;
            // user.RefreshTokens.Add(newRefreshToken);
            // await _userRepository.Update(user).SaveChangesAsync();

            var token = _tokenProvider.Create(user.Id.ToString(), _timeProvider, user.RolesList);
            return new LoginResponseDto(user.ToDto(), token);
        }

        public async Task RevokeTokenAsync(string token, string ipAddress)
        {
            var user = await _userRepository.Query()
            // .SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));
                .IsNotRemoved()
                .IsActive()
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (user == null)
                throw new BadRequestException("Token not found");

            //var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

            //if (!refreshToken.IsActive)
            //        throw new NotFoundException("Token not found");

            //refreshToken.Revoked = DateTime.UtcNow;
            //refreshToken.RevokedByIp = ipAddress;

            //  await _userRepository.Update(user).SaveChangesAsync();
        }


        public async Task RegisterAsync(RegisterUserDto dto)
        {
            var existing = await _userRepository.IsExistingAsync(dto.Email);
            if (existing)
                throw new BadRequestException($"User with email '{dto.Email}' already exists");

            var company = await _companyRepository.Query().ById(dto.CompanyId).FirstOrDefaultAsync();
            if (company == null)
                throw new BadRequestException($"Company not found");

            var user = new User(dto.Email, dto.Password, dto.FirstName, dto.LastName, company, UserRole.Customer);
            user.SetResetPermissionsToken(Guid.NewGuid().ToString(), _timeProvider.UtcNow.AddDays(1));

            // TODO: get current url

            await _emailProvider.SendRegisterCodeEmail(user.Email, "http://localhost:4200/auth/confirm-register", user.ResetPermissionsToken);

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
                throw new BadRequestException($"User with email '{email}' not found");

            VerifyResetPermissionsToken(user, token);

            user.Activate();
            await _userRepository.Update(user).SaveChangesAsync();
        }

        public async Task ForgotPasswordAsync(ForgotPasswordDto dto)
        {
            var user = await _userRepository.Query()
                .ByEmail(dto.Email)
                .IsNotRemoved()
                .FirstOrDefaultAsync();

            if (user == null)
                throw new BadRequestException($"User with email '{dto.Email}' not found");

            user.SetResetPermissionsToken(Guid.NewGuid().ToString(), _timeProvider.UtcNow.AddDays(1));

            // TODO: get current url

            await _emailProvider.SendForgotPasswordEmail(user.Email, "http://localhost:4200/auth/reset-password", user.ResetPermissionsToken);
            await _userRepository.Update(user).SaveChangesAsync();
        }

        public async Task ResetPasswordAsync(ResetPasswordDto dto)
        {
            var user = await _userRepository.Query()
                .ByEmail(dto.Email)
                .IsNotRemoved()
                .FirstOrDefaultAsync();

            if (user == null)
                throw new BadRequestException($"User with email '{dto.Email}' not found");

            VerifyResetPermissionsToken(user, dto.Token);

            user.SetPassword(dto.Password);
            user.SetResetPermissionsToken(String.Empty, _timeProvider.UtcNow);

            await _userRepository.Update(user).SaveChangesAsync();
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