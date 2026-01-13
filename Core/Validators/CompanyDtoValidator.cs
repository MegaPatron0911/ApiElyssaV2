using Elyssa.Core.DTOs;
using FluentValidation;

namespace Elyssa.Core.Validators;

public class CompanyDtoValidator : AbstractValidator<CompanyDto>
{
    public CompanyDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("El nombre es requerido")
            .MinimumLength(3)
            .WithMessage("El nombre debe tener al menos 3 caracteres")
            .MaximumLength(200)
            .WithMessage("El nombre no puede exceder los 200 caracteres");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("El email es requerido")
            .EmailAddress()
            .WithMessage("El email no es válido")
            .MaximumLength(100)
            .WithMessage("El email no puede exceder los 100 caracteres");

        RuleFor(x => x.Phone)
            .MaximumLength(20)
            .WithMessage("El teléfono no puede exceder los 20 caracteres")
            .Matches(@"^\+?[0-9\s\-\(\)]+$")
            .When(x => !string.IsNullOrWhiteSpace(x.Phone))
            .WithMessage("El teléfono contiene caracteres inválidos");

        RuleFor(x => x.Description)
            .MaximumLength(1000)
            .WithMessage("La descripción no puede exceder los 1000 caracteres");
    }
}
