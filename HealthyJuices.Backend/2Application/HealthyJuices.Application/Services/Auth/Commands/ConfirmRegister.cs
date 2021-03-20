using System.Threading;
using System.Threading.Tasks;
using HealthyJuices.Common.Contracts;
using HealthyJuices.Common.Exceptions;
using HealthyJuices.Domain.Models.Users.DataAccess;
using HealthyJuices.Shared.Enums;
using MediatR;

namespace HealthyJuices.Application.Services.Auth.Commands
{
    public static class ConfirmRegister
    {
        // Command 
        public record Command(string Email, string Token) : IRequest { }

        // Handler
        public class Handler : IRequestHandler<Command>
        {
            private readonly IUserRepository _userRepository;
            private readonly ITimeProvider _timeProvider;

            public Handler(IUserRepository repository, ITimeProvider timeProvider)
            {
                this._userRepository = repository;
                _timeProvider = timeProvider;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _userRepository.Query()
                    .ByEmail(request.Email)
                    .IsNotRemoved()
                    .FirstOrDefaultAsync();

                if (user == null)
                    throw new BadRequestException($"User with email '{request.Email}' not found");

                user.CheckPermissionsToken(_timeProvider, request.Token);
                user.ResetPermissionsToken();
                user.Activate();

                await _userRepository.SaveChangesAsync();

                return Unit.Value;
            }
        }
    }
}