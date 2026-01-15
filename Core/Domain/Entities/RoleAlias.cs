namespace Elyssa.Core.Domain.Entities;

public class RoleAlias : BaseEntity
{
    public string RoleName { get; set; }
    public string? Description { get; set; }
    
    public virtual ICollection<EstateAgentInCompany> EstateAgents { get; set; }
}
