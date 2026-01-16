namespace Elyssa.Core.Domain.Entities;

public class ItemDiagnostic 
{
    public ItemDiagnostic()
    {
        ChildItemDiagnostics = new HashSet<ItemDiagnostic>();
    }
    public Guid ItemDiagnosticId { get; set; }
    public Guid? MaterialId { get; set; }
    public Guid ItemId { get; set; }
    public string? Description { get; set; }
    public int? Rating { get; set; }
    public Guid EnvironmentDiagnosticId { get; set; }
    public Guid? ItemDiagnosticIdSelfReference { get; set; }
    public string? Subject { get; set; }
    public string? Anottation { get; set; }
    public bool IsANew { get; set; }
    public bool ToRepair { get; set; }
    public int Amount { get; set; }
    public int? Order { get; set; }

    public virtual Material? Material { get; set; }
    public virtual Item? Item { get; set; }
    public virtual EnvironmentDiagnostic? EnvironmentDiagnostic { get; set; }
    public virtual ItemDiagnostic? ParentItemDiagnostic { get; set; }
    public virtual ICollection<ItemDiagnostic> ChildItemDiagnostics { get; set; }
}
