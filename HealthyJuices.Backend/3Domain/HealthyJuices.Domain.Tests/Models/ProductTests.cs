using FluentAssertions;
using HealthyJuices.Domain.Models.Products;
using HealthyJuices.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            var product = new Product("", "", ProductUnitType.Items, 0.2m, true);

            // assert
            product.IsActive.Should().BeTrue();
        }
    }
}
