using Ecommerce.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.AccessData.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.serialNumber).IsRequired().HasMaxLength(60);
            builder.Property(x => x.Description).IsRequired().HasMaxLength(100);
            builder.Property(x => x.State).IsRequired();
            builder.Property(x => x.Price).IsRequired();
            builder.Property(x => x.Cost).IsRequired();
            builder.Property(x => x.CategoryId).IsRequired();
            builder.Property(x => x.ModelId).IsRequired();
            builder.Property(x => x.ImageUrl).IsRequired(false);
            builder.Property(x => x.ParentId).IsRequired(false);



            // Retaltions

            builder.HasOne(x => x.Category).WithMany().HasForeignKey(x => x.CategoryId).OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(m => m.Model).WithMany().HasForeignKey(m => m.ModelId).OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Parent).WithMany().HasForeignKey(x => x.ParentId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
