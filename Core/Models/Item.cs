namespace Core.Models
{
    public partial class Item
    {
        public Item()
        {
            ItemDiagnostics = new HashSet<ItemDiagnostic>();

        }

        public Guid ItemId { get; set; }
        public string ItemName { get; set; }

        public string? ImageUrl { get; set; }

        public virtual DefaultItem DefaultItem { get; set; }
        public virtual ICollection<ItemDiagnostic> ItemDiagnostics { get; set; }
    }
}
