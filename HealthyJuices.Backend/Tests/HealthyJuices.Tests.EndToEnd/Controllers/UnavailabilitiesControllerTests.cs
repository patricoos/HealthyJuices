using FluentAssertions;
using HealthyJuices.Application.Services;
using HealthyJuices.Shared.Dto;
using HealthyJuices.Shared.Enums;
using System;
using System.Linq;
using System.Threading.Tasks;
using HealthyJuices.Api.Controllers;
using Xunit;

namespace HealthyJuices.Tests.EndToEnd.Controllers
{
    public class UnavailabilitiesControllerTests : InMemoryDatabaseTestBase
    {
        [Fact]
        public async Task Unavailabilitie_can_be_created()
        {
            // arrange
            var request = new UnavailabilityDto()
            {
                From = DateTime.Now.AddDays(1),
                To = DateTime.Now.AddDays(2),
                Reason = UnavailabilityReason.Other,
                Comment = "test comment"
            };

            var controller = new UnavailabilitiesController(UnavailabilitiesService);

            // act
            var result = await controller.CreateAsync(request);

            // assert
            result.Should().NotBeNullOrWhiteSpace();
            var subject = AssertRepositoryContext.Unavailabilities.FirstOrDefault();

            subject.Should().NotBeNull();
            subject.From.Should().Be(request.From);
            subject.To.Should().Be(request.To);
            subject.Reason.Should().Be(request.Reason);
            subject.Comment.Should().Be(request.Comment);
        }
    }
}
