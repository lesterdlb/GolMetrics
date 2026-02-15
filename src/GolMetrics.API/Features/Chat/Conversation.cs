using GolMetrics.API.Core.Abstractions;
using GolMetrics.API.Features.UserManagement;

namespace GolMetrics.API.Features.Chat;

public sealed class Conversation : Entity
{
    public string Title { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public ICollection<Message> Messages { get; set; } = [];
}