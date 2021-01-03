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
                var company = new Company("Test company", "test coment", "123-123", "test city", "test street", 52.22, 21.01);
                context.Companies.Add(company);
            }

            context.SaveChanges();
        }
    }
}