using Ecommerce.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.AccessData.Configurations
{
    public class KardexInventoryConfiguration : IEntityTypeConfiguration<KardexInventory>
    {
        public void Configure(EntityTypeBuilder<KardexInventory> builder)
        {
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.ProductWarehouseId).IsRequired();
            builder.Property(x => x.Type).IsRequired();
            builder.Property(x => x.Detail).IsRequired();
            builder.Property(x => x.StockPrevious).IsRequired();
            builder.Property(x => x.Quantity).IsRequired();
            builder.Property(x => x.Cost).IsRequired();
            builder.Property(x => x.Stock).IsRequired();
            builder.Property(x => x.Total).IsRequired();
            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.DateRegister).IsRequired();



            // Relations
            builder.HasOne(x => x.ProductWarehouse).WithMany()
                .HasForeignKey(x => x.ProductWarehouseId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.User).WithMany()
                   .HasForeignKey(x => x.UserId)
                   .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
