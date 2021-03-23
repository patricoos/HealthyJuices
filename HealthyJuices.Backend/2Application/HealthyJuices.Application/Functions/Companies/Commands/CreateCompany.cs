using System.Threading;
using System.Threading.Tasks;
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

        // Handler
        public class Handler : IRequestHandler<Command, string>
        {
            private readonly ICompanyWriteRepository _companyWriteRepository;

            public Handler(ICompanyWriteRepository writeRepository)
            {
                this._companyWriteRepository = writeRepository;
            }

            public async Task<string> Handle(Command request, CancellationToken cancellationToken)
            {
                var entity = new Company(request.Name, request.Comment, request.PostalCode, request.City, request.Street, request.Latitude, request.Longitude);

                _companyWriteRepository.Insert(entity);
                await _companyWriteRepository.SaveChangesAsync();

                return entity.Id;
            }
        }
    }
}
