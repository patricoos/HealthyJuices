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
        public void Can_create_order_for_tomorrow()
        {
            //  arrange
            var company = new Company("", "", "", "", "", 50, 20);
            var user = new User("test@test.com", "demo", "", "", company, UserRole.Customer);
            user.Activate();
            var product = new Product("test Prod", "", ProductUnitType.Items, 0.2m, true) { Id = 1 };

            // act
            var order = new Order(user, DateTime.Now.AddDays(1), new List<KeyValuePair<Product, decimal>> { new KeyValuePair<Product, decimal>(product, 1) });

            //assert
            order.OrderProducts.Count.Should().Be(1);
            order.OrderProducts.First().Amount.Should().Be(1);
        }

        [Fact]
        public void Can_create_order_for_tomorrow_and_add_products()
        {
            //  arrange
            var company = new Company("", "", "", "", "", 50, 20);
            var user = new User("test@test.com", "demo", "", "", company, UserRole.Customer);
            user.Activate();
            var product = new Product("test Prod", "", ProductUnitType.Items, 0.2m, true) { Id = 1 };

            // act
            var order = new Order(user, DateTime.Now.AddDays(1), new List<KeyValuePair<Product, decimal>> {
                new KeyValuePair<Product, decimal>(product, 1) ,
                new KeyValuePair<Product, decimal>(product, 1) ,
            });
            order.AddProduct(product, 1);

            //assert
            order.OrderProducts.Count.Should().Be(1);
            order.OrderProducts.First().Amount.Should().Be(3);
        }

        [Fact]
        public void When_create_order_for_today_returns_exception()
        {
            var company = new Company("", "", "", "", "", 50, 20);
            var user = new User("test@test.com", "demo", "", "", company, UserRole.Customer);
            user.Activate();
            var product = new Product("test Prod", "", ProductUnitType.Items, 0.2m, true) { Id = 1 };

            // act
            Action act = () => new Order(user, DateTime.Now);
            BadRequestException exception = Assert.Throws<BadRequestException>(act);

            //assert
            exception.Message.Should().Be("Delivery Date must be in feature");
        }

        [Fact]
        public void When_create_order_with_not_active_user_returns_exception()
        {
            var company = new Company("", "", "", "", "", 50, 20);
            var user = new User("test@test.com", "demo", "", "", company, UserRole.Customer);
            var product = new Product("test Prod", "", ProductUnitType.Items, 0.2m, true) { Id = 1 };

            // act
            Action act = () => new Order(user, DateTime.Now.AddDays(1));
            BadRequestException exception = Assert.Throws<BadRequestException>(act);

            //assert
            exception.Message.Should().Be($"User '{user.Email}' is not active");
        }

        [Fact]
        public void When_add_product_when_order_is_removed_returns_exception()
        {
            var company = new Company("", "", "", "", "", 50, 20);
            var user = new User("test@test.com", "demo", "", "", company, UserRole.Customer);
            user.Activate();
            var product = new Product("test Prod", "", ProductUnitType.Items, 0.2m, true) { Id = 1 };
            var order = new Order(user, DateTime.Now.AddDays(1), new Dictionary<Product, decimal> { { product, 1 } }.ToArray());
            order.Remove();

            // act
            Action act = () => order.AddProduct(product, 1);
            BadRequestException exception = Assert.Throws<BadRequestException>(act);

            //assert
            exception.Message.Should().Be("This order is removed");
        }

        [Fact]
        public void When_update_removed_order_returns_exception()
        {
            var company = new Company("", "", "", "", "", 50, 20);
            var user = new User("test@test.com", "demo", "", "", company, UserRole.Customer);
            user.Activate();
            var product = new Product("test Prod", "", ProductUnitType.Items, 0.2m, true) { Id = 1 };
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
