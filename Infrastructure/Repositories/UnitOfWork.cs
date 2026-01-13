using Elyssa.Core.Domain.Entities;
using Elyssa.Core.Interfaces;
using Elyssa.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace Elyssa.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IDbContextTransaction? _transaction;
    private ICompanyRepository? _companies;
    private IPropertyRepository? _properties;
    private IInventoryRepository? _inventories;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public ICompanyRepository Companies =>
        _companies ??= new CompanyRepository(_context);

    public IPropertyRepository Properties =>
        _properties ??= new PropertyRepository(_context);

    public IInventoryRepository Inventories =>
        _inventories ??= new InventoryRepository(_context);

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        _transaction = await _context.Database
            .BeginTransactionAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            
            if (_transaction != null)
            {
                await _transaction.CommitAsync(cancellationToken).ConfigureAwait(false);
            }
        }
        catch
        {
            await RollbackTransactionAsync(cancellationToken).ConfigureAwait(false);
            throw;
        }
        finally
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync().ConfigureAwait(false);
                _transaction = null;
            }
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync(cancellationToken).ConfigureAwait(false);
            await _transaction.DisposeAsync().ConfigureAwait(false);
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}
