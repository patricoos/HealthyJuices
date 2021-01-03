using HealthyJuices.Domain.Models.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthyJuices.Persistence.Ef.Mappings
{
    public class OrderMap : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");

            builder.HasMany<OrderProduct>(x => x.OrderProducts)
                .WithOne(x => x.Order);
        }
    }
}