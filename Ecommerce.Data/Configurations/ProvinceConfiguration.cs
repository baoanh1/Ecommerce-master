using Ecommerce.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.Data.Configurations
{
    class ProvinceConfiguration : IEntityTypeConfiguration<Province>
    {
        public void Configure(EntityTypeBuilder<Province> builder)
        {

            builder.ToTable("Provinces");
            builder.HasKey(x => x.ID);
            builder.Property(x => x.Name).HasMaxLength(200);
            builder.Property(x => x.Code).HasMaxLength(200);
        }
    }
}
