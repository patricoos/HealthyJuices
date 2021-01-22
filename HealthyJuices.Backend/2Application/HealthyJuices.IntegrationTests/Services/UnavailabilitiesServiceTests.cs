using FluentAssertions;
using HealthyJuices.Application.Services;
using HealthyJuices.Persistence.Ef.Repositories.Unavailabilities;
using HealthyJuices.Persistence.TestHelpers;
using HealthyJuices.Shared.Dto;
using HealthyJuices.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace HealthyJuices.IntegrationTests.Services
{
    public class UnavailabilitiesServiceTests : InMemoryDatabaseTestBase
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

            // act
            var service = new UnavailabilitiesService(UnavailabilityRepository);
            var result = await service.CreateAsync(request);

            // assert
            result.Should().BeGreaterThan(0);
            var subject = AssertRepositoryContext.Unavailabilities.FirstOrDefault();

            subject.Should().NotBeNull();
            subject.From.Should().Be(request.From);
            subject.To.Should().Be(request.To);
            subject.Reason.Should().Be(request.Reason);
            subject.Comment.Should().Be(request.Comment);
        }
    }
}
