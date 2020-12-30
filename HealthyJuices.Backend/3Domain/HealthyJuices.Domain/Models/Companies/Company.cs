﻿using System;
using HealthyJuices.Domain.Models.Abstraction;
using HealthyJuices.Domain.Models.Abstraction.DataAccess.Entities;

namespace HealthyJuices.Domain.Models.Companies
{
    public class Company : Entity, IModifiableEntity, ISoftRemovableEntity, IAggregateRoot
    {
        public string Name { get; set; }
        public string Comment { get; set; }

        public string PostalCode { get; set; }
        public string City { get; set; }
        public string Street { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public bool IsRemoved { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }


        public Company() { }

        public Company(string name, string comment, string postalCode, string city, string street, double latitude,
            double longitude)
        {
            this.DateCreated = DateTime.UtcNow;
            this.IsRemoved = false;

            this.Name = name;
            this.Comment = comment;
            this.PostalCode = postalCode;
            this.City = city;
            this.Street = street;
            this.Latitude = latitude;
            this.Longitude = longitude;
        }

        public void Remove()
        {
            this.IsRemoved = true;
            this.DateModified = DateTime.UtcNow;
        }
    }
}