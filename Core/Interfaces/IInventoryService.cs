using Elyssa.Core.Common;
using Elyssa.Core.DTOs;

namespace Elyssa.Core.Interfaces;

public interface IInventoryService
{
    Task<Result<InventoryDetailResponse>> GetDetailAsync(
        Guid inventoryId,
        Guid companyId,
        CancellationToken cancellationToken = default);

    Task<Result<InventoryListResponse>> GetPagedInventoriesAsync(
        Guid companyId,
        InventoryFilterDto filter,
        CancellationToken cancellationToken = default);
}
