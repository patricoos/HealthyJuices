using System;
using System.Collections.Generic;
using System.Linq;
using HealthyJuices.Common.Exceptions;
using HealthyJuices.Domain.Models.Abstraction;
using HealthyJuices.Domain.Models.Abstraction.DataAccess.Entities;
using HealthyJuices.Domain.Models.Companies;
using HealthyJuices.Domain.Models.Products;
using HealthyJuices.Domain.Models.Users;
using HealthyJuices.Shared.Enums;

namespace HealthyJuices.Domain.Models.Orders
{
    public class Order : Entity, IModifiableEntity, ISoftRemovableEntity, IAggregateRoot
    {
        public DateTime DateCreated { get; init; }
        public DateTime DateModified { get; set; }
        public bool IsRemoved { get; set; }

        public DateTime DeliveryDate { get; set; }

        //  public OrderStatus Status { get; set; }


        public long UserId { get; set; }
        public User User { get; set; }

        public long DestinationCompanyId { get; set; }
        public Company DestinationCompany { get; set; }

        public ICollection<OrderProduct> OrderProducts { get; set; }

        public Order()
        {
        }

        public Order(User user, DateTime deliveryDate, OrderProduct[] products)
        {
            this.OrderProducts = new List<OrderProduct>();
            this.DateCreated = DateTime.UtcNow;
            this.Modifie();
            this.SetDeliveryDate(deliveryDate);
            this.AddProduct(products);
            this.SetUser(user);
            this.SetCompany(user.Company);
        }

        public void AddProduct(params OrderProduct[] products)
        {
            foreach (var item in products)
                this.AddProduct(item.Product, item.Amount);
        }

        public void AddProduct(Product product, decimal amount)
        {
            this.Modifie();
            this.OrderProducts.Add(new OrderProduct(this, product, amount));
        }

        public void Remove()
        {
            this.Modifie();
            this.IsRemoved = true;
        }

        public void Update(DateTime deliveryDate)
        {
            this.Modifie();
            this.SetDeliveryDate(deliveryDate);
        }


        private void Modifie()
        {
            if (DeliveryDate <= DateTime.Now)
                throw new BadRequestException("This order can not be modified");

            this.DateModified = DateTime.UtcNow;
        }

        private void SetDeliveryDate(DateTime date)
        {
            if (date <= DateTime.Now)
                throw new BadRequestException("Delivery Date must be in feature");

            this.Modifie();
            this.DeliveryDate = date;
        }

        private void SetCompany(Company company)
        {
            if (company == null)
                throw new BadRequestException("Company not exists");

            if (company.IsRemoved)
                throw new BadRequestException($"Company '{company.Name}' is removed");

            this.Modifie();
            this.DestinationCompany = company;
        }

        private void SetUser(User user)
        {
            if (user == null)
                throw new BadRequestException("User not exists");

            if (!user.IsActive || user.IsRemoved)
                throw new BadRequestException($"User '{user.Email}' is not active");

            if (user.Roles.HasFlag(UserRole.Customer))
                throw new BadRequestException($"User '{user.Email}' has no permision to create order");

            this.Modifie();
            this.User = user;
        }
    }
}