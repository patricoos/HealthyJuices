using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HealthyJuices.Application.Mappers;
using HealthyJuices.Domain.Models.Companies.DataAccess;
using HealthyJuices.Shared.Dto;
using MediatR;

namespace HealthyJuices.Application.Services.Companies.Queries
{
    public static class GetAllActiveCompanies
    {
        // Query 
        public record Query : IRequest<IEnumerable<CompanyDto>> { }

        // Handler
        public class Handler : IRequestHandler<Query, IEnumerable<CompanyDto>>
        {
            private readonly ICompanyRepository _companyRepository;

            public Handler(ICompanyRepository repository)
            {
                this._companyRepository = repository;
            }

            public async Task<IEnumerable<CompanyDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var entities = await _companyRepository.Query()
                    .IsNotRemoved()
                    .ToListAsync();

                var result = entities
                    .Select(x => x.ToDto())
                    .ToList();

                return result;
            }
        }
    }
}