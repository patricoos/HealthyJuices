using System.Threading;
using System.Threading.Tasks;
using HealthyJuices.Application.Wrappers;
using HealthyJuices.Common.Utils;
using HealthyJuices.Domain.Models.Companies;
using HealthyJuices.Domain.Models.Companies.DataAccess;
using HealthyJuices.Domain.Models.Products;
using HealthyJuices.Domain.Models.Products.DataAccess;
using HealthyJuices.Shared.Dto;

namespace HealthyJuices.Application.Services.Companies.Commands
{
    public static class CreateCompany
    {
        // Command 
        public record Command : CompanyDto, IRequestWrapper<string> { }

        // Handler
        public class Handler : IHandlerWrapper<Command, string>
        {
            private readonly ICompanyRepository _companyRepository;

            public Handler(ICompanyRepository repository)
            {
                this._companyRepository = repository;
            }

            public async Task<Response<string>> Handle(Command request, CancellationToken cancellationToken)
            {
                var entity = new Company(request.Name, request.Comment, request.PostalCode, request.City, request.Street, request.Latitude, request.Longitude);

                _companyRepository.Insert(entity);
                await _companyRepository.SaveChangesAsync();

                return Response<string>.Success(entity.Id);
            }
        }
    }
}
