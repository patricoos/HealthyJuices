using System.Threading;
using System.Threading.Tasks;
using HealthyJuices.Application.Mappers;
using HealthyJuices.Common.Exceptions;
using HealthyJuices.Domain.Models.Users.DataAccess;
using HealthyJuices.Shared.Dto;
using MediatR;

namespace HealthyJuices.Application.Functions.Users.Queries
{
    public static class GetByEmailUser
    {
        // Query 
        public record Query(string Email) : IRequest<UserDto> { }

        // Handler
        public class Handler : IRequestHandler<Query, UserDto>
        {
            private readonly IUserRepository _userRepository;

            public Handler(IUserRepository repository)
            {
                this._userRepository = repository;
            }

            public async Task<UserDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var entity = await _userRepository.GetByEmailAsync(request.Email);

                if (entity == null)
                    throw new BadRequestException($"Not found user with email: {request.Email}");

                return entity.ToDto();
            }
        }
    }
}