using AutoMapper;
using Elyssa.Core.Common;
using Elyssa.Core.Domain.Entities;
using Elyssa.Core.DTOs;
using Elyssa.Core.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace Elyssa.Core.Services;

public class CompanyService : ICompanyService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IMemoryCache _cache;
    private const int CacheExpirationMinutes = 5;

    public CompanyService(IUnitOfWork unitOfWork, IMapper mapper, IMemoryCache cache)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _cache = cache;
    }

    public async Task<Result<CompanyDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var company = await _unitOfWork.Companies.GetByIdAsync(id, cancellationToken);
        
        if (company == null)
            return Result<CompanyDto>.Failure(CompanyErrors.NotFound(id));

        return Result<CompanyDto>.Success(_mapper.Map<CompanyDto>(company));
    }

    public async Task<Result<IEnumerable<CompanyDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var companies = await _unitOfWork.Companies.GetAllAsync(cancellationToken);
        var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);
        
        return Result<IEnumerable<CompanyDto>>.Success(companiesDto);
    }

    public async Task<Result<CompanyDto>> CreateAsync(CompanyDto companyDto, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(companyDto.Name) || companyDto.Name.Length < 3)
            return Result<CompanyDto>.Failure(CompanyErrors.NameTooShort);

        var company = _mapper.Map<Company>(companyDto);
        company.Id = Guid.NewGuid();

        var createdCompany = await _unitOfWork.Companies.AddAsync(company, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<CompanyDto>.Success(_mapper.Map<CompanyDto>(createdCompany));
    }

    public async Task<Result> UpdateAsync(Guid id, CompanyDto companyDto, CancellationToken cancellationToken = default)
    {
        var company = await _unitOfWork.Companies.GetByIdAsync(id, cancellationToken);
        
        if (company == null)
            return Result.Failure(CompanyErrors.NotFound(id));

        if (string.IsNullOrWhiteSpace(companyDto.Name) || companyDto.Name.Length < 3)
            return Result.Failure(CompanyErrors.NameTooShort);

        _mapper.Map(companyDto, company);

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

    public async Task<Result<CompanyBasicInfoDto>> GetBasicInfoAsync(Guid companyId, CancellationToken cancellationToken = default)
    {
        var company = await _unitOfWork.Companies.GetByIdWithPlanAsync(companyId, cancellationToken);
        
        if (company == null)
            return Result<CompanyBasicInfoDto>.Failure(CompanyErrors.NotFound(companyId));

        if (company.Status != 1)
            return Result<CompanyBasicInfoDto>.Failure(
                Error.Validation("Company.Inactive", "La empresa no está activa"));

        var activeUsers = await _unitOfWork.Companies.CountActiveUsersByCompanyAsync(companyId, cancellationToken);
        var activeProperties = await _unitOfWork.Companies.CountActivePropertiesByCompanyAsync(companyId, cancellationToken);

        var planInfo = await GetPlanInfoCachedAsync(company.PlanType, cancellationToken);

        var basicInfo = new CompanyBasicInfoDto
        {
            CompanyId = company.Id,
            BusinessName = company.BusinessName,
            Nit = company.Nit,
            Email = company.Email,
            CreatedAt = company.CreatedAt,
            ActiveUsers = activeUsers,
            ActiveProperties = activeProperties,
            Plan = planInfo
        };

        return Result<CompanyBasicInfoDto>.Success(basicInfo);
    }

    private async Task<PlanInfoDto> GetPlanInfoCachedAsync(int planType, CancellationToken cancellationToken)
    {
        var cacheKey = $"PlanInfo_{planType}";

        if (!_cache.TryGetValue(cacheKey, out PlanInfoDto? planInfo))
        {
            var planName = planType switch
            {
                1 => "Básico",
                2 => "Estándar",
                3 => "Premium",
                _ => "Sin Plan"
            };

            planInfo = new PlanInfoDto
            {
                Name = planName,
                Type = planType
            };

            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(CacheExpirationMinutes));

            _cache.Set(cacheKey, planInfo, cacheOptions);
        }

        return planInfo!;
    }
}
