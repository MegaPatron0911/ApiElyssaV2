using Elyssa.Core.Common;
using Elyssa.Core.DTOs;

namespace Elyssa.Core.Interfaces;

public interface IPropertyService
{
    Task<Result<PropertyListResponseDto>> GetPagedPropertiesAsync(
        Guid companyId,
        PropertyFilterDto filter,
        CancellationToken cancellationToken = default);
    
    Task<Result<PropertyDetailResponseDto>> GetDetailAsync(
        Guid propertyId,
        Guid companyId,
        CancellationToken cancellationToken = default);
}
