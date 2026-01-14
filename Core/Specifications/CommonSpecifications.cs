using Ardalis.Specification;
using Elyssa.Core.Domain.Entities;

namespace Elyssa.Core.Specifications;

public class PropertyEnvironmentsByPropertySpec : Specification<PropertyEnvironment>
{
    public PropertyEnvironmentsByPropertySpec(Guid propertyId)
    {
        Query.Where(e => e.PropertyId == propertyId && e.IsActive);
    }
}

public class EnvironmentDiagnosticsByInventorySpec : Specification<EnvironmentDiagnostic>
{
    public EnvironmentDiagnosticsByInventorySpec(Guid inventoryId)
    {
        Query.Where(ed => ed.InventoryId == inventoryId);
    }
}

public class ItemDiagnosticsByInventorySpec : Specification<ItemDiagnostic>
{
    public ItemDiagnosticsByInventorySpec(Guid inventoryId)
    {
        Query.Where(id => id.EnvironmentDiagnostic!.InventoryId == inventoryId);
    }
}

public class EstateAgentsByCompanySpec : Specification<EstateAgentInCompany>
{
    public EstateAgentsByCompanySpec(Guid companyId)
    {
        Query.Where(e => e.CompanyId == companyId && e.IsActive);
    }
}
