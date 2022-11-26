namespace Domain.Entities.BaseEntities
{
    public class BaseFields : BaseEntity
    {
        public int? Priority { get; set; }
        public string Name { get; set; }
        public string? Details { get; set; }
        public bool Enabled { get; set; }
    }
}
