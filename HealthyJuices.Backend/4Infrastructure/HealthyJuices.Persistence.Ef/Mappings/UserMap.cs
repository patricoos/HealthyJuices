using HealthyJuices.Domain.Models.Orders;
using HealthyJuices.Domain.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthyJuices.Persistence.Ef.Mappings
{
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasMany<Order>()
                .WithOne(x => x.User);

            builder.OwnsOne(p => p.Password, cb =>
            {
                cb.Property(c => c.Text).HasColumnName("Password");
                cb.Property(c => c.Salt).HasColumnName("PasswordSalt");
            });
        }
    }
}