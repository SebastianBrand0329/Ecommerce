using Ecommerce.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.AccessData.Configurations
{
    public class InventoryDetailsConfiguration : IEntityTypeConfiguration<InventoryDetails>
    {
        public void Configure(EntityTypeBuilder<InventoryDetails> builder)
        {
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.InventoryId).IsRequired();
            builder.Property(x => x.ProductId).IsRequired();
            builder.Property(x => x.StockPrevious).IsRequired();
            builder.Property(x => x.Stock).IsRequired();
            



            // Relations
            builder.HasOne(x => x.Inventory).WithMany()
                .HasForeignKey(x => x.InventoryId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Product).WithMany()
                   .HasForeignKey(x => x.ProductId)
                   .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
