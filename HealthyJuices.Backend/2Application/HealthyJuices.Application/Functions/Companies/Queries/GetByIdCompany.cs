﻿using System.Threading;
using System.Threading.Tasks;
using HealthyJuices.Application.Mappers;
using HealthyJuices.Common.Exceptions;
using HealthyJuices.Domain.Models.Companies.DataAccess;
using HealthyJuices.Shared.Dto;
using MediatR;

namespace HealthyJuices.Application.Functions.Companies.Queries
{
    public static class GetByIdCompany
    {
        // Query 
        public record Query(string Id) : IRequest<CompanyDto> { }


        // Handler
        public class Handler : IRequestHandler<Query, CompanyDto>
        {
            private readonly ICompanyRepository _companyRepository;

            public Handler(ICompanyRepository repository)
            {
                this._companyRepository = repository;
            }

            public async Task<CompanyDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var entity = await _companyRepository.GetByIdAsync(request.Id);

                if (entity == null)
                    throw new BadRequestException($"Not found company with id: {request.Id}");

                return entity.ToDto();
            }
        }
    }
}