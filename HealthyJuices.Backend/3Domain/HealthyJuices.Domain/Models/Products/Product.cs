﻿using System;
using HealthyJuices.Common.Exceptions;
using HealthyJuices.Domain.Models.Abstraction;
using HealthyJuices.Domain.Models.Abstraction.DataAccess.Entities;
using HealthyJuices.Shared.Enums;

namespace HealthyJuices.Domain.Models.Products
{
    public class Product : Entity, ISoftRemovableEntity, IAggregateRoot
    {
        public string Name { get; private set; }
        public string Description { get; private set; }

        public ProductUnitType Unit { get; private set; }
        public decimal QuantityPerUnit { get; private set; }
        public Money DefaultPricePerUnit { get; private set; }

        public bool IsRemoved { get; private set; }
        public bool IsActive { get; private set; }


        protected Product() { }

        public Product(string name, string description, ProductUnitType unit, decimal quantityPerUnit, bool isActive, decimal? defaultPrice = null)
        {
            this.Created = DateTime.UtcNow;
            this.IsRemoved = false;
            this.Update();

            this.SetName(name);
            this.Description = description;
            this.Unit = unit;
            this.QuantityPerUnit = quantityPerUnit < 0 ? throw new BadRequestException($"Quantity per unit can not be less than zero.") : quantityPerUnit;
            this.DefaultPricePerUnit = defaultPrice.HasValue ? new Money(defaultPrice.Value) : null;
            this.IsActive = isActive;
        }

        public void Remove()
        {
            this.Update();
            this.IsRemoved = true;
        }

        public void Update(string name, string description, ProductUnitType unit, decimal quantityPerUnit, bool isActive, decimal? defaultPrice = null)
        {
            this.Update();

            this.SetName(name);
            this.Description = description;
            this.Unit = unit;
            this.QuantityPerUnit = quantityPerUnit < 0 ? throw new BadRequestException($"Quantity per unit can not be less than zero.") : quantityPerUnit;
            this.DefaultPricePerUnit = defaultPrice.HasValue ? new Money(defaultPrice.Value) : null;
            this.IsActive = isActive;
        }

        private void Update()
        {
            if (this.IsRemoved)
                throw new BadRequestException($"This {nameof(Product)} is removed");

            this.LastModified = DateTime.UtcNow;
        }

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new BadRequestException($"{nameof(Product)} can not have an empty name.");

            this.Name = name;
            this.Update();
        }
    }
}