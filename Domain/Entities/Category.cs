using System.ComponentModel;
using Domain.Entities.BaseEntities;

namespace Domain.Entities
{
    public class Category : BaseFields
    {
        public int? ParentCategoryId { get; set; }
        public virtual Category Parent { get; set; }
        public virtual ICollection<Category> Children { get; set; }
        // I use this because our database has not many children and we can find out about all of nodes without many joins
        public string? ParentPath { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
