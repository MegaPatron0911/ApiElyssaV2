using Elyssa.Core.Domain.Entities;
using Elyssa.Core.Interfaces;
using Elyssa.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Elyssa.Infrastructure.Repositories;

public class InventoryRepository : Repository<Inventory>, IInventoryRepository
{
    private readonly ILogger<InventoryRepository> _logger;

    public InventoryRepository(ApplicationDbContext context, ILogger<InventoryRepository> logger) : base(context)
    {
        _logger = logger;
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
            var result = await _context.Database
                .SqlQueryRaw<int>($@"SELECT COUNT(*)::integer AS ""Value"" FROM ""EnvironmentDiagnostics"" WHERE ""InventoryId"" = '{inventoryId}'")
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
            
            return result.FirstOrDefault();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error counting EnvironmentDiagnostics for inventory {InventoryId}", inventoryId);
            return 0;
        }
    }

    public async Task<int> CountItemsByInventoryAsync(
        Guid inventoryId, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            var sqlQuery = $@"
                SELECT COUNT(*)::integer AS ""Value"" 
                FROM ""ItemDiagnostic"" id
                INNER JOIN ""EnvironmentDiagnostics"" ed 
                    ON id.""EnvironmentDiagnosticId"" = ed.""EnvironmentDiagnosticId""
                WHERE ed.""InventoryId"" = '{inventoryId}'";
            
            var result = await _context.Database
                .SqlQueryRaw<int>(sqlQuery)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
            
            return result.FirstOrDefault();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error counting ItemDiagnostic for inventory {InventoryId}", inventoryId);
            return 0;
        }
    }
}
