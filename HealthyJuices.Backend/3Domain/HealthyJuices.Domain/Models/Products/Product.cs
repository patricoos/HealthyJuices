using System;
using HealthyJuices.Common.Exceptions;
using HealthyJuices.Domain.Models.Abstraction;
using HealthyJuices.Domain.Models.Abstraction.DataAccess.Entities;
using HealthyJuices.Shared.Enums;

namespace HealthyJuices.Domain.Models.Products
{
    public class Product : Entity, IModifiableEntity, ISoftRemovableEntity, IAggregateRoot
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public ProductUnitType Unit { get; set; }
        public decimal QuantityPerUnit { get; set; }
        public decimal? DefaultPricePerUnit { get; set; }

        public DateTime DateCreated { get; init; }
        public DateTime DateModified { get; set; }
        public bool IsRemoved { get; set; }
        public bool IsActive { get; set; }


        public Product() { }

        public Product(string name, string description, ProductUnitType unit, decimal quantityPerUnit, bool isActive, decimal? defaultPrice = null)
        {
            this.DateCreated = DateTime.UtcNow;
            this.DateModified = DateTime.UtcNow;

            this.Name = name;
            this.Description = description;
            this.Unit = unit;
            this.QuantityPerUnit = quantityPerUnit;
            this.DefaultPricePerUnit = defaultPrice;
            this.IsActive = isActive;
        }

        public void Remove()
        {
            this.IsRemoved = true;
            this.DateModified = DateTime.UtcNow;
        }

        public void Update(string name, string description, ProductUnitType unit, decimal quantityPerUnit, bool isActive, decimal? defaultPrice = null)
        {
            this.DateModified = DateTime.UtcNow;

            this.Name = name;
            this.Description = description;
            this.Unit = unit;
            this.QuantityPerUnit = quantityPerUnit;
            this.DefaultPricePerUnit = defaultPrice;
            this.IsActive = isActive;
        }
    }
}