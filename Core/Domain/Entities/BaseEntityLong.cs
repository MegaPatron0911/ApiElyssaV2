namespace Elyssa.Core.Domain.Entities;

public abstract class BaseEntityLong
{
    public long Id { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
