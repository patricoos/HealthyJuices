using HealthyJuices.Persistence.Ef;

namespace HealthyJuices.Api.Bootstrap.DataSeed
{
    public interface IDataSeeder
    {
        void Seed(IDbContext context);
    }
}