namespace GolMetrics.API.Core.Abstractions;

public abstract class Entity
{
    public Guid Id { get; set; }
    public Guid CreatedBy { get; set; }
    public Guid? LastModifiedBy { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime? UpdatedAtUtc { get; set; }
    public uint Version { get; set; }
}