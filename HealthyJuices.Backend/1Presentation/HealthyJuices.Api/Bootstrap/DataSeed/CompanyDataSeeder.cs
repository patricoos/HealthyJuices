using System.Collections.Generic;
using System.Linq;
using HealthyJuices.Domain.Models.Companies;
using HealthyJuices.Domain.Models.Products;
using HealthyJuices.Persistence.Ef;
using HealthyJuices.Shared.Enums;

namespace HealthyJuices.Api.Bootstrap.DataSeed
{
    public class CompanyDataSeeder : IDataSeeder
    {
        public void Seed(IDbContext context)
        {
            if (!context.Companies.Any())
            {
                var companies = new List<Company>()
                {
                    new Company("21infinity", "21infinity.com", "46-020", "Opole", "Wschodnia 25", 50.6617009, 17.9801195),
                    new Company("Solaris Center", "solariscenter.pl", "46-020", "Opole", "Wschodnia 25", 50.67038287903771, 17.92626781463624)
                };
            context.Companies.AddRange(companies);
            }

            context.SaveChanges();
        }
    }
}