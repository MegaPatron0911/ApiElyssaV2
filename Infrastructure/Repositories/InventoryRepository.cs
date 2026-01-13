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

    public async Task<(IEnumerable<Inventory> Inventories, int TotalCount)> GetPagedAsync(
        Guid companyId,
        int page,
        int pageSize,
        int? inventoryType,
        bool? isSigned,
        string sortBy,
        string sortOrder,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // Query base: solo inventarios activos de propiedades de la compañía
            var query = _dbSet.AsNoTracking()
                .Include(i => i.Property)
                .Where(i => i.IsActive && i.Property!.CompanyId == companyId);

            // Filtro por tipo de inventario
            if (inventoryType.HasValue)
            {
                query = query.Where(i => i.InventoryType == inventoryType.Value);
            }

            // Filtro por firmado
            if (isSigned.HasValue)
            {
                query = query.Where(i => i.IsSigned == isSigned.Value);
            }

            // Total de registros (antes de paginación)
            var totalCount = await query.CountAsync(cancellationToken).ConfigureAwait(false);

            // Ordenamiento
            query = sortBy.ToLowerInvariant() switch
            {
                "signaturedate" => sortOrder == "asc"
                    ? query.OrderBy(i => i.SignatureDate)
                    : query.OrderByDescending(i => i.SignatureDate),
                "rentalprice" => sortOrder == "asc"
                    ? query.OrderBy(i => i.RentalPrice)
                    : query.OrderByDescending(i => i.RentalPrice),
                _ => sortOrder == "asc"
                    ? query.OrderBy(i => i.CreatedAt)
                    : query.OrderByDescending(i => i.CreatedAt)
            };

            // Paginación
            var inventories = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            return (inventories, totalCount);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetPagedAsync for company {CompanyId}", companyId);
            throw;
        }
    }
}
