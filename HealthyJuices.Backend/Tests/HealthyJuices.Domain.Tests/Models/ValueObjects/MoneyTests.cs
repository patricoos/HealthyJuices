using FluentAssertions;
using HealthyJuices.Common.Exceptions;
using HealthyJuices.Domain.Models.Products;
using HealthyJuices.Shared.Enums;
using Xunit;

namespace HealthyJuices.Domain.Tests.Models.ValueObjects
{
    public class MoneyTests
    {
        [Fact]
        public void Can_create_money()
        {
            //  arrange
            var amount = 12M;

            // act
            var money = new Money(amount);

            //assert
            money.Amount.Should().Be(amount);
            money.Currency.Should().Be(Currency.Pln);
        }

        [Fact]
        public void Create_money_should_return_exception_on_negative_amount()
        {
            //  arrange
            var amount = -12M;

            // act
            var ex = Assert.Throws<BadRequestException>(() => new Money(amount));

            //assert
            ex.Message.Should().Be($"{nameof(Money)} {nameof(Money.Amount)} can not be less than zero.");
        }
    }
}