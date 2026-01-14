namespace Elyssa.Core.Domain.Entities;

public class RoleAlias : BaseEntity
{
    public string RoleName { get; set; } = string.Empty;
    public string? Description { get; set; }
    
    public virtual ICollection<EstateAgentInCompany> EstateAgents { get; set; } = new List<EstateAgentInCompany>();
}
