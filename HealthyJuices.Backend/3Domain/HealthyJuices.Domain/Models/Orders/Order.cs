using System;
using HealthyJuices.Domain.Models.Abstraction;
using HealthyJuices.Domain.Models.Abstraction.DataAccess.Entities;
using HealthyJuices.Domain.Models.Companies;
using HealthyJuices.Domain.Models.Users;

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

        public Order()
        {
        }

        public Order(User user, DateTime deliveryDate)
        {
            this.DateCreated = DateTime.UtcNow;
            this.DateModified = DateTime.UtcNow;
            this.DeliveryDate = deliveryDate;
            this.User = user;
            this.DestinationCompany = user.Company;
        }

        public void Remove()
        {
            this.IsRemoved = true;
            this.DateModified = DateTime.UtcNow;
        }

        public void Update(DateTime deliveryDate)
        {
            this.DateModified = DateTime.UtcNow;
            DeliveryDate = deliveryDate;
        }
}
}