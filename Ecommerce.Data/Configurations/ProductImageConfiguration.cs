using Ecommerce.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.Data.Configurations
{
    class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
    {
        public void Configure(EntityTypeBuilder<ProductImage> builder)
        {
            builder.ToTable("ProductImages");
            builder.HasKey(x => x.ID);
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.ImagePath).HasMaxLength(250);
            builder.Property(x => x.Caption).HasMaxLength(250);
            builder.Property(x => x.ProductID).HasDefaultValue(0);
            builder.Property(x => x.IsDefault).HasDefaultValue(true);
    
        }
    }
}
