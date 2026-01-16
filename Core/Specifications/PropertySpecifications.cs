using Ardalis.Specification;
using Elyssa.Core.Domain.Entities;

namespace Elyssa.Core.Specifications;

public class PropertiesByCompanySpec : Specification<Property>
{
    public PropertiesByCompanySpec(
        Guid companyId,
        string? code = null,
        string? address = null,
        string? city = null,
        string sortBy = "createdAt",
        bool ascending = false)
    {
        Query
            .Where(p => p.CompanyId == companyId && p.IsActive)
            .Include(p => p.PropertyType);

        if (!string.IsNullOrWhiteSpace(code))
        {
            Query.Search(p => p.Code, $"%{code}%");
        }

        if (!string.IsNullOrWhiteSpace(address))
        {
            Query.Search(p => p.Address, $"%{address}%");
        }

        if (!string.IsNullOrWhiteSpace(city))
        {
            Query.Search(p => p.City, $"%{city}%");
        }

        Query.OrderBy(sortBy.ToLowerInvariant() switch
        {
            "code" => p => p.Code!,
            "address" => p => p.Address,
            "city" => p => p.City,
            _ => p => p.CreationDate.ToString()
        }, ascending);
    }
}

public class PropertyByIdSpec : Specification<Property>, ISingleResultSpecification<Property>
{
    public PropertyByIdSpec(Guid propertyId, Guid companyId)
    {
        Query
            .Where(p => p.CompanyId == propertyId && p.CompanyId == companyId && p.IsActive)
            .Include(p => p.PropertyType);
    }
}

public class PropertyByIdWithoutCompanyFilterSpec : Specification<Property>, ISingleResultSpecification<Property>
{
    public PropertyByIdWithoutCompanyFilterSpec(Guid propertyId)
    {
        Query
            .Where(p => p.CompanyId == propertyId && p.IsActive)
            .Include(p => p.PropertyType);
    }
}
