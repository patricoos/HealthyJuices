using FluentAssertions;
using HealthyJuices.Domain.Models.Unavailabilities;
using HealthyJuices.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
