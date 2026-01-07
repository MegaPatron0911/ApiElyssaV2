using Elyssa.Core.DTOs;

namespace Elyssa.Core.Interfaces;

public interface ICompanyService
{
    Task<CompanyDto?> GetByIdAsync(Guid id);
    Task<IEnumerable<CompanyDto>> GetAllAsync();
    Task<CompanyDto> CreateAsync(CompanyDto companyDto);
    Task UpdateAsync(Guid id, CompanyDto companyDto);
    Task DeleteAsync(Guid id);
}
