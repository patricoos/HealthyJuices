using System;
using System.Threading;
using System.Threading.Tasks;
using HealthyJuices.Application.Providers;
using HealthyJuices.Application.Wrappers;
using HealthyJuices.Common.Contracts;
using HealthyJuices.Common.Utils;
using HealthyJuices.Domain.Models.Users.DataAccess;
using HealthyJuices.Shared.Enums;

namespace HealthyJuices.Application.Services.Auth.Commands
{
    public static class ForgotPassword
    {
        // Command 
        public record Command(string Email) : IRequestWrapper { }

        // Handler
        public class Handler : IHandlerWrapper<Command>
        {
            private readonly IUserRepository _userRepository;
            private readonly ITimeProvider _timeProvider;
            private readonly EmailProvider _emailProvider;

            public Handler(IUserRepository repository, ITimeProvider timeProvider, EmailProvider emailProvider)
            {
                this._userRepository = repository;
                _timeProvider = timeProvider;
                _emailProvider = emailProvider;
            }

            public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _userRepository.Query()
                    .ByEmail(request.Email)
                    .IsNotRemoved()
                    .FirstOrDefaultAsync();

                if (user == null)
                    return Response.Fail(ResponseStatus.NotFound, $"User with email '{request.Email}' not found");

                user.SetPermissionsToken(_timeProvider, Guid.NewGuid().ToString(), _timeProvider.UtcNow.AddDays(1));

                // TODO: get current url

                await _emailProvider.SendForgotPasswordEmail(user.Email, "http://localhost:4200/auth/reset-password", user.PermissionsToken.Token);
                await _userRepository.SaveChangesAsync();

                return Response.Success();
            }
        }
    }
}