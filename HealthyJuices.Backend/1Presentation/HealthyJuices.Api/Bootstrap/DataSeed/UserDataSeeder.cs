using System.Linq;
using HealthyJuices.Common;
using HealthyJuices.Domain.Models.Users;
using HealthyJuices.Persistence.Ef;
using HealthyJuices.Shared.Enums;

namespace HealthyJuices.Api.Bootstrap.DataSeed
{
    public class UserDataSeeder : IDataSeeder
    {
        public void Seed(IDbContext context)
        {
            if (!context.Users.Any())
            {
                var businessOwner = new User(HealthyJuicesConstants.DEFAULT_USER_LOGIN, HealthyJuicesConstants.DEFAULT_USER_PASSWORD, UserRole.BusinessOwner);
                context.Users.Add(businessOwner);
            }

            context.SaveChanges();
        }
    }
}