using Ecommerce.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.AccessData.Configurations
{
    public class ShoppingCarConfiguration : IEntityTypeConfiguration<ShoppingCar>
    {
        public void Configure(EntityTypeBuilder<ShoppingCar> builder)
        {
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.ProductId).IsRequired();
            builder.Property(x => x.Quantity).IsRequired();

            /* Relaciones */

            builder.HasOne(x => x.User).WithMany()
                   .HasForeignKey(x => x.UserId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Product).WithMany()
                   .HasForeignKey(x => x.ProductId)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
