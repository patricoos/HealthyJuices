using HealthyJuices.Domain.Models.Companies;
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

            builder.HasMany<OrderItem>(x => x.OrderProducts)
                .WithOne(x => x.Order);

            builder.HasOne<Company>(x => x.DestinationCompany)
                .WithMany();
        }
    }
}