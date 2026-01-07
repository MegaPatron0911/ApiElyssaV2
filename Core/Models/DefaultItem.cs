namespace Core.Models
{
    public class DefaultItem
    {
        public Guid Id { get; set; }
        public Guid EnvironmentTypeId { get; set; }
        public Guid ItemId { get; set; }
        public Guid? CompanyId { get; set; }
        public int Order { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public virtual Item Item { get; set; }
        public virtual EnvironmentType EnvironmentType { get; set; }
        public virtual Company Company { get; set; }
    }
}
