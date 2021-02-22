using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HealthyJuices.Application.Mappers;
using HealthyJuices.Common.Exceptions;
using HealthyJuices.Domain.Models.Unavailabilities;
using HealthyJuices.Domain.Models.Unavailabilities.DataAccess;
using HealthyJuices.Shared.Dto;

namespace HealthyJuices.Application.Services
{
    public class UnavailabilitiesService
    {
        private readonly IUnavailabilityRepository _unavailabilityRepository;

        public UnavailabilitiesService(IUnavailabilityRepository unavailabilityRepository)
        {
            _unavailabilityRepository = unavailabilityRepository;
        }

        public async Task<List<UnavailabilityDto>> GetAllAsync(DateTime? from = null, DateTime? to = null)
        {
            var query = _unavailabilityRepository.Query();
            if (from.HasValue)
                query.AfterDateTime(from.Value);

            if (to.HasValue)
                query.BeforeDateTime(to.Value);

            var entities = await query.ToListAsync();

            var result = entities
                .Select(x => x.ToDto())
                .ToList();

            return result;
        }

        public async Task<UnavailabilityDto> GetByIdAsync(string id)
        {
            var entity = await _unavailabilityRepository.Query()
                .ById(id)
                .FirstOrDefaultAsync();

            var result = entity?.ToDto();

            return result;
        }

        public async Task<string> CreateAsync(UnavailabilityDto dto)
        {
            var unavailability = new Unavailability(dto.From, dto.To, dto.Reason, dto.Comment);

            _unavailabilityRepository.Insert(unavailability);
            await _unavailabilityRepository.SaveChangesAsync();

            return unavailability.Id;
        }

        public async Task UpdateAsync(UnavailabilityDto dto)
        {
            var unavailability = await _unavailabilityRepository.Query()
                .ById(dto.Id)
                .FirstOrDefaultAsync();

            if (unavailability == null)
                throw new BadRequestException($"Not found product with id: {dto.Id}");

            unavailability.Update(dto.From, dto.To, dto.Reason, dto.Comment);

            _unavailabilityRepository.Update(unavailability);
            await _unavailabilityRepository.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(string id)
        {
            var unavailability = await _unavailabilityRepository.Query()
                .ById(id)
                .FirstOrDefaultAsync();

            if (unavailability == null)
                throw new BadRequestException($"Not found product with id: {id}");


            _unavailabilityRepository.Remove(unavailability);
            await _unavailabilityRepository.SaveChangesAsync();
        }
    }
}