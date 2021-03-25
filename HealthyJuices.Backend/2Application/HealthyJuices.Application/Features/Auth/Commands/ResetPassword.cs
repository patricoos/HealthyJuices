using System.Threading;
using System.Threading.Tasks;
using HealthyJuices.Common.Contracts;
using HealthyJuices.Common.Exceptions;
using HealthyJuices.Domain.Models.Users.DataAccess;
using MediatR;

namespace HealthyJuices.Application.Features.Auth.Commands
{
    public static class ResetPassword
    {
        // Command 
        public record Command(string Email, string Token, string Password) : IRequest { }

        // Handler
        public class Handler : IRequestHandler<Command>
        {
            private readonly IUserRepository _userRepository;
            private readonly IDateTimeProvider _dateTimeProvider;

            public Handler(IUserRepository repository, IDateTimeProvider dateTimeProvider)
            {
                this._userRepository = repository;
                _dateTimeProvider = dateTimeProvider;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _userRepository.GetByEmailAsync(request.Email, false);

                if (user == null || user.IsRemoved)
                    throw new BadRequestException($"User with email '{request.Email}' not found");

                user.CheckPermissionsToken(_dateTimeProvider, request.Token);

                user.SetPassword(request.Password);
                user.ResetPermissionsToken();

                await _userRepository.SaveChangesAsync();

                return Unit.Value;
            }
        }
    }
}