using Elyssa.Core.Common;
using Elyssa.Core.DTOs;

namespace Elyssa.Core.Interfaces;

public interface IPropertyService
{
    Task<Result<PropertyListResponse>> GetPagedPropertiesAsync(
        Guid companyId,
        PropertyFilterDto filter,
        CancellationToken cancellationToken = default);
}
