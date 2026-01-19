using Ardalis.Specification;
using Elyssa.Core.Domain.Entities;

namespace Elyssa.Core.Specifications;

public class CompanyByIdWithCountrySpec : Specification<Company>, ISingleResultSpecification<Company>
{
    public CompanyByIdWithCountrySpec(Guid companyId)
    {
        Query
            .Where(c => c.CompanyId == companyId)
            .Include(c => c.Country);
    }
}

public class AllCompaniesSpec : Specification<Company>
{
    public AllCompaniesSpec()
    {
        Query.Include(c => c.Country);
    }
}

public class CompaniesByCountrySpec : Specification<Company>
{
    public CompaniesByCountrySpec(Guid countryId)
    {
        Query
            .Where(c => c.CountryId == countryId)
            .Include(c => c.Country);
    }
}

public class CompaniesByNameSearchSpec : Specification<Company>
{
    public CompaniesByNameSearchSpec(string searchTerm)
    {
        Query.Search(c => c.TradeName, $"%{searchTerm}%");
    }
}

public class CompaniesByPlanTypeSpec : Specification<Company>
{
    public CompaniesByPlanTypeSpec(int planType)
    {
        Query.Where(c => c.PlanType == planType);
    }
}

public class CompaniesByStatusSpec : Specification<Company>
{
    public CompaniesByStatusSpec(int status)
    {
        Query.Where(c => c.Status == status);
    }
}

