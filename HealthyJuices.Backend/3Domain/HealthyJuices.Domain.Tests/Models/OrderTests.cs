using FluentAssertions;
using HealthyJuices.Common.Exceptions;
using HealthyJuices.Domain.Models.Companies;
using HealthyJuices.Domain.Models.Orders;
using HealthyJuices.Domain.Models.Products;
using HealthyJuices.Domain.Models.Users;
using HealthyJuices.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace HealthyJuices.Domain.Tests.Models
{
    public class OrderTests
    {
        [Fact]
        public void Can_create_order_for_tomorrow_and_add_product()
        {
            //  arrange
            var company = new Company("", "", "", "", "", 50, 20);
            var user = new User("", "", "", "", company, UserRole.Customer);
            user.Activate();
            var product = new Product("", "", ProductUnitType.Items, 0.2m, true) { Id = 1 };

            // act
            var order = new Order(user, DateTime.Now.AddDays(1), new Dictionary<Product, decimal> { { product, 1 } }.ToArray());
            order.AddProduct(product, 1);

            //assert
            order.OrderProducts.Count.Should().Be(1);
            order.OrderProducts.First().Amount.Should().Be(2);
        }

        [Fact]
        public void Can_not_create_order_for_today()
        {
            var company = new Company("", "", "", "", "", 50, 20);
            var user = new User("", "", "", "", company, UserRole.Customer);
            user.Activate();
            var product = new Product("", "", ProductUnitType.Items, 0.2m, true) { Id = 1 };

            // act
            Action act = () => new Order(user, DateTime.Now);
            BadRequestException exception = Assert.Throws<BadRequestException>(act);

            //assert
            exception.Message.Should().Be("Delivery Date must be in feature");
        }

        [Fact]
        public void Can_not_create_order_with_not_active_user()
        {
            var company = new Company("", "", "", "", "", 50, 20);
            var user = new User("", "", "", "", company, UserRole.Customer);
            var product = new Product("", "", ProductUnitType.Items, 0.2m, true) { Id = 1 };

            // act
            Action act = () => new Order(user, DateTime.Now.AddDays(1));
            BadRequestException exception = Assert.Throws<BadRequestException>(act);

            //assert
            exception.Message.Should().Be($"User '{user.Email}' is not active");
        }

        [Fact]
        public void Can_not_add_product_when_order_is_removed()
        {
            var company = new Company("", "", "", "", "", 50, 20);
            var user = new User("", "", "", "", company, UserRole.Customer);
            user.Activate();
            var product = new Product("", "", ProductUnitType.Items, 0.2m, true) { Id = 1 };
            var order = new Order(user, DateTime.Now.AddDays(1), new Dictionary<Product, decimal> { { product, 1 } }.ToArray());
            order.Remove();

            // act
            Action act = () => order.AddProduct(product, 1);
            BadRequestException exception = Assert.Throws<BadRequestException>(act);

            //assert
            exception.Message.Should().Be("This order is removed");
        }

        [Fact]
        public void Can_not_update_removed_order()
        {
            var company = new Company("", "", "", "", "", 50, 20);
            var user = new User("", "", "", "", company, UserRole.Customer);
            user.Activate();
            var product = new Product("", "", ProductUnitType.Items, 0.2m, true) { Id = 1 };
            var order = new Order(user, DateTime.Now.AddDays(1), new Dictionary<Product, decimal> { { product, 1 } }.ToArray());
            order.Remove();

            // act
            Action act = () => order.Update(DateTime.Now.AddDays(2));
            BadRequestException exception = Assert.Throws<BadRequestException>(act);

            //assert
            exception.Message.Should().Be("This order is removed");
        }
    }
}
