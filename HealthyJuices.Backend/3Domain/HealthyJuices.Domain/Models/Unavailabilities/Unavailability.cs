using System;
using HealthyJuices.Common.Exceptions;
using HealthyJuices.Domain.Models.Abstraction;
using HealthyJuices.Domain.Models.Abstraction.DataAccess.Entities;
using HealthyJuices.Shared.Enums;

namespace HealthyJuices.Domain.Models.Unavailabilities
{
    public class Unavailability : Entity, IAggregateRoot
    {
        public DateTime From { get; private set; }
        public DateTime To { get; private set; }

        public UnavailabilityReason Reason { get; private set; }

        public string Comment { get; private set; }


        public Unavailability() { }

        public Unavailability(DateTime from, DateTime to, UnavailabilityReason reason, string comment)
        {
            this.SetDuration(from, to);
            this.Reason = reason;
            this.Comment = comment;
        }

        public void Update(DateTime from, DateTime to, UnavailabilityReason reason, string comment)
        {
            this.SetDuration(from, to);
            this.Reason = reason;
            this.Comment = comment;
        }

        public void SetDuration(DateTime from, DateTime to)
        {
            if(from >= to)
                throw new BadRequestException($"Dates are invalid.");

            this.From = from;
            this.To = to;
        }
    }
}