using FluentAssertions;
using HealthyJuices.Application.Services;
using HealthyJuices.Persistence.TestHelpers;
using HealthyJuices.Shared.Dto.Products;
using HealthyJuices.Shared.Enums;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace HealthyJuices.IntegrationTests.Services
{
    public class ProductsServiceTests : InMemoryDatabaseTestBase
    {
        [Fact]
        public async Task Product_can_be_created()
        {
            // arrange
            var request = new ProductDto()
            {
                Name = "Sample product",
                Description = "test desc",
                Unit = ProductUnitType.Items,
                QuantityPerUnit = 1.5m,
                IsActive = true
            };

            // act
            var service = new ProductsService(ProductRepository);
            var result = await service.CreateAsync(request);

            // assert
            result.Should().BeGreaterThan(0);
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
