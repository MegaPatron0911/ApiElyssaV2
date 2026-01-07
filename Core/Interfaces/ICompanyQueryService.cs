using Core.DTO;

namespace Core.Interfaces
{
    public interface ICompanyQueryService
    {
        Task<CompanyInformationDTO> GetCompanyInformationAsync(Guid CompanyId);
        Task<CompanyBasicInfoDataDto?> GetCompanyBasicInfoAsync(Guid companyId);
    }
}
