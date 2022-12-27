namespace Domain.Entities.BaseEntities
{
    public class BaseFields : Auditable
    {
        public int Id { get; set; }
        public int? Priority { get; set; }
        public string Name { get; set; }
        public string? Details { get; set; }
        public bool Enabled { get; set; }
    }
}
