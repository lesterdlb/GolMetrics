namespace GolMetrics.API;

public static class EndpointNames
{
    public static class Auth
    {
        public const string Register = nameof(Register);
        public const string Login = nameof(Login);

        public static class Routes
        {
            public const string Register = "/api/auth/register";
            public const string Login = "/api/auth/login";
        }
    }

    public static class Chat
    {
        public const string SendMessage = nameof(SendMessage);
        public const string GetConversations = nameof(GetConversations);
        public const string GetConversation = nameof(GetConversation);

        public static class Routes
        {
            public const string SendMessage = "/api/chat";
            public const string GetConversations = "/api/conversations";
            public const string GetConversation = "/api/conversations/{id:guid}";
        }
    }

    public static class Football
    {
        public const string GetStandings = nameof(GetStandings);
        public const string GetMatches = nameof(GetMatches);

        public static class Routes
        {
            public const string GetStandings = "/api/football/standings";
            public const string GetMatches = "/api/football/matches";
        }
    }
}