using System;
using HealthyJuices.Domain.Models.Abstraction;
using HealthyJuices.Domain.Models.Abstraction.DataAccess.Entities;
using HealthyJuices.Shared.Enums;

namespace HealthyJuices.Domain.Models.Unavailabilities
{
    public class Unavailability : Entity, IAggregateRoot
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }

        public UnavailabilityReason Reason { get; set; }

        public string Comment { get; set; }


        public Unavailability() { }

        public Unavailability(DateTime from, DateTime to, UnavailabilityReason reason, string comment)
        {
            this.From = from;
            this.To = to;
            this.Reason = reason;
            this.Comment = comment;
        }

        public void Update(DateTime from, DateTime to, UnavailabilityReason reason, string comment)
        {
            this.From = from;
            this.To = to;
            this.Reason = reason;
            this.Comment = comment;
        }
    }
}