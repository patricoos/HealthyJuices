using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using HealthyJuices.Domain.Models.Companies;
using HealthyJuices.Domain.Models.Companies.DataAccess;
using HealthyJuices.Shared.Dto;
using MediatR;

namespace HealthyJuices.Application.Functions.Companies.Commands
{
    public static class CreateCompany
    {
        // Command 
        public record Command : CompanyDto, IRequest<string> { }

        // Validator
        public class Validator : AbstractValidator<Command>
        {
            private readonly ICompanyRepository _companyRepository;

            public Validator(ICompanyRepository companyRepository)
            {
                _companyRepository = companyRepository;
                RuleFor(v => v.Name)
                    .NotNull().WithMessage("Name is required.")
                    .EmailAddress();

                RuleFor(v => v.PostalCode)
                    .NotNull().WithMessage("PostalCode is required.")
                    .MinimumLength(4).WithMessage("PostalCode must be at least 4 characters.");

                RuleFor(v => v.City)
                    .NotNull().WithMessage("City is required.")
                    .MinimumLength(2).WithMessage("City must be at least 2 characters.");

                RuleFor(v => v.Street)
                    .NotNull().WithMessage("Street is required.")
                    .MinimumLength(2).WithMessage("Street must be at least 2 characters.");

                RuleFor(v => v.Latitude)
                    .NotNull().WithMessage("Latitude is required.");

                RuleFor(v => v.Longitude)
                    .NotNull().WithMessage("Longitude is required.");
            }
        }

        // Handler
        public class Handler : IRequestHandler<Command, string>
        {
            private readonly ICompanyRepository _companyRepository;

            public Handler(ICompanyRepository repository)
            {
                this._companyRepository = repository;
            }

            public async Task<string> Handle(Command request, CancellationToken cancellationToken)
            {
                var entity = new Company(request.Name, request.Comment, request.PostalCode, request.City, request.Street, request.Latitude, request.Longitude);

                _companyRepository.Insert(entity);
                await _companyRepository.SaveChangesAsync();

                return entity.Id;
            }
        }
    }
}
