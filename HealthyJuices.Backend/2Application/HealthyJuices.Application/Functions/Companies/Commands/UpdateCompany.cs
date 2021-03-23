using System.Threading;
using System.Threading.Tasks;
using HealthyJuices.Common.Exceptions;
using HealthyJuices.Domain.Models.Companies.DataAccess;
using HealthyJuices.Shared.Dto;
using MediatR;

namespace HealthyJuices.Application.Functions.Companies.Commands
{
    public static class UpdateCompany
    { 
        // Command 
        public record Command : CompanyDto, IRequest { }

        // Handler
        public class Handler : IRequestHandler<Command>
        {
            private readonly ICompanyRepository _companyRepository;

            public Handler(ICompanyRepository repository)
            {
                this._companyRepository = repository;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var entity = await _companyRepository.Query()
                    .ById(request.Id)
                    .FirstOrDefaultAsync();

                if (entity == null)
                    throw new BadRequestException($"Not found Company with id: {request.Id}");

                entity.Update(request.Name, request.Comment, request.PostalCode, request.City, request.Street, request.Latitude, request.Longitude);

                _companyRepository.Update(entity);
                await _companyRepository.SaveChangesAsync();

                return Unit.Value;
            }
        }
    }
}