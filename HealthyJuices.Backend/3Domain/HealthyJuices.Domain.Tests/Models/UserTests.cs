using FluentAssertions;
using HealthyJuices.Domain.Models.Companies;
using HealthyJuices.Domain.Models.Users;
using HealthyJuices.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace HealthyJuices.Domain.Tests.Models
{
   public class UserTests
    {
        [Fact]
        public void Can_create_user()
        {
            //  arrange
            var company = new Company("", "", "", "", "", 50, 20);

            // act
            var user = new User("test@test.com", "demo", "", "", company, UserRole.Customer);

            // assert
            user.IsActive.Should().BeFalse();
        }
    }
}
