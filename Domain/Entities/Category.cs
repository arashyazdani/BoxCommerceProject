using System.ComponentModel;
using Domain.Entities.BaseEntities;

namespace Domain.Entities
{
    [DisplayName("Category Table")]
    public class Category : BaseFields
    {
        public int? ParentCategoryId { get; set; }
        public virtual Category Parent { get; set; }
        public virtual IList<Category> Children { get; set; }
    }
}
