using Elyssa.Core.DTOs;
using FluentValidation;

namespace Elyssa.Core.Validators;

public class CompanyDtoValidator : AbstractValidator<CompanyDto>
{
    public CompanyDtoValidator()
    {
        RuleFor(x => x.TradeName)
            .MaximumLength(200)
            .WithMessage("El nombre comercial no puede exceder los 200 caracteres");

        RuleFor(x => x.BusinessName)
            .MaximumLength(200)
            .WithMessage("La razón social no puede exceder los 200 caracteres");

        RuleFor(x => x)
            .Must(x => !string.IsNullOrWhiteSpace(x.TradeName) || !string.IsNullOrWhiteSpace(x.BusinessName))
            .WithMessage("Debe proporcionar al menos el nombre comercial o la razón social");

        RuleFor(x => x.Tin)
            .MaximumLength(50)
            .WithMessage("El NIT no puede exceder los 50 caracteres");

        RuleFor(x => x.Email)
            .EmailAddress()
            .When(x => !string.IsNullOrWhiteSpace(x.Email))
            .WithMessage("El email no es válido")
            .MaximumLength(100)
            .WithMessage("El email no puede exceder los 100 caracteres");

        RuleFor(x => x.Phone)
            .MaximumLength(20)
            .WithMessage("El teléfono no puede exceder los 20 caracteres")
            .Matches(@"^\+?[0-9\s\-\(\)]+$")
            .When(x => !string.IsNullOrWhiteSpace(x.Phone))
            .WithMessage("El teléfono contiene caracteres inválidos");

        RuleFor(x => x.CountryId)
            .NotEmpty()
            .WithMessage("El país es requerido");
    }
}
