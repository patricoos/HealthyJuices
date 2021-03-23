using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HealthyJuices.Domain.Models.Users.DataAccess;
using HealthyJuices.Shared.Dto;
using MediatR;

namespace HealthyJuices.Application.Functions.Users.Queries
{
    public static class IsExistingUser
    {
        // Query 
        public record Query(string Email) : IRequest<bool> { }

        // Handler
        public class Handler : IRequestHandler<Query, bool>
        {
            private readonly IUserRepository _userRepository;

            public Handler(IUserRepository repository)
            {
                this._userRepository = repository;
            }

            public async Task<bool> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _userRepository.Query().ByEmail(request.Email).AnyAsync();
            }
        }
    }
}