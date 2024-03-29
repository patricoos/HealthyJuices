﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using HealthyJuices.Domain.Models.Companies.DataAccess;
using HealthyJuices.Shared.Dto;
using MediatR;

namespace HealthyJuices.Application.Features.Companies.Queries
{
    public abstract class GetAllActiveCompanies
    {
        // Query 
        public record Query : IRequest<IEnumerable<CompanyDto>> { }

        // Handler
        public class Handler : IRequestHandler<Query, IEnumerable<CompanyDto>>
        {
            private readonly ICompanyRepository _companyRepository;
            private readonly IMapper _mapper;

            public Handler(ICompanyRepository repository, IMapper mapper)
            {
                this._companyRepository = repository;
                _mapper = mapper;
            }

            public async Task<IEnumerable<CompanyDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var entities = await _companyRepository.GetAllActiveAsync();
                return _mapper.Map<IEnumerable<CompanyDto>>(entities);
            }
        }
    }
}