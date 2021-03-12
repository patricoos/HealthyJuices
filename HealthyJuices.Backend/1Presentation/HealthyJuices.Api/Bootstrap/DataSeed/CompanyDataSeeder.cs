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
                    new Company("Karolinka", "ch-karolinka.pl", "46-020", "Opole", "Wrocławska 152", 50.6816877, 17.8775668),
                    new Company("Solaris Center", "solariscenter.pl", "46-020", "Opole", "Wschodnia 25", 50.67038287903771, 17.92626781463624)
                };
                context.Companies.AddRange(companies);
            }

            context.SaveChanges();
        }
    }
}