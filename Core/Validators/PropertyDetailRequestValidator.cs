using Elyssa.Core.DTOs;
using FluentValidation;

namespace Elyssa.Core.Validators;

public class PropertyDetailRequestValidator : AbstractValidator<PropertyDetailRequestDto>
{
    public PropertyDetailRequestValidator()
    {
        RuleFor(x => x.PropertyId)
            .NotEmpty()
            .WithMessage("El ID de la propiedad es requerido")
            .WithErrorCode("INVALID_PROPERTY_ID")
            .Must(id => id != Guid.Empty)
            .WithMessage("El ID de la propiedad no puede estar vacío")
            .WithErrorCode("INVALID_PROPERTY_ID");
    }
}
