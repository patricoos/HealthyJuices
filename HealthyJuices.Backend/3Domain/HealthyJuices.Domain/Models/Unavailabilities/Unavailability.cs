using System;
using HealthyJuices.Domain.Models.Abstraction.DataAccess.Entities;
using HealthyJuices.Shared.Enums;

namespace HealthyJuices.Domain.Models.Unavailabilities
{
    public class Unavailability : Entity
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }

        public UnavailabilityReason Reason { get; set; }

        public string Comment { get; set; }

   //     public long DriverId { get; set; }
    //    public Driver Driver { get; set; }
    }
}