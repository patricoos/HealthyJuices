﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HealthyJuices.Application.Mappers;
using HealthyJuices.Domain.Models.Companies.DataAccess;
using HealthyJuices.Shared.Dto;
using MediatR;

namespace HealthyJuices.Application.Functions.Companies.Queries
{
    public static class GetAllCompanies
    {
        // Query 
        public record Query : IRequest<IEnumerable<CompanyDto>> {}

        // Handler
        public class Handler : IRequestHandler<Query, IEnumerable<CompanyDto>>
        {
            private readonly ICompanyWriteRepository _companyWriteRepository;

            public Handler(ICompanyWriteRepository writeRepository)
            {
                this._companyWriteRepository = writeRepository;
            }

            public async Task<IEnumerable<CompanyDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var entities = await _companyWriteRepository.GetAllAsync();

                var result = entities
                    .Select(x => x.ToDto())
                    .ToList();

                return result;
            }
        }
    }
}