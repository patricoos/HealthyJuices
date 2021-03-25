using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using HealthyJuices.Application.Mappers;
using HealthyJuices.Common.Exceptions;
using HealthyJuices.Domain.Models.Companies.DataAccess;
using HealthyJuices.Shared.Dto;
using HealthyJuices.Shared.Dto.Products;
using MediatR;

namespace HealthyJuices.Application.Features.Companies.Queries
{
    public static class GetByIdCompany
    {
        // Query 
        public record Query(string Id) : IRequest<CompanyDto> { }


        // Handler
        public class Handler : IRequestHandler<Query, CompanyDto>
        {
            private readonly ICompanyRepository _companyRepository;
            private readonly IMapper _mapper;

            public Handler(ICompanyRepository repository, IMapper mapper)
            {
                this._companyRepository = repository;
                _mapper = mapper;
            }

            public async Task<CompanyDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var entity = await _companyRepository.GetByIdAsync(request.Id);

                if (entity == null)
                    throw new BadRequestException($"Not found company with id: {request.Id}");

                return _mapper.Map<CompanyDto>(entity);
            }
        }
    }
}