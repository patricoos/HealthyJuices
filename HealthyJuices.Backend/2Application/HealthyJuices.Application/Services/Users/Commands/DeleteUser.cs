using System.Threading;
using System.Threading.Tasks;
using HealthyJuices.Application.Wrappers;
using HealthyJuices.Common.Utils;
using HealthyJuices.Domain.Models.Users.DataAccess;

namespace HealthyJuices.Application.Services.Users.Commands
{
    public static class DeleteUser
    {
        // Command 
        public record Command(string Id) : IRequestWrapper { }

        // Handler
        public class Handler : IHandlerWrapper<Command>
        {
            private readonly IUserRepository _userRepository;

            public Handler(IUserRepository repository)
            {
                this._userRepository = repository;
            }

            public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
            {
                var entity = await _userRepository.Query()
                    .ById(request.Id)
                    .FirstOrDefaultAsync();

                if (entity == null)
                    return Response.Fail($"Not found order with id: {request.Id}");

                entity.Remove();

                _userRepository.Update(entity);
                await _userRepository.SaveChangesAsync();

                return Response.Success();
            }
        }
    }
}