using System.Threading;
using System.Threading.Tasks;
using HealthyJuices.Application.Auth;
using HealthyJuices.Application.Mappers;
using HealthyJuices.Common.Contracts;
using HealthyJuices.Common.Exceptions;
using HealthyJuices.Domain.Models.Users.DataAccess;
using HealthyJuices.Shared.Dto.Auth;
using MediatR;

namespace HealthyJuices.Application.Functions.Auth.Queries
{
    public static class Login
    {
        // Query 
        public record Query(string Email, string Password) : IRequest<LoginResponseDto> { }

        // Handler
        public class Handler : IRequestHandler<Query, LoginResponseDto>
        {
            private readonly IUserRepository _userRepository;
            private readonly SimpleTokenProvider _tokenProvider;
            private readonly ITimeProvider _timeProvider;
            public Handler(IUserRepository repository, SimpleTokenProvider tokenProvider, ITimeProvider timeProvider)
            {
                this._userRepository = repository;
                _tokenProvider = tokenProvider;
                _timeProvider = timeProvider;
            }
            public async Task<LoginResponseDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _userRepository.GetByEmailAsync(request.Email);

                if (user == null)
                    throw new BadRequestException($"User with email '{request.Email}' not found");

                if (!user.IsActive)
                    throw new BadRequestException($"User with email '{request.Email}' is not activated");

                if (!user.Password.CheckValidity(request.Password))
                    throw new BadRequestException($"Wrong password for user '{request.Email}'");

                var token = _tokenProvider.Create(user.Id, _timeProvider, user.RolesList);
                return new LoginResponseDto(user.ToDto(), token);
            }
        }
    }
}