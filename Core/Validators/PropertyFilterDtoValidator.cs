using Elyssa.Core.Common.Constants;
using Elyssa.Core.DTOs;
using FluentValidation;

namespace Elyssa.Core.Validators;

public class PropertyFilterDtoValidator : AbstractValidator<PropertyFilterDto>
{
    private static readonly HashSet<string> ValidSortFields = new(StringComparer.OrdinalIgnoreCase)
    {
        PropertySortFields.CREATED_AT,
        PropertySortFields.CODE,
        PropertySortFields.ADDRESS,
        PropertySortFields.CITY
    };

    private static readonly HashSet<string> ValidSortOrders = new(StringComparer.OrdinalIgnoreCase)
    {
        SortOrder.ASCENDING,
        SortOrder.DESCENDING
    };

    public PropertyFilterDtoValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(PropertyConstants.MIN_PAGE_NUMBER)
            .WithMessage($"La página debe ser mayor o igual a {PropertyConstants.MIN_PAGE_NUMBER}");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(PropertyConstants.MIN_PAGE_SIZE)
            .WithMessage($"El tamaño de página debe ser mayor o igual a {PropertyConstants.MIN_PAGE_SIZE}")
            .LessThanOrEqualTo(PropertyConstants.MAX_PAGE_SIZE)
            .WithMessage($"El tamaño de página no puede exceder {PropertyConstants.MAX_PAGE_SIZE}");

        RuleFor(x => x.Code)
            .MaximumLength(100)
            .WithMessage("El código no puede exceder los 100 caracteres")
            .When(x => !string.IsNullOrWhiteSpace(x.Code));

        RuleFor(x => x.Address)
            .MaximumLength(500)
            .WithMessage("La dirección no puede exceder los 500 caracteres")
            .When(x => !string.IsNullOrWhiteSpace(x.Address));

        RuleFor(x => x.City)
            .MaximumLength(200)
            .WithMessage("La ciudad no puede exceder los 200 caracteres")
            .When(x => !string.IsNullOrWhiteSpace(x.City));

        RuleFor(x => x.SortBy)
            .Must(sortBy => ValidSortFields.Contains(sortBy))
            .WithMessage($"Campo de orden inválido. Valores permitidos: {string.Join(", ", ValidSortFields)}");

        RuleFor(x => x.SortOrder)
            .Must(sortOrder => ValidSortOrders.Contains(sortOrder))
            .WithMessage($"El orden debe ser '{SortOrder.ASCENDING}' o '{SortOrder.DESCENDING}'");
    }
}
