using FluentAssertions;
using HealthyJuices.Application.Services;
using HealthyJuices.Shared.Dto.Products;
using HealthyJuices.Shared.Enums;
using System.Linq;
using System.Threading.Tasks;
using HealthyJuices.Api.Controllers;
using HealthyJuices.Application.Services.Products.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
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

            var mediator = new Mock<IMediator>();

            var controller = new ProductsController(mediator.Object);

            // act
            var result = await controller.CreateAsync(request);

            // assert

            var okResult = result as OkObjectResult;
            var actualConfiguration = okResult.Value as string;

            // Now you can compare with expected values
            actualConfiguration.Should().NotBeNullOrWhiteSpace();

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
