using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using HealthyJuices.Application.Providers;
using HealthyJuices.Common.Contracts;
using HealthyJuices.Common.Exceptions;
using HealthyJuices.Domain.Models.Companies.DataAccess;
using HealthyJuices.Domain.Models.Users;
using HealthyJuices.Domain.Models.Users.DataAccess;
using HealthyJuices.Shared.Enums;
using MediatR;

namespace HealthyJuices.Application.Functions.Auth.Commands
{
    public static class Register
    {
        // Command 
        public record Command(string Email, string Password, string FirstName, string LastName, string CompanyId) : IRequest<string> { }

        // Validator
        public class Validator : AbstractValidator<Command>
        {
            private readonly IUserRepository _userRepository;

            public Validator(IUserRepository userRepository)
            {
                _userRepository = userRepository;
                RuleFor(v => v.Email)
                    .EmailAddress()
                    .MustAsync(BeUniqueEmail).WithMessage("Email title already exists.");

                RuleFor(v => v.Password)
                    .MinimumLength(4).WithMessage("Password must be at least 4 characters.");
            }

            public async Task<bool> BeUniqueEmail(string email, CancellationToken cancellationToken)
            {
                var existing = await _userRepository.IsExistingAsync(email);
                return !existing;
            }
        }

        // Handler
        public class Handler : IRequestHandler<Command, string>
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

            public async Task<string> Handle(Command request, CancellationToken cancellationToken)
            {
                var company = await _companyRepository.Query().ById(request.CompanyId).FirstOrDefaultAsync();
                if (company == null)
                    throw new BadRequestException($"Company not found");

                var user = new User(request.Email, request.Password, request.FirstName, request.LastName, company, UserRole.Customer);
                user.SetPermissionsToken(_timeProvider, Guid.NewGuid().ToString(), _timeProvider.UtcNow.AddDays(1));

                // TODO: get current url

                await _emailProvider.SendRegisterCodeEmail(user.Email, "http://localhost:4200/auth/confirm-register", user.PermissionsToken.Token);

                await _userRepository.Insert(user).SaveChangesAsync();
                return user.Id;
            }
        }
    }
}
