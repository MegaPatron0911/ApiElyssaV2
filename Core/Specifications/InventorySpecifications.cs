using Ardalis.Specification;
using Elyssa.Core.Domain.Entities;

namespace Elyssa.Core.Specifications;

public class InventoriesByCompanySpec : Specification<Inventory>
{
    public InventoriesByCompanySpec(
        Guid companyId,
        int? inventoryType = null,
        bool? isSigned = null,
        string sortBy = "createdAt",
        bool ascending = false)
    {
        Query
            .Where(i => i.IsActive && i.Property!.CompanyId == companyId)
            .Include(i => i.Property);

        if (inventoryType.HasValue)
        {
            Query.Where(i => i.InventoryType == inventoryType.Value);
        }

        if (isSigned.HasValue)
        {
            Query.Where(i => i.IsSigned == isSigned.Value);
        }

        Query.OrderBy(sortBy.ToLowerInvariant() switch
        {
            "signaturedate" => i => i.SignatureDate.ToString()!,
            "rentalprice" => i => i.RentalPrice.ToString(),
            _ => i => i.CreationDate.ToString()
        }, ascending);
    }
}

public class InventoryByIdSpec : Specification<Inventory>, ISingleResultSpecification<Inventory>
{
    public InventoryByIdSpec(Guid inventoryId)
    {
        Query
            .Where(i => i.PropertyId == inventoryId && i.IsActive)
            .Include(i => i.Property);
    }
}

public class InventoriesByPropertySpec : Specification<Inventory>
{
    public InventoriesByPropertySpec(Guid propertyId)
    {
        Query.Where(i => i.PropertyId == propertyId && i.IsActive);
    }
}

public class InventoriesByPropertiesSpec : Specification<Inventory>
{
    public InventoriesByPropertiesSpec(IEnumerable<Guid> propertyIds)
    {
        var idsList = propertyIds.ToList();
        Query.Where(i => idsList.Contains(i.PropertyId) && i.IsActive);
    }
}
