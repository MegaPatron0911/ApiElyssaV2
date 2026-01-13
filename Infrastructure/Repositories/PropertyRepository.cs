using Elyssa.Core.Domain.Entities;
using Elyssa.Core.Interfaces;
using Elyssa.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

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
        try
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

            var totalCount = await query.CountAsync(cancellationToken).ConfigureAwait(false);

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
                    ? query.OrderBy(p => p.CreatedAt)
                    : query.OrderByDescending(p => p.CreatedAt)
            };

            var properties = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            if (properties.Any())
            {
                var typeIds = properties.Select(p => p.PropertyTypeId).Distinct().ToList();
                
                var propertyTypesQuery = from pt in _context.Set<PropertyType>().AsNoTracking()
                                        where typeIds.Contains(pt.Id)
                                        select new PropertyType 
                                        { 
                                            Id = pt.Id, 
                                            Name = pt.Name,
                                            CreatedAt = DateTime.MinValue,
                                            UpdatedAt = null,
                                            Properties = new List<Property>()
                                        };

                var propertyTypes = await propertyTypesQuery
                    .ToDictionaryAsync(pt => pt.Id, cancellationToken)
                    .ConfigureAwait(false);

                foreach (var property in properties)
                {
                    if (propertyTypes.TryGetValue(property.PropertyTypeId, out var propertyType))
                    {
                        property.PropertyType = propertyType;
                    }
                }
            }

            return (properties, totalCount);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error in GetPagedAsync: {ex.Message} | InnerException: {ex.InnerException?.Message}", ex);
        }
    }

    public async Task<bool> HasInventoriesAsync(Guid propertyId, CancellationToken cancellationToken = default)
    {
        try
        {
            var count = await _context.Database
                .SqlQueryRaw<int>(@"SELECT COUNT(*) as ""Value"" FROM ""Inventory"" WHERE ""PropertyId"" = {0} AND ""IsActive"" = true", propertyId)
                .FirstOrDefaultAsync(cancellationToken)
                .ConfigureAwait(false);
            return count > 0;
        }
        catch
        {
            return false;
        }
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

        try
        {
            var propertyIdsArray = string.Join(",", propertyIdsList.Select(id => $"'{id}'"));
            var sql = $@"
                SELECT DISTINCT ""PropertyId"" 
                FROM ""Inventory"" 
                WHERE ""PropertyId"" IN ({propertyIdsArray})
                AND ""IsActive"" = true";

            var propertiesWithInventories = await _context.Database
                .SqlQueryRaw<Guid>(sql)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            return propertyIdsList.ToDictionary(
                id => id,
                id => propertiesWithInventories.Contains(id)
            );
        }
        catch
        {
            return propertyIdsList.ToDictionary(id => id, id => false);
        }
    }

    public async Task<Property?> GetDetailByIdAsync(
        Guid propertyId, 
        Guid companyId, 
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(p => p.PropertyType)
            .Where(p => p.Id == propertyId 
                     && p.CompanyId == companyId 
                     && p.IsActive)
            .FirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<Property?> GetByIdWithoutCompanyFilterAsync(
        Guid propertyId, 
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(p => p.PropertyType)
            .Where(p => p.Id == propertyId && p.IsActive)
            .FirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<int> CountEnvironmentsByPropertyAsync(
        Guid propertyId, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            var sql = @"SELECT COUNT(*)::integer AS ""Value"" 
                        FROM ""Environment"" 
                        WHERE ""PropertyId"" = {0} AND ""IsActive"" = true";
            
            return await _context.Database
                .SqlQueryRaw<int>(sql, propertyId)
                .FirstOrDefaultAsync(cancellationToken)
                .ConfigureAwait(false);
        }
        catch
        {
            return 0;
        }
    }

    public async Task<int> CountInventoriesByPropertyAsync(
        Guid propertyId, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            var sql = @"SELECT COUNT(*)::integer AS ""Value"" 
                        FROM ""Inventory"" 
                        WHERE ""PropertyId"" = {0} AND ""IsActive"" = true";
            
            return await _context.Database
                .SqlQueryRaw<int>(sql, propertyId)
                .FirstOrDefaultAsync(cancellationToken)
                .ConfigureAwait(false);
        }
        catch
        {
            return 0;
        }
    }
}
