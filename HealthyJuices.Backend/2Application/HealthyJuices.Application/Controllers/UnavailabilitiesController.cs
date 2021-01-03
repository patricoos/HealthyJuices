using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HealthyJuices.Application.Mappers;
using HealthyJuices.Common.Exceptions;
using HealthyJuices.Domain.Models.Unavailabilities;
using HealthyJuices.Domain.Models.Unavailabilities.DataAccess;
using HealthyJuices.Shared.Dto;

namespace HealthyJuices.Application.Controllers
{
    public class UnavailabilitiesController
    {
        private readonly IUnavailabilityRepository _unavailabilityRepository;

        public UnavailabilitiesController(IUnavailabilityRepository unavailabilityRepository)
        {
            _unavailabilityRepository = unavailabilityRepository;
        }

        public async Task<List<UnavailabilityDto>> GetAllAsync()
        {
            var entities = await _unavailabilityRepository.Query()
                .ToListAsync();

            var result = entities
                .Select(x => x.ToDto())
                .ToList();

            return result;
        }

        public async Task<UnavailabilityDto> GetByIdAsync(long id)
        {
            var entity = await _unavailabilityRepository.Query()
                .ById(id)
                .FirstOrDefaultAsync();

            var result = entity?.ToDto();

            return result;
        }

        public async Task<long> CreateAsync(UnavailabilityDto dto)
        {
            var unavailability = new Unavailability(dto.From, dto.To, dto.Reason, dto.Comment);

            _unavailabilityRepository.Insert(unavailability);
            await _unavailabilityRepository.SaveChangesAsync();

            return unavailability.Id;
        }

        public async Task UpdateAsync(UnavailabilityDto dto)
        {
            var unavailability = await _unavailabilityRepository.Query()
                .ById(dto.Id.Value)
                .FirstOrDefaultAsync();

            if (unavailability == null)
                throw new BadRequestException($"Not found product with id: {dto.Id}");

            unavailability.Update(dto.From, dto.To, dto.Reason, dto.Comment);

            _unavailabilityRepository.Update(unavailability);
            await _unavailabilityRepository.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(long id)
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