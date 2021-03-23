using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HealthyJuices.Application.Mappers;
using HealthyJuices.Domain.Models.Users.DataAccess;
using HealthyJuices.Shared.Dto;
using HealthyJuices.Shared.Enums;
using MediatR;

namespace HealthyJuices.Application.Functions.Users.Queries
{
    public static class GetAllActiveByUserRoleUsers
    {
        // Query 
        public record Query(UserRole Role) : IRequest<IEnumerable<UserDto>> { }

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
                    .ByUserRole(request.Role)
                    .ToListAsync();

                var result = users.Select(x => x.ToDto())
                    .ToList();

                return result;
            }
        }
    }
}