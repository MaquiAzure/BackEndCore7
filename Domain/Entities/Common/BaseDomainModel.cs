namespace Domain.Entities.Common
{
    public abstract class BaseDomainModel
    {
        public Guid Id { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? LasModifiedDate { get; set; }
        public string? LastModifiedBy { get; set; }
        public bool isActive { get; set; }
    }
}
