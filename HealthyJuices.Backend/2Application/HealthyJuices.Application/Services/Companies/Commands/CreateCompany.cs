using System.Threading;
using System.Threading.Tasks;
using HealthyJuices.Domain.Models.Companies;
using HealthyJuices.Domain.Models.Companies.DataAccess;
using HealthyJuices.Shared.Dto;
using MediatR;

namespace HealthyJuices.Application.Services.Companies.Commands
{
    public static class CreateCompany
    {
        // Command 
        public record Command : CompanyDto, IRequest<string> { }

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
