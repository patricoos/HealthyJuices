using System.Threading;
using System.Threading.Tasks;
using HealthyJuices.Application.Mappers;
using HealthyJuices.Application.Wrappers;
using HealthyJuices.Common.Utils;
using HealthyJuices.Domain.Models.Companies.DataAccess;
using HealthyJuices.Shared.Dto;

namespace HealthyJuices.Application.Services.Companies.Queries
{
    public static class GetByIdCompany
    {
        // Query 
        public record Query(string Id) : IRequestWrapper<CompanyDto> { }


        // Handler
        public class Handler : IHandlerWrapper<Query, CompanyDto>
        {
            private readonly ICompanyRepository _companyRepository;

            public Handler(ICompanyRepository repository)
            {
                this._companyRepository = repository;
            }

            public async Task<Response<CompanyDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var entity = await _companyRepository.Query()
                    .ById(request.Id)
                    .FirstOrDefaultAsync();

                if (entity == null)
                    return Response<CompanyDto>.Fail<CompanyDto>($"Not found company with id: {request.Id}");

                return Response<CompanyDto>.Success(entity.ToDto());
            }
        }
    }
}