namespace Elyssa.Core.Domain.Entities;

public class EstateAgentInCompany : BaseEntityLong
{
    public DateTime RegistrationDate { get; set; }
    public Guid? CompanyId { get; set; }
    public bool IsActive { get; set; }
    public bool IsAdmin { get; set; }
    public string EmployeeName { get; set; } = string.Empty;
    public string IdentificationNumber { get; set; } = string.Empty;
    public string DocumentType { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string? UrlImage { get; set; }
    public string? UrlSignatureImage { get; set; }
    public string TokenRecovery { get; set; } = string.Empty;
    public DateTime? TokenExpiration { get; set; }
    public string? Phone { get; set; }
    public Guid? RoleAliasId { get; set; }

    public virtual Company? Company { get; set; }
    public virtual RoleAlias? RoleAlias { get; set; }
    public virtual ICollection<Property> Properties { get; set; } = new List<Property>();
    public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();
}
