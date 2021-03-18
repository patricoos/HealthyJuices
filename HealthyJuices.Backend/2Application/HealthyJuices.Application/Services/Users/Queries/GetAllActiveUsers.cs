using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HealthyJuices.Application.Mappers;
using HealthyJuices.Domain.Models.Users.DataAccess;
using HealthyJuices.Shared.Dto;
using MediatR;

namespace HealthyJuices.Application.Services.Users.Queries
{
    public static class GetAllActiveUsers
    {
        // Query 
        public record Query : IRequest<IEnumerable<UserDto>> { }

        // Handler
        public class Handler : IRequestHandler<Query, IEnumerable<UserDto>>
        {
            private readonly IUserRepository _userRepository;

            public Handler(IUserRepository repository)
            {
                this._userRepository = repository;
            }

            public async Task<IEnumerable<UserDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var users = await _userRepository.Query()
                    .IsActive()
                    .IsNotRemoved()
                    .ToListAsync();

                var result = users.Select(x => x.ToDto())
                    .ToList();

                return result;
            }
        }
    }
}