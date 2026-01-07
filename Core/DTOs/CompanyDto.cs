using Core.Enums;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Core.DTO
{
    public class CompanyDto
    {
        public string BusinessName { get; set; }
        public string Tin { get; set; }
        public string Email { get; set; }
        public string AddressNotification { get; set; }
        public string TradeName { get; set; }
        public string CityId { get; set; }
        public string Phone { get; set; }

        public IFormFile Logo { get; set; }
        public Guid CountryId { get; set; }

        public ICollection<PlanCompanyDto> PlanCompanies { get; set; }
    }

    public class CompanyInformationDTO
    {
        public string BusinessName { get; set; }
        public string Tin { get; set; }
        public string Email { get; set; }
        public string AddressNotification { get; set; }
        public string TradeName { get; set; }
        public string CityId { get; set; }
        public string Phone { get; set; }
        public int? Status { get; set; }
        public DateTime CreationDate { get; set; }
        public int AmountActiveProperties { get; set; }
        public PlanCompanyDto PlanCompany { get; set; }
        public string LegalTextDelivery { get; set; }
        public string LegalTextRecruiment { get; set; }
        public string LegalTextReturn { get; set; }
        public string LegalTextNews { get; set; }
        public string Logo { get; set; }
        public Guid CountryId { get; set; }
        public string CountryName { get; set; }
    }

    public class CreateCompanyDTO
    {
        [Required]
        public string BusinessName { get; set; }
        [Required]
        public string Tin { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string AddressNotification { get; set; }
        [Required]
        public string CityId { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public int Status { get; set; }
        [Required]
        public DateTime CreationDate { get; set; }
        [Required]
        public int PlanType { get; set; }
        public string? TradeName { get; set; }
        public int? Coins { get; set; }
        public int? MaxRentedProperties { get; set; }
        public string? LegalTextDelivery { get; set; }
        public string? LegalTextRecruiment { get; set; }
        public string? LegalTextReturn { get; set; }
        public string? LegalTextNews { get; set; }
        public IFormFile? Logo { get; set; }
    }

    public class UpdateCompanyDTO
    {
        [Required]
        public string BusinessName { get; set; }
        [Required]
        public string AddressNotification { get; set; }
        [Required]
        public string CityId { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Tin { get; set; }
        [Required]
        public string Email { get; set; }
    }

    public class UpdateLogoDto
    {
        public Guid CompanyId { get; set; }
        public IFormFile? Logo { get; set; }
    }

    public class UpdateLogoResponse {
        public string LogoUrl { get; set; }
    }

    public class UpdateLegalTextDto
    {
        [Required]
        public Guid CompanyId { get; set; }
        [Required]
        public LegalTextType Type { get; set;}
        [Required]
        public string Description { get; set; }
    }

    public class CompanyBasicInfoResponseDto
    {
        public bool Success { get; set; } = true;
        public CompanyBasicInfoDataDto Data { get; set; } = null!;
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }

    public class CompanyBasicInfoDataDto
    {
        public Guid CompanyId { get; set; }
        public string BusinessName { get; set; } = string.Empty;
        public string Nit { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public int ActiveUsers { get; set; }
        public int ActiveProperties { get; set; }
        public CompanyPlanInfoDto Plan { get; set; } = null!;
    }

    public class CompanyPlanInfoDto
    {
        public string Name { get; set; } = string.Empty;
        public int Type { get; set; }
    }

    public class ErrorResponseDto
    {
        public bool Success { get; set; } = false;
        public ErrorDetailDto Error { get; set; } = null!;
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }

    public class ErrorDetailDto
    {
        public string Code { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string Details { get; set; } = string.Empty;
    }
}
