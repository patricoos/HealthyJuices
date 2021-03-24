using System.Threading;
using System.Threading.Tasks;
using HealthyJuices.Common.Exceptions;
using HealthyJuices.Domain.Models.Companies.DataAccess;
using MediatR;

namespace HealthyJuices.Application.Functions.Companies.Commands
{
    public static class DeleteCompany
    {
        // Command 
        public record Command(string Id) : IRequest { }

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
                var entity = await _companyRepository.GetByIdAsync(request.Id);

                if (entity == null)
                    throw new BadRequestException($"Not found Company with id: {request.Id}");

                entity.Remove();

                _companyRepository.Update(entity);
                await _companyRepository.SaveChangesAsync();

                return Unit.Value;
            }
        }
    }
}