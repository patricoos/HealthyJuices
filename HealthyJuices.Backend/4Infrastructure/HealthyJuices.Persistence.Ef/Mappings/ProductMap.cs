using HealthyJuices.Domain.Models.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthyJuices.Persistence.Ef.Mappings
{
    public class ProductMap : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");

            builder.OwnsOne(p => p.DefaultPricePerUnit, cb =>
            {
                cb.Property(c => c.Amount).HasColumnName("DefaultPricePerUnitAmount");
                cb.Property(c => c.Currency).HasColumnName("DefaultPricePerUnitCurrency");
            });
        }
    }
}