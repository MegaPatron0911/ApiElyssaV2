using Elyssa.Core.Common;
using Elyssa.Core.DTOs;

namespace Elyssa.Core.Interfaces;

public interface IInventoryService
{
    Task<Result<InventoryDetailResponseDto>> GetDetailAsync(
        Guid inventoryId,
        Guid companyId,
        CancellationToken cancellationToken = default);

    Task<Result<InventoryListResponseDto>> GetPagedInventoriesAsync(
        Guid companyId,
        InventoryFilterDto filter,
        CancellationToken cancellationToken = default);
}
