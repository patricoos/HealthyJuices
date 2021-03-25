using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using HealthyJuices.Application.Auth;
using HealthyJuices.Application.Mappers;
using HealthyJuices.Common.Contracts;
using HealthyJuices.Common.Exceptions;
using HealthyJuices.Domain.Models.Users.DataAccess;
using HealthyJuices.Shared.Dto.Auth;
using MediatR;

namespace HealthyJuices.Application.Functions.Auth.Queries
{
    public static class Login
    {
        // Query 
        public record Query(string Email, string Password) : IRequest<LoginResponseDto> { }

        // Validator
        public class Validator : AbstractValidator<Query>
        {
            private readonly IUserRepository _userRepository;

            public Validator(IUserRepository userRepository)
            {
                _userRepository = userRepository;
                RuleFor(v => v.Email)
                    .NotNull().WithMessage("Email is required.");

                RuleFor(v => v.Password)
                    .NotNull().WithMessage("Password is required.")
                    .MinimumLength(4).WithMessage("Password must be at least 4 characters.");
            }
        }

        // Handler
        public class Handler : IRequestHandler<Query, LoginResponseDto>
        {
            private readonly IUserRepository _userRepository;
            private readonly SimpleTokenProvider _tokenProvider;
            private readonly IDateTimeProvider _dateTimeProvider;
            public Handler(IUserRepository repository, SimpleTokenProvider tokenProvider, IDateTimeProvider dateTimeProvider)
            {
                this._userRepository = repository;
                _tokenProvider = tokenProvider;
                _dateTimeProvider = dateTimeProvider;
            }
            public async Task<LoginResponseDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _userRepository.GetByEmailAsync(request.Email);

                if (user == null)
                    throw new BadRequestException($"User with email '{request.Email}' not found");

                if (!user.IsActive)
                    throw new BadRequestException($"User with email '{request.Email}' is not activated");

                if (!user.Password.CheckValidity(request.Password))
                    throw new BadRequestException($"Wrong password for user '{request.Email}'");

                var token = _tokenProvider.Create(user.Id, _dateTimeProvider, user.RolesList);
                return new LoginResponseDto(user.ToDto(), token);
            }
        }
    }
}