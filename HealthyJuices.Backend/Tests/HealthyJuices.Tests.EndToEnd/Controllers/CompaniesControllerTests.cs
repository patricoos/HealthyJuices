using FluentAssertions;
using HealthyJuices.Shared.Dto;
using System.Linq;
using System.Threading.Tasks;
using HealthyJuices.Api.Controllers;
using HealthyJuices.Application.Functions.Companies.Commands;
using MediatR;
using Xunit;

namespace HealthyJuices.Tests.EndToEnd.Controllers
{
    public class CompaniesControllerTests : InMemoryDatabaseTestBase
    {
        [Fact]
        public async Task Company_can_be_created()
        {
            // arrange
            var request = new CreateCompany.Command()
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
            var controller = new CompaniesController(Mediator);
            var result = await controller.CreateAsync(request);

            // assert
            result.Should().NotBeNullOrWhiteSpace();
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
