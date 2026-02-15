namespace GolMetrics.API.Core.Authorization;

public static class Permissions
{
    public static class Conversations
    {
        public const string Read = "conversations:read";
        public const string Write = "conversations:write";
    }

    public static class Users
    {
        public const string Read = "user:read";
        public const string Write = "user:write";
    }
}