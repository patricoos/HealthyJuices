using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HealthyJuices.Domain.Models.Unavailabilities;
using HealthyJuices.Domain.Models.Unavailabilities.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace HealthyJuices.Persistence.Ef.Repositories.Unavailabilities
{
    public class UnavailabilityRepository : BaseRepository<Unavailability>, IUnavailabilityRepository
    {
        public UnavailabilityRepository(IDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Unavailability>> GetAllAsync(DateTime? @from, DateTime? to)
        {
            var query = Query;
            if (@from.HasValue)
                query.AfterDateTime(from.Value);

            if (to.HasValue)
                query.BeforeDateTime(to.Value);

            return await query.ToListAsync();
        }
    }
}