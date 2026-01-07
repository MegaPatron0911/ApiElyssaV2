namespace Core.Models
{
    public class RoleAlias
    {
        public Guid CompanyId { get; set; }
        public Guid RoleAliasId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string InternalRole { get; set; } = null!;
        public string? Description { get; set; }
        public string? Log { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public virtual ICollection<RoleAliasPermission> RoleAliasPermissions { get; set; } = new List<RoleAliasPermission>();
        public virtual Company Company { get; set; } = null!;
    }
}
