using Elyssa.Core.Domain.Entities;
using Elyssa.Core.Interfaces;

namespace Elyssa.Core.Interfaces;

public interface IUnitOfWork : IDisposable
{
    ICompanyRepository Companies { get; }
    IPropertyRepository Properties { get; }
    IInventoryRepository Inventories { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}
