namespace Elyssa.Core.Domain.Entities;

public class EstateAgent : BaseEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<EstateAgentInCompany> CompanyAssociations { get; set; } = new List<EstateAgentInCompany>();
}
