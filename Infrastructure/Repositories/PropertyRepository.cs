using Elyssa.Core.Domain.Entities;
using Elyssa.Core.Interfaces;
using Elyssa.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using EnvironmentEntity = Elyssa.Core.Domain.Entities.Environment;

namespace Elyssa.Infrastructure.Repositories;

public class PropertyRepository : Repository<Property>, IPropertyRepository
{
    public PropertyRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<(IEnumerable<Property> Properties, int TotalCount)> GetPagedAsync(
        Guid companyId,
        int page,
        int pageSize,
        string? code,
        string? address,
        string? city,
        string sortBy,
        string sortOrder,
        CancellationToken cancellationToken = default)
    {
        var query = _dbSet.AsNoTracking()
            .Where(p => p.CompanyId == companyId && p.IsActive);

        if (!string.IsNullOrWhiteSpace(code))
        {
            query = query.Where(p => EF.Functions.ILike(p.Code, $"%{code}%"));
        }

        if (!string.IsNullOrWhiteSpace(address))
        {
            query = query.Where(p => EF.Functions.ILike(p.Address, $"%{address}%"));
        }

        if (!string.IsNullOrWhiteSpace(city))
        {
            query = query.Where(p => EF.Functions.ILike(p.City, $"%{city}%"));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        query = sortBy.ToLowerInvariant() switch
        {
            "code" => sortOrder == "asc"
                ? query.OrderBy(p => p.Code)
                : query.OrderByDescending(p => p.Code),
            "address" => sortOrder == "asc"
                ? query.OrderBy(p => p.Address)
                : query.OrderByDescending(p => p.Address),
            "city" => sortOrder == "asc"
                ? query.OrderBy(p => p.City)
                : query.OrderByDescending(p => p.City),
            _ => sortOrder == "asc"
                ? query.OrderBy(p => p.CreationDate)
                : query.OrderByDescending(p => p.CreationDate)
        };

        var properties = await query
            .Include(p => p.PropertyType)
            .Include(p => p.EstateAgent)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (properties, totalCount);
    }

    public async Task<bool> HasInventoriesAsync(Guid propertyId, CancellationToken cancellationToken = default)
    {
        var count = await _context.Set<Inventory>()
            .Where(i => i.PropertyId == propertyId && i.IsActive)
            .CountAsync(cancellationToken);

        return count > 0;
    }

    public async Task<Dictionary<Guid, bool>> GetInventoriesExistenceAsync(
        IEnumerable<Guid> propertyIds,
        CancellationToken cancellationToken = default)
    {
        var propertyIdsList = propertyIds.ToList();
        if (!propertyIdsList.Any())
        {
            return new Dictionary<Guid, bool>();
        }

        var propertiesWithInventories = await _context.Set<Inventory>()
            .Where(i => propertyIdsList.Contains(i.PropertyId) && i.IsActive)
            .Select(i => i.PropertyId)
            .Distinct()
            .ToListAsync(cancellationToken);

        return propertyIdsList.ToDictionary(
            id => id,
            id => propertiesWithInventories.Contains(id)
        );
    }

    public async Task<Property?> GetDetailByIdAsync(
        Guid propertyId,
        Guid companyId,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(p => p.PropertyType)
            .Where(p => p.PropertyId == propertyId
                     && p.CompanyId == companyId
                     && p.IsActive)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Property?> GetByIdWithoutCompanyFilterAsync(
        Guid propertyId,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(p => p.PropertyType)
            .Include(p => p.EstateAgent)
            .Where(p => p.PropertyId == propertyId && p.IsActive)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<int> CountEnvironmentsByPropertyAsync(
        Guid propertyId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Set<EnvironmentEntity>()
            .Where(e => e.PropertyId == propertyId && e.IsActive)
            .CountAsync(cancellationToken);
    }

    public async Task<int> CountInventoriesByPropertyAsync(
        Guid propertyId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Set<Inventory>()
            .Where(i => i.PropertyId == propertyId && i.IsActive)
            .CountAsync(cancellationToken);
    }

    public override async Task<Property> AddAsync(Property entity, CancellationToken cancellationToken = default)
    {
        entity.CreationDate = DateTime.UtcNow;
        entity.ModificationDate = null;
        await _dbSet.AddAsync(entity, cancellationToken);
        return entity;
    }

    public override Task UpdateAsync(Property entity, CancellationToken cancellationToken = default)
    {
        entity.ModificationDate = DateTime.UtcNow;
        _dbSet.Update(entity);
        return Task.CompletedTask;
    }
}