using Elyssa.Core.Domain.Entities;
using Elyssa.Core.Interfaces;
using Elyssa.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Elyssa.Infrastructure.Repositories;

public class InventoryRepository : Repository<Inventory>, IInventoryRepository
{
    private readonly ILogger<InventoryRepository> _logger;

    public InventoryRepository(ApplicationDbContext context, ILogger<InventoryRepository> logger)
        : base(context)
    {
        _logger = logger;
    }

    public async Task<Inventory?> GetDetailByIdAsync(Guid inventoryId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(i => i.Property)
                .ThenInclude(p => p.PropertyType)
            .Include(i => i.EstateAgent)
            .Include(i => i.StakeHolderSignature)
            .Include(i => i.OwnerSignature)
            .FirstOrDefaultAsync(i => i.Id == inventoryId && i.IsActive, cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<int> CountEnvironmentsByInventoryAsync(Guid inventoryId, CancellationToken cancellationToken = default)
    {
        return await _context.Set<EnvironmentDiagnostic>()
            .AsNoTracking()
            .Where(ed => ed.InventoryId == inventoryId)
            .CountAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<int> CountItemsByInventoryAsync(Guid inventoryId, CancellationToken cancellationToken = default)
    {
        return await _context.Set<ItemDiagnostic>()
            .AsNoTracking()
            .Where(id => id.EnvironmentDiagnostic!.InventoryId == inventoryId)
            .Select(id => id.Id)
            .Distinct()
            .CountAsync(cancellationToken)
            .ConfigureAwait(false);
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
        var query = _dbSet
            .AsNoTracking()
            .Include(i => i.Property)
            .Include(i => i.EstateAgent)
            .Where(i => i.IsActive && i.Property!.CompanyId == companyId);

        if (inventoryType.HasValue)
        {
            query = query.Where(i => i.InventoryType == inventoryType.Value);
        }

        if (isSigned.HasValue)
        {
            query = query.Where(i => i.IsSigned == isSigned.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken).ConfigureAwait(false);

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

        var inventories = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        return (inventories, totalCount);
    }
}
