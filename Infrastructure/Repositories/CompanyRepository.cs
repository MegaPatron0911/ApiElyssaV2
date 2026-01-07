using Elyssa.Core.Domain.Entities;
using Elyssa.Infrastructure.Data;

namespace Elyssa.Infrastructure.Repositories;

public class CompanyRepository : Repository<Company>
{
    public CompanyRepository(ApplicationDbContext context) : base(context)
    {
    }
}
