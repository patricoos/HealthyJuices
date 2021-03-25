using System.Linq;
using HealthyJuices.Domain.Models.Users;
using HealthyJuices.Persistence.Ef;
using HealthyJuices.Shared.Enums;

namespace HealthyJuices.Api.Bootstrap.DataSeed
{
    public class UserDataSeeder : IDataSeeder
    {
        public void Seed(IApplicationDbContext context)
        {
            if (!context.Users.Any())
            {
                var businessOwner = new User("bo@demo.com", "demo", "bo name", "bo surname", UserRole.BusinessOwner);
                businessOwner.Activate();
                context.Users.Add(businessOwner);

                var customer = new User("customer@demo.com", "demo", "customer name", "customer surname", context.Companies.FirstOrDefault(), UserRole.Customer);
                customer.Activate();
                context.Users.Add(customer);
            }

            context.SaveChanges();
        }
    }
}