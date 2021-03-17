using System.Threading;
using System.Threading.Tasks;
using HealthyJuices.Application.Wrappers;
using HealthyJuices.Common.Utils;
using HealthyJuices.Domain.Models.Companies.DataAccess;
using HealthyJuices.Shared.Dto;

namespace HealthyJuices.Application.Services.Companies.Commands
{
    public static class UpdateCompany
    { 
        // Command 
        public record Command : CompanyDto, IRequestWrapper { }

        // Handler
        public class Handler : IHandlerWrapper<Command>
        {
            private readonly ICompanyRepository _companyRepository;

            public Handler(ICompanyRepository repository)
            {
                this._companyRepository = repository;
            }

            public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
            {
                var entity = await _companyRepository.Query()
                    .ById(request.Id)
                    .FirstOrDefaultAsync();

                if (entity == null)
                    return Response.Fail($"Not found product with id: {request.Id}");

                entity.Update(request.Name, request.Comment, request.PostalCode, request.City, request.Street, request.Latitude, request.Longitude);

                _companyRepository.Update(entity);
                await _companyRepository.SaveChangesAsync();

                return Response.Success();
            }
        }
    }
}