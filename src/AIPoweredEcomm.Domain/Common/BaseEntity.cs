namespace AIPoweredEcomm.Domain.Common;

/// <summary>
/// Base type for all domain entities.
/// </summary>
public abstract class BaseEntity
{
    public Guid Id { get; protected set; } = Guid.NewGuid();
}
