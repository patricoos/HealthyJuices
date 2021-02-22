using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HealthyJuices.Application.Mappers;
using HealthyJuices.Common.Exceptions;
using HealthyJuices.Domain.Models.Companies;
using HealthyJuices.Domain.Models.Companies.DataAccess;
using HealthyJuices.Shared.Dto;

namespace HealthyJuices.Application.Services
{
    public class CompaniesService
    {
        private readonly ICompanyRepository _companyRepository;

        public CompaniesService(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        public async Task<List<CompanyDto>> GetAllAsync()
        {
            var entities = await _companyRepository.Query()
                .ToListAsync();

            var result = entities
                .Select(x => x.ToDto())
                .ToList();

            return result;
        }

        public async Task<List<CompanyDto>> GetAllActiveAsync()
        {
            var entities = await _companyRepository.Query()
                .IsNotRemoved()
                .ToListAsync();

            var result = entities
                .Select(x => x.ToDto())
                .ToList();

            return result;
        }

        public async Task<CompanyDto> GetByIdAsync(string id)
        {
            var entity = await _companyRepository.Query()
                .ById(id)
                .FirstOrDefaultAsync();

            var result = entity?.ToDto();

            return result;
        }

        public async Task<string> CreateAsync(CompanyDto dto)
        {
            var entity = new Company(dto.Name, dto.Comment, dto.PostalCode, dto.City, dto.Street, dto.Latitude, dto.Longitude);

            _companyRepository.Insert(entity);
            await _companyRepository.SaveChangesAsync();

            return entity.Id;
        }

        public async Task UpdateAsync(CompanyDto dto)
        {
            var company = await _companyRepository.Query()
                .ById(dto.Id)
                .FirstOrDefaultAsync();

            if (company == null)
                throw new BadRequestException($"Not found company with id: {dto.Id}");

            company.Update(dto.Name, dto.Comment, dto.PostalCode, dto.City, dto.Street, dto.Latitude, dto.Longitude);

            _companyRepository.Update(company);
            await _companyRepository.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(string id)
        {
            var company = await _companyRepository.Query()
                .ById(id)
                .FirstOrDefaultAsync();

            if (company == null)
                throw new BadRequestException($"Not found company with id: {id}");

            company.Remove();

            _companyRepository.Update(company);
            await _companyRepository.SaveChangesAsync();
        }
    }
}