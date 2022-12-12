namespace Domain.Entities.BaseEntities
{
    public abstract class BaseFields
    {
        public int Id { get; set; }
        public int? Priority { get; set; }
        public string Name { get; set; }
        public string? Details { get; set; }
        public bool Enabled { get; set; }
    }
}
