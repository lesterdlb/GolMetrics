using Microsoft.AspNetCore.Identity;

namespace GolMetrics.API.Features.UserManagement;

public sealed class User : IdentityUser<Guid>
{
    public string? EncryptedApiKey { get; set; }
    public Guid CreatedBy { get; set; }
    public Guid? LastModifiedBy { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime? UpdatedAtUtc { get; set; }
    public uint Version { get; set; }
}