using System;
using System.Threading;
using System.Threading.Tasks;
using HealthyJuices.Application.Providers;
using HealthyJuices.Application.Wrappers;
using HealthyJuices.Common.Contracts;
using HealthyJuices.Common.Utils;
using HealthyJuices.Domain.Models.Users.DataAccess;

namespace HealthyJuices.Application.Services.Auth.Commands
{
    public static class ResetPassword
    {
        // Command 
        public record Command(string Email, string Token, string Password) : IRequestWrapper { }

        // Handler
        public class Handler : IHandlerWrapper<Command>
        {
            private readonly IUserRepository _userRepository;
            private readonly ITimeProvider _timeProvider;

            public Handler(IUserRepository repository, ITimeProvider timeProvider)
            {
                this._userRepository = repository;
                _timeProvider = timeProvider;
            }

            public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _userRepository.Query()
                    .ByEmail(request.Email)
                    .IsNotRemoved()
                    .FirstOrDefaultAsync();

                if (user == null)
                   return Response.Fail($"User with email '{request.Email}' not found");

                user.CheckPermissionsToken(_timeProvider, request.Token);

                user.SetPassword(request.Password);
                user.ResetPermissionsToken();

                await _userRepository.SaveChangesAsync();

                return Response.Success();
            }
        }
    }
}