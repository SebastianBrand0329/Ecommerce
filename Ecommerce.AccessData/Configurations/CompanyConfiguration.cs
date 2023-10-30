using Ecommerce.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.AccessData.Configurations
{
    public class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.Name).IsRequired().HasMaxLength(60);
            builder.Property(x => x.Description).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Country).IsRequired();
            builder.Property(x => x.City).IsRequired();
            builder.Property(x => x.Address).IsRequired();
            builder.Property(x => x.Phone).IsRequired();
            builder.Property(x => x.WarehouseId).IsRequired();
            builder.Property(x => x.UserCreateId).IsRequired();
            builder.Property(x => x.UserUpdateId).IsRequired();

            /* Relaciones */

            builder.HasOne(x => x.Warehouse).WithMany()
                   .HasForeignKey(x => x.WarehouseId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.UserCreate).WithMany()
                   .HasForeignKey(x => x.UserCreateId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.UserUpdate).WithMany()
                   .HasForeignKey(x => x.UserUpdateId)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
