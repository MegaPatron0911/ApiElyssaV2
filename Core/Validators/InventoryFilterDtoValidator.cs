using Elyssa.Core.Common.Constants;
using Elyssa.Core.DTOs;
using FluentValidation;

namespace Elyssa.Core.Validators;

public class InventoryFilterDtoValidator : AbstractValidator<InventoryFilterDto>
{
    private static readonly HashSet<string> ValidSortFields = new(StringComparer.OrdinalIgnoreCase)
    {
        InventorySortFields.CREATED_AT,
        InventorySortFields.SIGNATURE_DATE,
        InventorySortFields.RENTAL_PRICE
    };

    public InventoryFilterDtoValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(InventoryConstants.MIN_PAGE_NUMBER)
            .WithMessage("El número de página debe ser mayor o igual a 1")
            .WithErrorCode("INVALID_PAGE");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(InventoryConstants.MIN_PAGE_SIZE)
            .LessThanOrEqualTo(InventoryConstants.MAX_PAGE_SIZE)
            .WithMessage("El tamaño de página debe estar entre 1 y 20")
            .WithErrorCode("INVALID_PAGE_SIZE");

        RuleFor(x => x.InventoryType)
            .Must(type => !type.HasValue || (type.Value >= 0 && type.Value <= 3))
            .WithMessage("El tipo de inventario debe estar entre 0 y 3")
            .WithErrorCode("INVALID_INVENTORY_TYPE");

        RuleFor(x => x.SortBy)
            .Must(sortBy => ValidSortFields.Contains(sortBy))
            .WithMessage("El campo de ordenamiento no es válido. Valores permitidos: createdAt, signatureDate, rentalPrice")
            .WithErrorCode("INVALID_SORT_BY");

        RuleFor(x => x.SortOrder)
            .Must(order => order.Equals("asc", StringComparison.OrdinalIgnoreCase) || 
                          order.Equals("desc", StringComparison.OrdinalIgnoreCase))
            .WithMessage("El orden debe ser 'asc' o 'desc'")
            .WithErrorCode("INVALID_SORT_ORDER");
    }
}
