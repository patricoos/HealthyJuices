using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using HealthyJuices.Common.Exceptions;
using HealthyJuices.Domain.Models.Users;
using HealthyJuices.Domain.Models.Users.DataAccess;
using HealthyJuices.Shared.Enums;
using MediatR;

namespace HealthyJuices.Application.Features.Users.Commands
{
    public abstract class CreateUser
    {
        // Command 
        public record Command(string Id, string Email, string Password, UserRole Roles) : IRequest<string> { }

        // Validator
        public class Validator : AbstractValidator<Command>
        {
            private readonly IUserRepository _userRepository;

            public Validator(IUserRepository userRepository)
            {
                _userRepository = userRepository;
                RuleFor(v => v.Email)
                    .EmailAddress()
                    .MustAsync(BeUniqueEmail).WithMessage("Email already taken.");

                RuleFor(v => v.Password)
                    .MinimumLength(4).WithMessage("Password must be at least 4 characters.");
            }

            public async Task<bool> BeUniqueEmail(string email, CancellationToken cancellationToken)
            {
                var existing = await _userRepository.IsExistingAsync(email);
                return !existing;
            }
        }

        // Handler
        public class Handler : IRequestHandler<Command, string>
        {
            private readonly IUserRepository _userRepository;

            public Handler( IUserRepository userRepository)
            {
                _userRepository = userRepository;
            }

            public async Task<string> Handle(Command request, CancellationToken cancellationToken)
            {
                if (await _userRepository.IsExistingAsync(request.Email))
                    throw new BadRequestException($"User '{request.Email}' already existing");

                var user = new User(request.Email, request.Password, request.Roles);
                await _userRepository.Insert(user).SaveChangesAsync();

                return user.Id;
            }
        }
    }
}
