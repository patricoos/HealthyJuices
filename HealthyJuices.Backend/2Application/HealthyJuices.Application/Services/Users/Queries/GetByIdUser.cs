using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HealthyJuices.Application.Mappers;
using HealthyJuices.Application.Wrappers;
using HealthyJuices.Common.Utils;
using HealthyJuices.Domain.Models.Users.DataAccess;
using HealthyJuices.Shared.Dto;
using MediatR;

namespace HealthyJuices.Application.Services.Users.Queries
{
    public static class GetByIdUser
    {
        // Query 
        public record Query(string Id) : IRequestWrapper<UserDto> { }

        // Handler
        public class Handler : IHandlerWrapper<Query, UserDto>
        {
            private readonly IUserRepository _userRepository;

            public Handler(IUserRepository repository)
            {
                this._userRepository = repository;
            }

            public async Task<Response<UserDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var entity = await _userRepository.Query()
                    .ById(request.Id)
                    .FirstOrDefaultAsync();

                if (entity == null)
                    return Response<UserDto>.Fail<UserDto>($"Not found user with id: {request.Id}");

                return Response<UserDto>.Success(entity.ToDto());
            }
        }
    }
}