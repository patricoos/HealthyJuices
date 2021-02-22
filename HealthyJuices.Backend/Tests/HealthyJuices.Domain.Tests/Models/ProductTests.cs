using FluentAssertions;
using HealthyJuices.Domain.Models.Products;
using HealthyJuices.Shared.Enums;
using Xunit;

namespace HealthyJuices.Domain.Tests.Models
{
    public class ProductTests
    {
        [Fact]
        public void Can_create_product()
        {
            //  arrange

            // act
            var product = new Product("test Prod", "", ProductUnitType.Items, 0.2m, true);

            // assert
            product.IsActive.Should().BeTrue();
        }
    }
}
