using Elyssa.Core.Common;
using Elyssa.Core.DTOs;

namespace Elyssa.Core.Interfaces;

public interface ICompanyService
{
    Task<Result<CompanyDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<CompanyDto>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Result<CompanyDto>> CreateAsync(CompanyDto companyDto, CancellationToken cancellationToken = default);
    Task<Result> UpdateAsync(Guid id, CompanyDto companyDto, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    
    Task<Result<CompanyBasicInfoDto>> GetBasicInfoAsync(Guid companyId, CancellationToken cancellationToken = default);
}
