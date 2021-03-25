using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using HealthyJuices.Application.Mappers;
using HealthyJuices.Common.Exceptions;
using HealthyJuices.Domain.Models.Users.DataAccess;
using HealthyJuices.Shared.Dto;
using MediatR;

namespace HealthyJuices.Application.Features.Users.Queries
{
    public static class GetByIdUser
    {
        // Query 
        public record Query(string Id) : IRequest<UserDto> { }

        // Handler
        public class Handler : IRequestHandler<Query, UserDto>
        {
            private readonly IUserRepository _userRepository;
            private readonly IMapper _mapper;

            public Handler(IUserRepository repository, IMapper mapper)
            {
                this._userRepository = repository;
                _mapper = mapper;
            }

            public async Task<UserDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var entity = await _userRepository.GetByIdAsync(request.Id);

                if (entity == null)
                    throw new BadRequestException($"Not found user with id: {request.Id}");

                return _mapper.Map<UserDto>(entity);
            }
        }
    }
}