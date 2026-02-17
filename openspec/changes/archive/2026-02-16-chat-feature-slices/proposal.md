## Why

TICK-011: The chat domain entities (Conversation, Message) and AI infrastructure (SemanticKernelService, FootballPlugin) are fully implemented, but no API endpoints exist for users to interact with the chat system. Users cannot send messages, view conversations, or create new conversations.

## What Changes

- Add `SendMessage` slice (POST /api/chat) that orchestrates user message persistence, AI processing via SemanticKernelService, and assistant response persistence; auto-creates conversations when no conversation ID is provided
- Add `GetConversations` slice (GET /api/conversations) to list the authenticated user's conversations ordered by last update
- Add `GetConversationMessages` slice (GET /api/conversations/{id}/messages) to retrieve messages for a specific conversation
- Add `CreateConversation` slice (POST /api/conversations) to create a standalone empty conversation with a title
- Add `ChatErrors.cs` defining domain-specific error constants for the chat feature
- Update `EndpointNames.cs` to add missing `CreateConversation` and `GetConversationMessages` routes, and fix `SendMessage` route to match spec (`/api/chat/message`)

## Capabilities

### New Capabilities

(none - all requirements are already defined in the existing `chat` spec)

### Modified Capabilities

- `chat`: Fix route inconsistencies (SendMessage route `/api/chat` vs spec's `/api/chat/message`, add missing CreateConversation endpoint, separate GetConversationMessages from GetConversation); fix AiProcessingFailed error category to support HTTP 502

## Impact

- **Code**: New files under `src/GolMetrics.API/Features/Chat/` (4 slice classes + ChatErrors.cs); update `EndpointNames.cs`
- **APIs**: 4 new endpoints; route corrections to align with spec
- **Dependencies**: No new NuGet packages; uses existing MediatR, FluentValidation, SemanticKernel, EF Core
- **Authorization**: All endpoints require authentication; uses existing `Permissions.Conversations.Read` and `Permissions.Conversations.Write`
