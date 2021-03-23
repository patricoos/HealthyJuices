﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HealthyJuices.Common.Contracts;
using HealthyJuices.Domain.Models.Companies;
using HealthyJuices.Domain.Models.Companies.DataAccess;

namespace HealthyJuices.Persistence.Ef.Repositories.Companies
{
    public class CompanyRepository : BaseRepository<Company>, ICompanyWriteRepository
    {
        private CompanyQueryBuilder Query => new CompanyQueryBuilder(AggregateRootDbSet.AsQueryable());

        public CompanyRepository(IDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Company>> GetAllActiveAsync()
        {
            return await Query.IsNotRemoved().ToListAsync();
        }


    }
}