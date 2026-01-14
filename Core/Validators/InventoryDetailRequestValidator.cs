using Elyssa.Core.DTOs;
using FluentValidation;

namespace Elyssa.Core.Validators;

public class InventoryDetailRequestValidator : AbstractValidator<InventoryDetailRequestDto>
{
    public InventoryDetailRequestValidator()
    {
        RuleFor(x => x.InventoryId)
            .NotEmpty()
            .WithMessage("El ID del inventario es requerido")
            .WithErrorCode("INVALID_INVENTORY_ID")
            .Must(id => id != Guid.Empty)
            .WithMessage("El ID del inventario no puede estar vacío")
            .WithErrorCode("INVALID_INVENTORY_ID");
    }
}
