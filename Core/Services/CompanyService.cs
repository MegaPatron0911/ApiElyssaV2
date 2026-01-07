using Elyssa.Core.Common;
using Elyssa.Core.Domain.Entities;
using Elyssa.Core.DTOs;
using Elyssa.Core.Interfaces;

namespace Elyssa.Core.Services;

public class CompanyService : ICompanyService
{
    private readonly IUnitOfWork _unitOfWork;

    public CompanyService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<CompanyDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var company = await _unitOfWork.Companies.GetByIdAsync(id, cancellationToken);
        
        if (company == null)
            return Result<CompanyDto>.Failure(CompanyErrors.NotFound(id));

        return Result<CompanyDto>.Success(MapToDto(company));
    }

    public async Task<Result<IEnumerable<CompanyDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var companies = await _unitOfWork.Companies.GetAllAsync(cancellationToken);
        var companiesDto = companies.Select(MapToDto);
        
        return Result<IEnumerable<CompanyDto>>.Success(companiesDto);
    }

    public async Task<Result<CompanyDto>> CreateAsync(CompanyDto companyDto, CancellationToken cancellationToken = default)
    {
        // Validación de negocio
        if (string.IsNullOrWhiteSpace(companyDto.Name) || companyDto.Name.Length < 3)
            return Result<CompanyDto>.Failure(CompanyErrors.NameTooShort);

        var company = new Company
        {
            Id = Guid.NewGuid(),
            Name = companyDto.Name,
            Description = companyDto.Description,
            Email = companyDto.Email,
            Phone = companyDto.Phone,
            IsActive = companyDto.IsActive
        };

        var createdCompany = await _unitOfWork.Companies.AddAsync(company, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<CompanyDto>.Success(MapToDto(createdCompany));
    }

    public async Task<Result> UpdateAsync(Guid id, CompanyDto companyDto, CancellationToken cancellationToken = default)
    {
        var company = await _unitOfWork.Companies.GetByIdAsync(id, cancellationToken);
        
        if (company == null)
            return Result.Failure(CompanyErrors.NotFound(id));

        // Validación de negocio
        if (string.IsNullOrWhiteSpace(companyDto.Name) || companyDto.Name.Length < 3)
            return Result.Failure(CompanyErrors.NameTooShort);

        company.Name = companyDto.Name;
        company.Description = companyDto.Description;
        company.Email = companyDto.Email;
        company.Phone = companyDto.Phone;
        company.IsActive = companyDto.IsActive;

        await _unitOfWork.Companies.UpdateAsync(company, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var exists = await _unitOfWork.Companies.ExistsAsync(id, cancellationToken);
        
        if (!exists)
            return Result.Failure(CompanyErrors.NotFound(id));

        await _unitOfWork.Companies.DeleteAsync(id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
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
