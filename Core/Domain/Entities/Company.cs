using Elyssa.Core.Common.Constants;

namespace Elyssa.Core.Domain.Entities;

public class Company : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string BusinessName { get; set; } = string.Empty;
    public string Nit { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Status { get; set; } = CompanyStatus.ACTIVE;
    public bool IsActive { get; set; } = true;
    public int PlanType { get; set; } = Common.Constants.PlanType.NO_PLAN;
}
