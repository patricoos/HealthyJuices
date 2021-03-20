using System.Threading;
using System.Threading.Tasks;
using HealthyJuices.Application.Auth;
using HealthyJuices.Application.Mappers;
using HealthyJuices.Application.Wrappers;
using HealthyJuices.Common.Contracts;
using HealthyJuices.Common.Utils;
using HealthyJuices.Domain.Models.Companies.DataAccess;
using HealthyJuices.Domain.Models.Users.DataAccess;
using HealthyJuices.Shared.Dto.Auth;
using HealthyJuices.Shared.Enums;

namespace HealthyJuices.Application.Services.Auth.Queries
{
    public static class Login
    {
        // Query 
        public record Query(string Email, string Password) : IRequestWrapper<LoginResponseDto> { }

        // Handler
        public class Handler : IHandlerWrapper<Query, LoginResponseDto>
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
            public async Task<Response<LoginResponseDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _userRepository.Query()
                    .ByEmail(request.Email)
                    .IsNotRemoved()
                    .AsNoTracking()
                    .FirstOrDefaultAsync();

                if (user == null)
                    return Response<LoginResponseDto>.Fail<LoginResponseDto>(ResponseStatus.NotFound, $"User with email '{request.Email}' not found");

                if (!user.IsActive)
                    return Response<LoginResponseDto>.Fail<LoginResponseDto>(ResponseStatus.ValidationError, $"User with email '{request.Email}' is not activated");

                if (!user.Password.CheckValidity(request.Password))
                    return Response<LoginResponseDto>.Fail<LoginResponseDto>(ResponseStatus.ValidationError, $"Wrong password for user '{request.Email}'");

                var token = _tokenProvider.Create(user.Id, _timeProvider, user.RolesList);
                return Response<LoginResponseDto>.Success(new LoginResponseDto(user.ToDto(), token));
            }
        }
    }
}