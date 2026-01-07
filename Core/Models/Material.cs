namespace Core.Models
{
    public partial class Material
    {
        public Material()
        {
            ItemDiagnostics = new HashSet<ItemDiagnostic>();
        }

        public Guid MaterialId { get; set; }
        public string MaterialName { get; set; }

        public virtual ICollection<ItemDiagnostic> ItemDiagnostics { get; set; }
    }
}
