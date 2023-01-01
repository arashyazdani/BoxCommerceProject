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
    internal class VehiclesPartConfiguration : IEntityTypeConfiguration<VehiclesPart>
    {
        public void Configure(EntityTypeBuilder<VehiclesPart> builder)
        {
            builder.HasKey(x => new { x.ProductId, x.VehicleId });
        }
    }
}
