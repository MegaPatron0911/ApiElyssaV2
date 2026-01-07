namespace Elyssa.Core.Domain.ValueObjects;

public record Address(
    string Street,
    string City,
    string State,
    string Country,
    string ZipCode
);
