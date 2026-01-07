namespace Core.Models
{
    public class RoleAliasPermission
    {
        public Guid RoleAliasPermissionId { get; set; }
        public Guid RoleAliasId { get; set; }
        public string PermissionCode { get; set; } = null!;
        public virtual RoleAlias RoleAlias { get; set; } = null!;
        public virtual Permission Permission { get; set; } = null!;
    }
}
