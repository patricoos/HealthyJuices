using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using HealthyJuices.Domain.Models.Users.DataAccess;
using HealthyJuices.Shared.Dto;
using MediatR;

namespace HealthyJuices.Application.Features.Users.Queries
{
    public static class GetAllActiveUsers
    {
        // Query 
        public record Query : IRequest<IEnumerable<UserDto>> { }

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
                var users = await _userRepository.GetAllActiveAsync();
                return _mapper.Map<IEnumerable<UserDto>>(users);
            }
        }
    }
}