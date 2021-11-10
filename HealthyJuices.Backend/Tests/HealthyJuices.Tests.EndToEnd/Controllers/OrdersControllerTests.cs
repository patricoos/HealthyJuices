using FluentAssertions;
using HealthyJuices.Shared.Dto.Products;
using HealthyJuices.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HealthyJuices.Api.Controllers;
using HealthyJuices.Application.Features.Orders.Commands;
using HealthyJuices.Common.Exceptions;
using HealthyJuices.Tests.EndToEnd.Extensions;
using HealthyJuices.Tests.EndToEnd.SeedTestData;
using Xunit;

namespace HealthyJuices.Tests.EndToEnd.Controllers
{
    public class OrdersControllerTests : InMemoryDatabaseTestBase
    {
        [Fact]
        public async Task Orders_can_be_created()
        {
            // arrange
            var user = UserBuilder.Create().WithRole(UserRole.Customer).WithEmail("test@test.com").WithRole(UserRole.Customer).Active().Build(ArrangeRepositoryContext);
            var company = CompanyBuilder.Create().WithName("test company").WithUsers(user).Build(ArrangeRepositoryContext);
            var product = ProductBuilder.Create().WithName("test Prod").Active().Build(ArrangeRepositoryContext);

            var amount = 2;

            var request = new CreateOrder.Command
            {
                DeliveryDate = DateTime.Now.AddDays(1),
                UserId = user.Id,
                OrderProducts = new List<OrderItemDto> {
                    new OrderItemDto()
                    {
                        ProductId = product.Id,
                        Amount = amount
                    }
                }
            };
            var controller = new OrdersController(Mediator);
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
            var user = UserBuilder.Create().WithRole(UserRole.Customer).WithEmail("test@test.com").WithRole(UserRole.Customer).Active().Build(ArrangeRepositoryContext);
            var company = CompanyBuilder.Create().WithName("test company").WithUsers(user).Removed().Build(ArrangeRepositoryContext);

            var product = ProductBuilder.Create().WithName("test Prod").Active().Build(ArrangeRepositoryContext);

            var unavailability = UnavailabilityBuilder.Create().From(DateTime.Now).To(DateTime.Now.AddDays(3)).WithReason(UnavailabilityReason.Sick).Build(ArrangeRepositoryContext);

            var request = new CreateOrder.Command
            {
                DeliveryDate = DateTime.Now.AddDays(1),
                UserId = user.Id,
                OrderProducts = new List<OrderItemDto> {
                    new OrderItemDto()
                    {
                        ProductId = product.Id,
                        Amount = 1
                    }
                }
            };

            var controller = new OrdersController(Mediator);
            controller.SetUserId(user.Id);

            // act
            Func<Task> act = () => controller.CreateAsync(request);
            var exception = await Assert.ThrowsAsync<BadRequestException>(act);

            //assert
            exception.Message.Should().Be("Can not create order in unavailability duration");

            var subject = AssertRepositoryContext.Orders.ToList();
            subject.Count.Should().Be(0);
        }
    }
}
