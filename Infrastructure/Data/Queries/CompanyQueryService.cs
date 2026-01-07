using AutoMapper;
using Core.DTO;
using Core.Extensions;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure.Data.Queries
{
    public class CompanyQueryService : ICompanyQueryService
    {
        private readonly ElyssaContext context;
        private readonly IBlobStorageService _blobStorageService;
        private readonly IMapper _mapper;

        public CompanyQueryService(ElyssaContext context, IBlobStorageService blobStorageService, IMapper mapper)
        {
            this.context = context;
            this._blobStorageService = blobStorageService;
            _mapper = mapper;
        }

        public async Task<CompanyInformationDTO> GetCompanyInformationAsync(Guid CompanyId)
        {
            var company = context.Companies.Include(i => i.PlanCompanies).ThenInclude(i => i.PlanType).Include(i=>i.Country).FirstOrDefault(x => x.CompanyId == CompanyId);

            if (company != null)
            {
                if (company.Status != 1)
                {
                    return null;
                }

                var amountActiveProperties = context.Properties
                    .Where(property => property.CompanyId == CompanyId && property.IsActive)
                    .Count();

                var companyInformation = new CompanyInformationDTO
                {
                    BusinessName = company.BusinessName,
                    Tin = company.Tin,
                    Email = company.Email,
                    AddressNotification = company.AddressNotification,
                    TradeName = company.TradeName,
                    CityId = company.CityId,
                    Phone = company.Phone,
                    Status = company.Status,
                    PlanCompany = _mapper.Map<PlanCompanyDto>(company.PlanCompanies.FirstOrDefault(x => x.PlanStatus)),
                    Logo = company.Logo,
                    CreationDate = company.CreationDate,
                    LegalTextDelivery = company.LegalTextDelivery,
                    LegalTextRecruiment = company.LegalTextRecruiment,
                    LegalTextReturn = company.LegalTextReturn,
                    LegalTextNews = company.LegalTextNews,
                    AmountActiveProperties = amountActiveProperties,
                    CountryId = company.CountryId,
                    CountryName = company.Country.CountryName
                };

                if (companyInformation.Logo.IsBase64String())
                {
                    companyInformation.Logo = $"data:image/png;base64,{companyInformation.Logo}";
                    return companyInformation;
                }

                var containerName = "refactoring-empresas";
                var companyLogo = await _blobStorageService.DownloadAsync(companyInformation.Logo, containerName);
                if (companyLogo != null)
                {
                    var uri = _blobStorageService.GetServiceSasUriForContainer(containerName);
                    companyInformation.Logo = companyLogo.Uri + uri;
                }
                else
                {
                    companyInformation.Logo = $"No se encontro el logo de la empresa: {companyInformation.Logo}";
                }

                return companyInformation;
            }

            return null;
        }

        public async Task<CompanyBasicInfoDataDto?> GetCompanyBasicInfoAsync(Guid companyId)
        {
            var company = await context.Companies
                .AsNoTracking()
                .Include(c => c.PlanCompanies)
                    .ThenInclude(pc => pc.PlanType)
                .FirstOrDefaultAsync(c => c.CompanyId == companyId);

            if (company == null)
            {
                return null;
            }

            if (company.Status != 1)
            {
                return null;
            }

            var activeUsersCount = await context.EstateAgentInCompanies
                .AsNoTracking()
                .CountAsync(u => u.CompanyId == companyId && u.IsActive);

            var activePropertiesCount = await context.Properties
                .AsNoTracking()
                .CountAsync(p => p.CompanyId == companyId && p.IsActive);

            var currentPlan = company.PlanCompanies
                .Where(pc => pc.PlanStatus)
                .OrderByDescending(pc => pc.PlanActivationDate)
                .FirstOrDefault();

            return new CompanyBasicInfoDataDto
            {
                CompanyId = company.CompanyId,
                BusinessName = company.BusinessName ?? string.Empty,
                Nit = company.Tin ?? string.Empty,
                Email = company.Email ?? string.Empty,
                CreatedAt = company.CreationDate,
                ActiveUsers = activeUsersCount,
                ActiveProperties = activePropertiesCount,
                Plan = currentPlan != null && currentPlan.PlanType != null
                    ? new CompanyPlanInfoDto
                    {
                        Name = currentPlan.PlanType.PlanTypeName ?? "Sin Plan",
                        Type = company.PlanType
                    }
                    : new CompanyPlanInfoDto
                    {
                        Name = "Sin Plan",
                        Type = 0
                    }
            };
        }
    }
}
