using System;
using HealthyJuices.Domain.Models.Unavailabilities;
using HealthyJuices.Shared.Enums;
using HealthyJuices.Tests.EndToEnd.Extensions;
using HealthyJuices.Tests.EndToEnd.SeedTestData.Abstraction;

namespace HealthyJuices.Tests.EndToEnd.SeedTestData
{
    public class UnavailabilityBuilder : TestDataBuilder<Unavailability>
    {
        public static UnavailabilityBuilder Create() => new UnavailabilityBuilder();

        public UnavailabilityBuilder From(DateTime from)
        {
            base.Entity.SetProperty(p => p.From, from);
            return this;
        }

        public UnavailabilityBuilder To(DateTime to)
        {
            base.Entity.SetProperty(p => p.To, to);
            return this;
        }
        public UnavailabilityBuilder WithComment(string commnet)
        {
            base.Entity.SetProperty(p => p.Comment, commnet);
            return this;
        }

        public UnavailabilityBuilder WithReason(UnavailabilityReason reason)
        {
            base.Entity.SetProperty(p => p.Reason, reason);
            return this;
        }
    }
}