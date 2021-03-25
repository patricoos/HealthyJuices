using FluentAssertions;
using HealthyJuices.Shared.Enums;
using System.Linq;
using System.Threading.Tasks;
using HealthyJuices.Api.Controllers;
using HealthyJuices.Application.Features.Products.Commands;
using Xunit;

namespace HealthyJuices.Tests.EndToEnd.Controllers
{
    public class ProductsControllerTests : InMemoryDatabaseTestBase
    {
        [Fact]
        public async Task Product_can_be_created()
        {
            // arrange
            var request = new CreateProduct.Command()
            {
                Name = "Sample product",
                Description = "test desc",
                Unit = ProductUnitType.Items,
                QuantityPerUnit = 1.5m,
                IsActive = true
            };

            var controller = new ProductsController(Mediator);

            // act
            var result = await controller.CreateAsync(request);

            // assert
            result.Should().NotBeNullOrWhiteSpace();

            var subject = AssertRepositoryContext.Products.FirstOrDefault();

            subject.Should().NotBeNull();
            subject.Name.Should().Be(request.Name);
            subject.Unit.Should().Be(request.Unit);
            subject.IsActive.Should().Be(request.IsActive);
            subject.QuantityPerUnit.Should().Be(request.QuantityPerUnit);
            subject.Description.Should().Be(request.Description);
            subject.IsRemoved.Should().BeFalse();
        }
    }
}
