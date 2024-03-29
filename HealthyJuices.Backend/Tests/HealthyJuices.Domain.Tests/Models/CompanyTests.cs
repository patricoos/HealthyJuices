﻿using FluentAssertions;
using HealthyJuices.Domain.Models.Companies;
using Xunit;

namespace HealthyJuices.Domain.Tests.Models
{
    public class CompanyTests
    {
        [Fact]
        public void Can_create_company()
        {
            //  arrange

            // act
            var company = new Company("test comp", "", "", "", "", 50, 20);

            //assert
            company.IsRemoved.Should().BeFalse();
        }
    }
}
