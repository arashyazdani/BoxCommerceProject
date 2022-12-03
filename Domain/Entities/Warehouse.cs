using System.ComponentModel;
using Domain.Entities.BaseEntities;

namespace Domain.Entities
{
    [DisplayName("Warehouse Table")]
    public class Warehouse : BaseFields
    {
        public string? Address { get; set; }

    }
}
