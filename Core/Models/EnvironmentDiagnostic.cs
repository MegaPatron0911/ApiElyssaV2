namespace Core.Models
{
    public class EnvironmentDiagnostic
    {
        public EnvironmentDiagnostic()
        {
            ItemDiagnostics = new HashSet<ItemDiagnostic>();
        }

        public Guid EnvironmentDiagnosticId { get; set; }

        public string? Description { get; set; }

        public DateTime LastUpdate { get; set; }
        public Guid EnvironmentId { get; set; }
        public string? EnvironmentName { get; set; }
        public virtual Environment Environment { get; set; }

        public Guid InventoryId { get; set; }
        public virtual Inventory Inventory { get; set; }
        public ICollection<ItemDiagnostic> ItemDiagnostics { get; set; }

    }
}
