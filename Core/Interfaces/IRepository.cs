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
