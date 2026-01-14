using Elyssa.Core.Domain.Entities;
using Elyssa.Core.Interfaces;
using Elyssa.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Elyssa.Infrastructure.Repositories;

public class CompanyRepository : Repository<Company>, ICompanyRepository
{
    public CompanyRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Company?> GetByIdWithPlanAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Companies
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<int> CountActiveUsersByCompanyAsync(Guid companyId, CancellationToken cancellationToken = default)
    {
        return await _context.Set<EstateAgentInCompany>()
            .AsNoTracking()
            .Where(e => e.CompanyId == companyId && e.IsActive)
            .CountAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<int> CountActivePropertiesByCompanyAsync(Guid companyId, CancellationToken cancellationToken = default)
    {
        return await _context.Set<Property>()
            .AsNoTracking()
            .Where(p => p.CompanyId == companyId && p.IsActive)
            .CountAsync(cancellationToken)
            .ConfigureAwait(false);
    }
}
