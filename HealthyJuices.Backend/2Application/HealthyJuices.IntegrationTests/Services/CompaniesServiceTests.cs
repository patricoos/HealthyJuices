using FluentAssertions;
using HealthyJuices.Application.Services;
using HealthyJuices.Persistence.TestHelpers;
using HealthyJuices.Shared.Dto;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace HealthyJuices.IntegrationTests.Services
{
    public class CompaniesServiceTests : InMemoryDatabaseTestBase
    {
        [Fact]
        public async Task Company_can_be_created()
        {
            // arrange
            var request = new CompanyDto()
            {
                Name = "Sample product",
                Comment = "test comment",
                PostalCode = "Postal Code",
                City = "City test",
                Street = "test Street",
                Latitude = 12,
                Longitude = 25
            };

            // act
            var service = new CompaniesService(CompanyRepository);
            var result = await service.CreateAsync(request);

            // assert
            result.Should().BeGreaterThan(0);
            var subject = AssertRepositoryContext.Companies.FirstOrDefault();

            subject.Should().NotBeNull();
            subject.Name.Should().Be(request.Name);
            subject.Comment.Should().Be(request.Comment);
            subject.PostalCode.Should().Be(request.PostalCode);
            subject.City.Should().Be(request.City);
            subject.Street.Should().Be(request.Street);
            subject.Latitude.Should().Be(request.Latitude);
            subject.Longitude.Should().Be(request.Longitude);
        }
    }
}
