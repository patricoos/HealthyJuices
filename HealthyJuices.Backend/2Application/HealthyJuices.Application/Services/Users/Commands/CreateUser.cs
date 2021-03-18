using System.Threading;
using System.Threading.Tasks;
using HealthyJuices.Application.Wrappers;
using HealthyJuices.Common.Utils;
using HealthyJuices.Domain.Models.Users;
using HealthyJuices.Domain.Models.Users.DataAccess;
using HealthyJuices.Shared.Enums;

namespace HealthyJuices.Application.Services.Users.Commands
{
    public static class CreateUser
    {
        // Command 
        public record Command(string Id, string Email, string Password, UserRole Roles) : IRequestWrapper<string> { }

        // Handler
        public class Handler : IHandlerWrapper<Command, string>
        {
            private readonly IUserRepository _userRepository;

            public Handler( IUserRepository userRepository)
            {
                _userRepository = userRepository;
            }

            public async Task<Response<string>> Handle(Command request, CancellationToken cancellationToken)
            {
                if (await _userRepository.IsExistingAsync(request.Email))
                   return Response<string>.Fail<string>($"User '{request.Email}' already existing");

                var user = new User(request.Email, request.Password, request.Roles);
                await _userRepository.Insert(user).SaveChangesAsync();

                return Response<string>.Success(user.Id);
            }
        }
    }
}
