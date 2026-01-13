namespace Elyssa.Core.Common;

public static class CompanyErrors
{
    public static Error NotFound(Guid id) =>
        Error.NotFound(
            "Company.NotFound",
            $"La compañía con ID '{id}' no fue encontrada",
            "companyId");

    public static Error EmailAlreadyExists(string email) =>
        Error.Conflict(
            "Company.EmailConflict",
            $"Ya existe una compañía con el email '{email}'",
            "email");

    public static Error InvalidEmail(string email) =>
        Error.Validation(
            "Company.InvalidEmail",
            $"El email '{email}' no es válido",
            "email");

    public static Error NameTooShort =>
        Error.Validation(
            "Company.NameTooShort",
            "El nombre debe tener al menos 3 caracteres",
            "name");
}
