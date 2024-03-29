﻿using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using HealthyJuices.Application.Mappers;
using HealthyJuices.Common.Exceptions;
using HealthyJuices.Domain.Models.Users.DataAccess;
using HealthyJuices.Shared.Dto;
using MediatR;

namespace HealthyJuices.Application.Features.Users.Queries
{
    public abstract class GetByEmailUser
    {
        // Query 
        public record Query(string Email) : IRequest<UserDto> { }

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
                var entity = await _userRepository.GetByEmailAsync(request.Email);

                if (entity == null)
                    throw new BadRequestException($"Not found user with email: {request.Email}");

                return _mapper.Map<UserDto>(entity);
            }
        }
    }
}