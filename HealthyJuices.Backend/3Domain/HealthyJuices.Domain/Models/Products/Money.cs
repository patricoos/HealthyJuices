using System;
using HealthyJuices.Common.Exceptions;
using HealthyJuices.Shared.Enums;

namespace HealthyJuices.Domain.Models.Products
{
    public class Money : IEquatable<Money>, IComparable, IComparable<Money>
    {
        public decimal Amount { get; private init; }
        public Currency Currency { get; private init; }

        protected Money() { }

        public Money(decimal amount)
        {
            Amount = amount >= 0 ? amount : throw new BadRequestException($"{nameof(Money)} {nameof(Amount)} can not be less than zero.");
            Currency = Currency.Pln;
        }

        public bool Equals(Money other)
        {
            throw new NotImplementedException();
        }

        public int CompareTo(object obj)
        {
            throw new NotImplementedException();
        }

        public int CompareTo(Money other)
        {
            throw new NotImplementedException();
        }
    }
}