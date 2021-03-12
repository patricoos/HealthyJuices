using System;
using FluentAssertions;
using HealthyJuices.Common.Exceptions;
using HealthyJuices.Domain.Models.Companies;
using HealthyJuices.Domain.Models.Users;
using Xunit;

namespace HealthyJuices.Domain.Tests.Models.ValueObjects
{
    public class PasswordTests
    {
        [Fact]
        public void Can_create_password()
        {
            //  arrange
            var passText = "123";

            // act
            var password = new Password(passText);

            //assert
            password.Text.Should().NotBeNullOrWhiteSpace();
            password.Salt.Should().NotBeNullOrWhiteSpace();

            password.CheckValidity(passText).Should().BeTrue();
        }

        [Fact]
        public void Create_password_should_return_exception_on_whitespace_input()
        {
            //  arrange
            var passText = String.Empty;

            // act
            var ex = Assert.Throws<BadRequestException>(() => new Password(passText));

            //assert
            ex.Message.Should().Be("Password is incorrect.");
        }

        [Fact]
        public void Create_password_should_return_exception_on_to_short_input()
        {
            //  arrange
            var passText = "12";

            // act
            var ex = Assert.Throws<BadRequestException>(() => new Password(passText));

            //assert
            ex.Message.Should().Be("Password is incorrect.");
        }
    }
}