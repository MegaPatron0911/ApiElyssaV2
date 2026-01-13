using Elyssa.Core.Domain.Entities;
using System.Threading;

namespace Elyssa.Core.Interfaces;

public interface IRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
}

public interface ICompanyRepository : IRepository<Company>
{
    Task<Company?> GetByIdWithPlanAsync(Guid id, CancellationToken cancellationToken = default);
    Task<int> CountActiveUsersByCompanyAsync(Guid companyId, CancellationToken cancellationToken = default);
    Task<int> CountActivePropertiesByCompanyAsync(Guid companyId, CancellationToken cancellationToken = default);
}

public interface IPropertyRepository : IRepository<Property>
{
    Task<(IEnumerable<Property> Properties, int TotalCount)> GetPagedAsync(
        Guid companyId,
        int page,
        int pageSize,
        string? code,
        string? address,
        string? city,
        string sortBy,
        string sortOrder,
        CancellationToken cancellationToken = default);
    
    Task<bool> HasInventoriesAsync(Guid propertyId, CancellationToken cancellationToken = default);
    
    Task<Dictionary<Guid, bool>> GetInventoriesExistenceAsync(IEnumerable<Guid> propertyIds, CancellationToken cancellationToken = default);
    
    Task<Property?> GetDetailByIdAsync(Guid propertyId, Guid companyId, CancellationToken cancellationToken = default);
    
    Task<Property?> GetByIdWithoutCompanyFilterAsync(Guid propertyId, CancellationToken cancellationToken = default);
    
    Task<int> CountEnvironmentsByPropertyAsync(Guid propertyId, CancellationToken cancellationToken = default);
    
    Task<int> CountInventoriesByPropertyAsync(Guid propertyId, CancellationToken cancellationToken = default);
}
