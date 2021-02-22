using FluentAssertions;
using HealthyJuices.Application.Services;
using HealthyJuices.Common.Exceptions;
using HealthyJuices.Domain.Models.Companies;
using HealthyJuices.Domain.Models.Products;
using HealthyJuices.Domain.Models.Unavailabilities;
using HealthyJuices.Domain.Models.Users;
using HealthyJuices.Shared.Dto.Orders;
using HealthyJuices.Shared.Dto.Products;
using HealthyJuices.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HealthyJuices.Api.Controllers;
using HealthyJuices.Tests.EndToEnd.Extensions;
using Xunit;

namespace HealthyJuices.Tests.EndToEnd.Controllers
{
    public class OrdersControllerTests : InMemoryDatabaseTestBase
    {
        [Fact]
        public async Task Orders_can_be_created()
        {
            // arrange
            var company = new Company("test comp", "", "", "", "", 50, 20);
            ArrangeRepositoryContext.Companies.Add(company);

            var user = new User("test@test.com", "demo", "", "", company, UserRole.Customer);
            user.Activate();
            ArrangeRepositoryContext.Users.Add(user);

            var product = new Product("test Prod", "", ProductUnitType.Items, 0.2m, true);
            ArrangeRepositoryContext.Products.Add(product);

            ArrangeRepositoryContext.SaveChanges();

            var amount = 2;

            var request = new OrderDto()
            {
                DeliveryDate = DateTime.Now.AddDays(1),
                UserId = user.Id,
                OrderProducts = new List<OrderProductDto> {
                    new OrderProductDto()
                    {
                        ProductId = product.Id,
                        Amount = amount
                    }
                }
            };
            var controller = new OrdersController(OrdersService);
            controller.SetUserId(user.Id);

            // act
            var result = await controller.CreateAsync(request);

            // assert
            result.Should().NotBeNullOrWhiteSpace();
            var subject = AssertRepositoryContext.Orders
                .Include(x => x.OrderProducts)
                .ThenInclude(x => x.Product)
                .FirstOrDefault();

            subject.Should().NotBeNull();
            subject.DeliveryDate.Should().Be(request.DeliveryDate);
            subject.UserId.Should().Be(request.UserId);
            subject.DestinationCompanyId.Should().Be(company.Id);

            subject.OrderProducts.Count.Should().Be(1);
            subject.OrderProducts.First().ProductId.Should().Be(product.Id);
            subject.OrderProducts.First().Amount.Should().Be(amount);
        }

        [Fact]
        public async Task Can_not_create_order_in_unavailability_duration()
        {
            // arrange
            var company = new Company("test comp", "", "", "", "", 50, 20);
            ArrangeRepositoryContext.Companies.Add(company);

            var user = new User("test@test.com", "demo", "", "", company, UserRole.Customer);
            user.Activate();
            ArrangeRepositoryContext.Users.Add(user);

            var product = new Product("test Prod", "", ProductUnitType.Items, 0.2m, true);
            ArrangeRepositoryContext.Products.Add(product);

            var unavailability = new Unavailability(DateTime.Now, DateTime.Now.AddDays(3), UnavailabilityReason.Sick, "test coment");
            ArrangeRepositoryContext.Unavailabilities.Add(unavailability);

            ArrangeRepositoryContext.SaveChanges();

            var request = new OrderDto()
            {
                DeliveryDate = DateTime.Now.AddDays(1),
                UserId = user.Id,
                OrderProducts = new List<OrderProductDto> {
                    new OrderProductDto()
                    {
                        ProductId = product.Id,
                        Amount = 1
                    }
                }
            };

            var controller = new OrdersController(OrdersService);
            controller.SetUserId(user.Id);

            // act
            Func<Task> act = () => controller.CreateAsync(request);
            BadRequestException exception = await Assert.ThrowsAsync<BadRequestException>(act);

            //assert
            exception.Message.Should().Be("Can not create order in unavailability duration");

            // assert
            var subject = AssertRepositoryContext.Orders.ToList();

            subject.Count.Should().Be(0);
        }
    }
}
