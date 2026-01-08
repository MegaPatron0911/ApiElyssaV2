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
        return await _dbSet
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<int> CountActiveUsersByCompanyAsync(Guid companyId, CancellationToken cancellationToken = default)
    {
        var sql = @"SELECT COUNT(*)::integer AS ""Value"" FROM ""EstateAgentInCompany"" WHERE ""CompanyId"" = {0} AND ""IsActive"" = true";
        return await _context.Database.SqlQueryRaw<int>(sql, companyId).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<int> CountActivePropertiesByCompanyAsync(Guid companyId, CancellationToken cancellationToken = default)
    {
        var sql = @"SELECT COUNT(*)::integer AS ""Value"" FROM ""Property"" WHERE ""CompanyId"" = {0} AND ""IsActive"" = true";
        return await _context.Database.SqlQueryRaw<int>(sql, companyId).FirstOrDefaultAsync(cancellationToken);
    }
}
