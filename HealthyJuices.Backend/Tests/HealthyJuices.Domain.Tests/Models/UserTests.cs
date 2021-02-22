using FluentAssertions;
using HealthyJuices.Domain.Models.Companies;
using HealthyJuices.Domain.Models.Users;
using HealthyJuices.Shared.Enums;
using Xunit;

namespace HealthyJuices.Domain.Tests.Models
{
   public class UserTests
    {
        [Fact]
        public void Can_create_user()
        {
            //  arrange
            var company = new Company("test comp", "", "", "", "", 50, 20);

            // act
            var user = new User("test@test.com", "demo", "", "", company, UserRole.Customer);

            // assert
            user.IsActive.Should().BeFalse();
        }
    }
}
