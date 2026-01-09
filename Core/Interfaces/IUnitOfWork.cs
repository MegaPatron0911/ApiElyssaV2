using Elyssa.Core.Domain.Entities;
using Elyssa.Core.Interfaces;

namespace Elyssa.Core.Interfaces;

/// <summary>
/// Unit of Work para transacciones
/// </summary>
public interface IUnitOfWork : IDisposable
{
    ICompanyRepository Companies { get; }
    IPropertyRepository Properties { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}
