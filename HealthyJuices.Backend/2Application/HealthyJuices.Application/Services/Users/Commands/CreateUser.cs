using System.Threading;
using System.Threading.Tasks;
using HealthyJuices.Common.Exceptions;
using HealthyJuices.Domain.Models.Users;
using HealthyJuices.Domain.Models.Users.DataAccess;
using HealthyJuices.Shared.Enums;
using MediatR;

namespace HealthyJuices.Application.Services.Users.Commands
{
    public static class CreateUser
    {
        // Command 
        public record Command(string Id, string Email, string Password, UserRole Roles) : IRequest<string> { }

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
