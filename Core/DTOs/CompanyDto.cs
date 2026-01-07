using System.ComponentModel.DataAnnotations;

namespace Elyssa.Core.DTOs;

public class CompanyDto
{
    public Guid Id { get; set; }
    
    [Required(ErrorMessage = "El nombre es requerido")]
    [StringLength(200, ErrorMessage = "El nombre no puede exceder los 200 caracteres")]
    public string Name { get; set; } = string.Empty;
    
    [StringLength(1000, ErrorMessage = "La descripción no puede exceder los 1000 caracteres")]
    public string Description { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "El email es requerido")]
    [EmailAddress(ErrorMessage = "El email no es válido")]
    [StringLength(100, ErrorMessage = "El email no puede exceder los 100 caracteres")]
    public string Email { get; set; } = string.Empty;
    
    [Phone(ErrorMessage = "El teléfono no es válido")]
    [StringLength(20, ErrorMessage = "El teléfono no puede exceder los 20 caracteres")]
    public string Phone { get; set; } = string.Empty;
    
    public bool IsActive { get; set; }
}
