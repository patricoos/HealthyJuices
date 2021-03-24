using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using HealthyJuices.Domain.Models.Abstraction.DataAccess.Repositories;

namespace HealthyJuices.Domain.Models.Unavailabilities.DataAccess
{
    public interface IUnavailabilityRepository : IWriteRepository<Unavailability>, IReadRepository<Unavailability>
    {
        Task<IEnumerable<Unavailability>> GetAllAsync(DateTime? from, DateTime? to);
    }
}