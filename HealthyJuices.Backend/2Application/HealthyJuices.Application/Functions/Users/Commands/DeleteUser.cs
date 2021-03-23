using System.Threading;
using System.Threading.Tasks;
using HealthyJuices.Common.Exceptions;
using HealthyJuices.Domain.Models.Users.DataAccess;
using MediatR;

namespace HealthyJuices.Application.Functions.Users.Commands
{
    public static class DeleteUser
    {
        // Command 
        public record Command(string Id) : IRequest { }

        // Handler
        public class Handler : IRequestHandler<Command>
        {
            private readonly IUserRepository _userRepository;

            public Handler(IUserRepository repository)
            {
                this._userRepository = repository;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var entity = await _userRepository.GetByIdAsync(request.Id);

                if (entity == null)
                    throw new BadRequestException($"Not found order with id: {request.Id}");

                entity.Remove();

                _userRepository.Update(entity);
                await _userRepository.SaveChangesAsync();

                return Unit.Value;
            }
        }
    }
}