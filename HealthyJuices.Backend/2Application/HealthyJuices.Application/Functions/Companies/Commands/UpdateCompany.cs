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
            private readonly ICompanyWriteRepository _companyWriteRepository;

            public Handler(ICompanyWriteRepository writeRepository)
            {
                this._companyWriteRepository = writeRepository;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var entity = await _companyWriteRepository.GetByIdAsync(request.Id);

                if (entity == null)
                    throw new BadRequestException($"Not found Company with id: {request.Id}");

                entity.Update(request.Name, request.Comment, request.PostalCode, request.City, request.Street, request.Latitude, request.Longitude);

                _companyWriteRepository.Update(entity);
                await _companyWriteRepository.SaveChangesAsync();

                return Unit.Value;
            }
        }
    }
}