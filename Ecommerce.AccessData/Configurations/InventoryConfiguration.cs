using Ecommerce.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.AccessData.Configurations
{
    public class InventoryConfiguration : IEntityTypeConfiguration<Inventory>
    {
        public void Configure(EntityTypeBuilder<Inventory> builder)
        {
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.WarehouseId).IsRequired();
            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.DateInitial).IsRequired();
            builder.Property(x => x.DateEnd).IsRequired();
            builder.Property(x => x.State).IsRequired();



            // Relations
            builder.HasOne(x => x.Warehouse).WithMany()
                .HasForeignKey(x => x.WarehouseId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.User).WithMany()
                   .HasForeignKey(x => x.UserId)
                   .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
