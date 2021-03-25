using System;
using System.Collections.Generic;
using HealthyJuices.Common.Exceptions;
using HealthyJuices.Domain.Models.Abstraction;
using HealthyJuices.Domain.Models.Abstraction.DataAccess.Entities;
using HealthyJuices.Domain.Models.Users;

namespace HealthyJuices.Domain.Models.Companies
{
    public class Company : Entity, ISoftRemovableEntity, IAggregateRoot
    {
        public string Name { get; private set; }
        public string Comment { get; private set; }

        public string PostalCode { get; private set; }
        public string City { get; private set; }
        public string Street { get; private set; }

        public double Latitude { get; private set; }
        public double Longitude { get; private set; }

        public bool IsRemoved { get; private set; }

        public ICollection<User> Users { get; private set; }


        protected Company() { }

        public Company(string name, string comment, string postalCode, string city, string street, double latitude,
            double longitude)
        {
            this.Created = DateTime.UtcNow;
            this.IsRemoved = false;
            this.Update();

            this.SetName(name);
            this.Comment = comment;
            this.PostalCode = postalCode;
            this.City = city;
            this.Street = street;
            this.Latitude = latitude;
            this.Longitude = longitude;
        }

        public void Update(string name, string comment, string postalCode, string city, string street, double latitude,
            double longitude)
        {
            this.Update();

            this.SetName(name);
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
            this.LastModified = DateTime.UtcNow;
        }

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new BadRequestException($"{nameof(Company)} can not have an empty name.");

            this.Update();
            this.Name = name;
        }

        private void Update()
        {
            if (this.IsRemoved)
                throw new BadRequestException($"This {nameof(Company)} is removed");

            this.LastModified = DateTime.UtcNow;
        }
    }
}