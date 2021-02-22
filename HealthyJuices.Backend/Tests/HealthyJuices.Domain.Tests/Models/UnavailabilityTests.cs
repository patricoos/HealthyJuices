using FluentAssertions;
using HealthyJuices.Common.Exceptions;
using HealthyJuices.Domain.Models.Unavailabilities;
using HealthyJuices.Shared.Enums;
using System;
using Xunit;

namespace HealthyJuices.Domain.Tests.Models
{
    public class UnavailabilityTests
    {
        [Fact]
        public void Can_create_unavailability()
        {
            //  arrange

            // act
            var unavailability = new Unavailability(DateTime.Now, DateTime.Now.AddDays(1), UnavailabilityReason.Sick, "");

            // assert
            unavailability.Reason.Should().Be(UnavailabilityReason.Sick);
        }

        [Fact]
        public void When_create_unavailability_with_from_date_greater_than_to_should_retunr_exception()
        {
            //  arrange
            Action act = () => new Unavailability(DateTime.Now.AddDays(1), DateTime.Now, UnavailabilityReason.Sick, "");

            // act
            BadRequestException exception = Assert.Throws<BadRequestException>(act);

            //assert
            exception.Message.Should().Be("Dates are invalid.");
        }

        [Fact]
        public void When_create_unavailability_with_from_date_equals_to_date_should_retunr_exception()
        {
            //  arrange
            var time = DateTime.Now;
            Action act = () => new Unavailability(time, time, UnavailabilityReason.Sick, "");

            // act
            BadRequestException exception = Assert.Throws<BadRequestException>(act);

            //assert
            exception.Message.Should().Be("Dates are invalid.");
        }
    }
}
