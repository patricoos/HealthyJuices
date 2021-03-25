using System;
using System.Threading;
using System.Threading.Tasks;
using HealthyJuices.Application.Providers;
using HealthyJuices.Common.Contracts;
using HealthyJuices.Common.Exceptions;
using HealthyJuices.Domain.Models.Users.DataAccess;
using MediatR;

namespace HealthyJuices.Application.Features.Auth.Commands
{
    public static class ForgotPassword
    {
        // Command 
        public record Command(string Email) : IRequest { }

        // Handler
        public class Handler : IRequestHandler<Command>
        {
            private readonly IUserRepository _userRepository;
            private readonly IDateTimeProvider _dateTimeProvider;
            private readonly EmailProvider _emailProvider;

            public Handler(IUserRepository repository, IDateTimeProvider dateTimeProvider, EmailProvider emailProvider)
            {
                this._userRepository = repository;
                _dateTimeProvider = dateTimeProvider;
                _emailProvider = emailProvider;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _userRepository.GetByEmailAsync(request.Email, false);

                if (user == null || user.IsRemoved)
                   throw new BadRequestException($"User with email '{request.Email}' not found");

                user.SetPermissionsToken(_dateTimeProvider, Guid.NewGuid().ToString(), _dateTimeProvider.UtcNow.AddDays(1));

                // TODO: get current url

                await _emailProvider.SendForgotPasswordEmail(user.Email, "http://localhost:4200/auth/reset-password", user.PermissionsToken.Token);
                await _userRepository.SaveChangesAsync();

                return Unit.Value;
            }
        }
    }
}