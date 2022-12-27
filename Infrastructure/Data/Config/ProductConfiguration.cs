using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    internal class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(x => x.Name).IsRequired().HasMaxLength(50);
            builder.HasIndex(x => x.Name).IsUnique(true);
            builder.Property(x => x.Enabled).HasDefaultValue(true);
            builder.Property(x=>x.Price).HasColumnType("decimal(18,2)");
            builder.Property(x => x.IsDiscontinued).HasDefaultValue(false);
            builder.Property(x => x.Quantity).IsRequired();
            builder.HasOne(z => z.Category).WithMany().HasForeignKey(z => z.CategoryId);
        }
    }
}
