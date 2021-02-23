using HealthyJuices.Domain.Models.Companies;
using HealthyJuices.Domain.Models.Users;
using HealthyJuices.Tests.EndToEnd.Extensions;
using HealthyJuices.Tests.EndToEnd.SeedTestData.Abstraction;

namespace HealthyJuices.Tests.EndToEnd.SeedTestData
{
    public class CompanyBuilder : TestDataBuilder<Company>
    {
        public static CompanyBuilder Create() => new CompanyBuilder();

        public CompanyBuilder WithName(string name)
        {
            base.Entity.SetProperty(p => p.Name, name);
            return this;
        }

        public CompanyBuilder WithComment(string comment)
        {
            base.Entity.SetProperty(p => p.Comment, comment);
            return this;
        }

        public CompanyBuilder WithPostalCode(string postalCode)
        {
            base.Entity.SetProperty(p => p.PostalCode, postalCode);
            return this;
        }

        public CompanyBuilder WithCity(string city)
        {
            base.Entity.SetProperty(p => p.City, city);
            return this;
        }

        public CompanyBuilder WithStreet(string street)
        {
            base.Entity.SetProperty(p => p.Street, street);
            return this;
        }

        public CompanyBuilder WithLatitude(double latitude)
        {
            base.Entity.SetProperty(p => p.Latitude, latitude);
            return this;
        }

        public CompanyBuilder WithLongitude(double longitude)
        {
            base.Entity.SetProperty(p => p.Longitude, longitude);
            return this;
        }

        public CompanyBuilder WithUsers(params User[] users)
        {
            base.Entity.SetProperty(p => p.Users, users);
            return this;
        }

        public CompanyBuilder Removed()
        {
            base.Entity.SetProperty(p => p.IsRemoved, true);
            return this;
        }
    }
}