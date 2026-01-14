namespace Elyssa.Core.Domain.Entities;

public class EstateAgentInCompany : BaseEntity
{
    public Guid EstateAgentId { get; set; }
    public Guid CompanyId { get; set; }
    public bool IsActive { get; set; } = true;

    public EstateAgent? EstateAgent { get; set; }
    public Company? Company { get; set; }
}
