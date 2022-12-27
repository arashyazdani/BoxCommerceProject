using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class CategoryConfiguration :  IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(x => x.Name).IsRequired().HasMaxLength(50);
            builder.HasIndex(x => x.Name).IsUnique(true);
            builder.Property(x => x.Enabled).HasDefaultValue(true);
            builder.HasOne(tk => tk.Parent).WithMany(t => t.Children).HasForeignKey(tk => tk.ParentCategoryId);

        }
    }
}
