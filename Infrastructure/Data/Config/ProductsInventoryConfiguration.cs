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
    internal class ProductsInventoryConfiguration : IEntityTypeConfiguration<ProductsInventory>
    {
        public void Configure(EntityTypeBuilder<ProductsInventory> builder)
        {
            // By using newsequentialid() in SQL Server we have a unique and sequence Id and our index is better than using newid().
            builder.Property(x=>x.Id).HasDefaultValueSql("newsequentialid()");
        }
    }
}
