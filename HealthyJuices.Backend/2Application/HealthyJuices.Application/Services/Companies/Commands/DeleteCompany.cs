using System.Threading;
using System.Threading.Tasks;
using HealthyJuices.Application.Wrappers;
using HealthyJuices.Common.Utils;
using HealthyJuices.Domain.Models.Companies.DataAccess;
using HealthyJuices.Shared.Enums;

namespace HealthyJuices.Application.Services.Companies.Commands
{
    public static class DeleteCompany
    {
        // Command 
        public record Command(string Id) : IRequestWrapper { }

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
                    return Response.Fail(ResponseStatus.NotFound, $"Not found Company with id: {request.Id}");

                entity.Remove();

                _companyRepository.Update(entity);
                await _companyRepository.SaveChangesAsync();

                return Response.Success();
            }
        }
    }
}