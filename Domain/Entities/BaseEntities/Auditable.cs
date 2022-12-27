namespace Domain.Entities.BaseEntities
{
    public abstract class Auditable
    {
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }
    }
}
