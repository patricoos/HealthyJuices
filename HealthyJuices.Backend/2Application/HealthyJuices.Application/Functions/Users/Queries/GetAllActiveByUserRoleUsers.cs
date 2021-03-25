using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
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
            private readonly IMapper _mapper;

            public Handler(IUserRepository repository, IMapper mapper)
            {
                this._userRepository = repository;
                _mapper = mapper;
            }

            public async Task<IEnumerable<UserDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var users = await _userRepository.GetAllActiveByUserRoleAsync(request.Role);
                return _mapper.Map<IEnumerable<UserDto>>(users);
            }
        }
    }
}