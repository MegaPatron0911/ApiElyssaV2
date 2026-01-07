using Elyssa.Core.Domain.Entities;

namespace Elyssa.Core.Interfaces;

/// <summary>
/// Unit of Work para transacciones
/// </summary>
public interface IUnitOfWork : IDisposable
{
    IRepository<Company> Companies { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}
