using Elyssa.Core.Domain.Entities;
using Elyssa.Core.DTOs;
using Elyssa.Core.Interfaces;

namespace Elyssa.Core.Services;

public class CompanyService : ICompanyService
{
    private readonly IRepository<Company> _companyRepository;

    public CompanyService(IRepository<Company> companyRepository)
    {
        _companyRepository = companyRepository;
    }

    public async Task<CompanyDto?> GetByIdAsync(Guid id)
    {
        var company = await _companyRepository.GetByIdAsync(id);
        return company != null ? MapToDto(company) : null;
    }

    public async Task<IEnumerable<CompanyDto>> GetAllAsync()
    {
        var companies = await _companyRepository.GetAllAsync();
        return companies.Select(MapToDto);
    }

    public async Task<CompanyDto> CreateAsync(CompanyDto companyDto)
    {
        var company = new Company
        {
            Id = Guid.NewGuid(),
            Name = companyDto.Name,
            Description = companyDto.Description,
            Email = companyDto.Email,
            Phone = companyDto.Phone,
            IsActive = companyDto.IsActive
        };

        var createdCompany = await _companyRepository.AddAsync(company);
        return MapToDto(createdCompany);
    }

    public async Task UpdateAsync(Guid id, CompanyDto companyDto)
    {
        var company = await _companyRepository.GetByIdAsync(id);
        if (company == null)
            throw new KeyNotFoundException($"Company with ID {id} not found");

        company.Name = companyDto.Name;
        company.Description = companyDto.Description;
        company.Email = companyDto.Email;
        company.Phone = companyDto.Phone;
        company.IsActive = companyDto.IsActive;

        await _companyRepository.UpdateAsync(company);
    }

    public async Task DeleteAsync(Guid id)
    {
        var company = await _companyRepository.GetByIdAsync(id);
        if (company == null)
            throw new KeyNotFoundException($"Company with ID {id} not found");

        await _companyRepository.DeleteAsync(id);
    }

    private static CompanyDto MapToDto(Company company)
    {
        return new CompanyDto
        {
            Id = company.Id,
            Name = company.Name,
            Description = company.Description,
            Email = company.Email,
            Phone = company.Phone,
            IsActive = company.IsActive
        };
    }
}
