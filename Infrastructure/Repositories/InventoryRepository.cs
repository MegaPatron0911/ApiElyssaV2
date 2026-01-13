using Elyssa.Core.Domain.Entities;
using Elyssa.Core.Interfaces;
using Elyssa.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Elyssa.Infrastructure.Repositories;

public class InventoryRepository : Repository<Inventory>, IInventoryRepository
{
    public InventoryRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Inventory?> GetDetailByIdAsync(
        Guid inventoryId, 
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(i => i.Property)
            .Where(i => i.Id == inventoryId && i.IsActive)
            .FirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<int> CountEnvironmentsByInventoryAsync(
        Guid inventoryId, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            var sql = @"SELECT COUNT(*)::integer AS ""Value"" 
                        FROM ""EnvironmentDiagnostics"" 
                        WHERE ""InventoryId"" = {0} AND ""IsActive"" = true";
            
            return await _context.Database
                .SqlQueryRaw<int>(sql, inventoryId)
                .FirstOrDefaultAsync(cancellationToken)
                .ConfigureAwait(false);
        }
        catch
        {
            return 0;
        }
    }

    public async Task<int> CountItemsByInventoryAsync(
        Guid inventoryId, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            var sql = @"SELECT COUNT(*)::integer AS ""Value"" 
                        FROM ""ItemDiagnostic"" id
                        INNER JOIN ""EnvironmentDiagnostics"" ed 
                            ON id.""EnvironmentDiagnosticsId"" = ed.""EnvironmentDiagnosticsId""
                        WHERE ed.""InventoryId"" = {0} 
                          AND id.""IsActive"" = true 
                          AND ed.""IsActive"" = true";
            
            return await _context.Database
                .SqlQueryRaw<int>(sql, inventoryId)
                .FirstOrDefaultAsync(cancellationToken)
                .ConfigureAwait(false);
        }
        catch
        {
            return 0;
        }
    }
}
