namespace Elyssa.Core.DTOs;
public class CompanyBasicInfoDto
{
    public Guid CompanyId { get; set; }
    public string BusinessName { get; set; } = string.Empty;
    public string Nit { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public int ActiveUsers { get; set; }
    public int ActiveProperties { get; set; }
    public PlanInfoDto Plan { get; set; } = null!;
}
public class PlanInfoDto
{
    public string Name { get; set; } = string.Empty;
    public int Type { get; set; }
}
