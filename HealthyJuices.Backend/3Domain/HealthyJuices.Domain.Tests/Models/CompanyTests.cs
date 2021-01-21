using FluentAssertions;
using HealthyJuices.Domain.Models.Companies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            var company = new Company("", "", "", "", "", 50, 20);

            //assert
            company.IsRemoved.Should().BeFalse();
        }
    }
}
