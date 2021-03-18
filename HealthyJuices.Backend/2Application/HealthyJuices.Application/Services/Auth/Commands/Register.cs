using System;
using System.Threading;
using System.Threading.Tasks;
using HealthyJuices.Application.Providers;
using HealthyJuices.Application.Wrappers;
using HealthyJuices.Common.Contracts;
using HealthyJuices.Common.Exceptions;
using HealthyJuices.Common.Utils;
using HealthyJuices.Domain.Models.Companies.DataAccess;
using HealthyJuices.Domain.Models.Users;
using HealthyJuices.Domain.Models.Users.DataAccess;
using HealthyJuices.Shared.Dto;
using HealthyJuices.Shared.Enums;

namespace HealthyJuices.Application.Services.Auth.Commands
{
    public static class Register
    {
        // Command 
        public record Command(string Email, string Password, string FirstName, string LastName, string CompanyId) : IRequestWrapper<string> { }

        // Handler
        public class Handler : IHandlerWrapper<Command, string>
        {
            private readonly EmailProvider _emailProvider;
            private readonly ITimeProvider _timeProvider;
            private readonly IUserRepository _userRepository;
            private readonly ICompanyRepository _companyRepository;

            public Handler(IUserRepository repository, EmailProvider emailProvider, ITimeProvider timeProvider, ICompanyRepository companyRepository)
            {
                this._userRepository = repository;
                _emailProvider = emailProvider;
                _timeProvider = timeProvider;
                _companyRepository = companyRepository;
            }

            public async Task<Response<string>> Handle(Command request, CancellationToken cancellationToken)
            {
                var existing = await _userRepository.IsExistingAsync(request.Email);
                if (existing)
                    throw new BadRequestException($"User with email '{request.Email}' already exists");

                var company = await _companyRepository.Query().ById(request.CompanyId).FirstOrDefaultAsync();
                if (company == null)
                    throw new BadRequestException($"Company not found");

                var user = new User(request.Email, request.Password, request.FirstName, request.LastName, company, UserRole.Customer);
                user.SetPermissionsToken(_timeProvider, Guid.NewGuid().ToString(), _timeProvider.UtcNow.AddDays(1));

                // TODO: get current url

                await _emailProvider.SendRegisterCodeEmail(user.Email, "http://localhost:4200/auth/confirm-register", user.PermissionsToken.Token);

                await _userRepository.Insert(user).SaveChangesAsync();
                return Response<string>.Success(user.Id);
            }
        }
    }
}
