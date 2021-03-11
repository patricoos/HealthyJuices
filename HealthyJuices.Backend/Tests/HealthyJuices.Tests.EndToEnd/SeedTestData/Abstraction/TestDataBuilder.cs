using System;
using System.Linq;
using HealthyJuices.Domain.Models.Abstraction.DataAccess.Entities;
using HealthyJuices.Persistence.Ef;

namespace HealthyJuices.Tests.EndToEnd.SeedTestData.Abstraction
{
    public abstract class TestDataBuilder<T>
        where T : Entity
    {
        protected readonly T Entity;
        protected readonly Random _random;


        protected TestDataBuilder()
        {
            Entity = (T)Activator.CreateInstance(typeof(T), true);
            _random = new Random();
        }

        public virtual T Build(IDbContext context)
        {
            context.Set<T>().Add(Entity);
            context.SaveChanges();

            return Entity;
        }

        public virtual T Build()
        {
            return Entity;
        }

        protected int GeneratePositiveRandomNumber(int minValue = 1, int maxValue = int.MaxValue) => _random.Next(minValue, maxValue);
        protected string RandomId => Guid.NewGuid().ToString();

        protected string GenerateRandomString(int length = 10)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[_random.Next(s.Length)]).ToArray());
        }
    }
}