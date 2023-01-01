using System.ComponentModel;
using Domain.Entities.BaseEntities;

namespace Domain.Entities
{
    public class Category : BaseFields
    {
        public int? ParentCategoryId { get; set; }
        public virtual Category Parent { get; set; }
        public virtual ICollection<Category> Children { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
