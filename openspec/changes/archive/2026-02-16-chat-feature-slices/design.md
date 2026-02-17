## Context

The chat domain layer is complete (Conversation, Message entities with EF configurations) and the AI pipeline is operational (SemanticKernelService + FootballPlugin + CacheService). What's missing are the Minimal API slices that expose these capabilities to users. The existing EndpointNames.cs partially defines routes but has inconsistencies with the chat spec that must be resolved during implementation.

## Goals / Non-Goals

**Goals:**
- Implement all 4 chat API slices following the established vertical slice pattern
- Resolve route and error inconsistencies between the chat spec and EndpointNames.cs
- Maintain consistency with existing slice patterns (Auth, UserManagement)

**Non-Goals:**
- Pagination for conversations or messages (can be added later)
- WebSocket/SSE for real-time message streaming
- Conversation deletion or title updates
- Message editing or deletion
- Rate limiting on chat endpoints

## Decisions

### D1: Route alignment

The chat spec defines `POST /api/chat/message` but EndpointNames has `/api/chat`. The spec also defines `GET /api/conversations/{id}/messages` but EndpointNames has `/api/conversations/{id:guid}` without the `/messages` suffix.

**Decision**: Align EndpointNames with the spec routes:
- `SendMessage` -> `/api/chat/message`
- `GetConversationMessages` -> `/api/conversations/{id:guid}/messages`
- Add `CreateConversation` -> `/api/conversations`

**Rationale**: The spec is the source of truth. The `/messages` suffix on GetConversationMessages makes the resource hierarchy explicit and leaves room for a future `GET /api/conversations/{id}` endpoint that returns conversation metadata only.

### D2: AiProcessingFailed error handling (HTTP 502)

The spec requires HTTP 502 for AI failures, but `ErrorCategory` only maps to 400/401/403/404/409. Adding `BadGateway` to ErrorCategory requires modifying a core abstraction.

**Decision**: Add `BadGateway` to `ErrorCategory` enum and map it to `StatusCodes.Status502BadGateway` in `ResultExtensions.ToProblemDetails()`.

**Rationale**: 502 is semantically correct (the API is acting as a gateway to the AI service which failed). Extending the enum is a minimal, backward-compatible change that benefits any future upstream service errors. The alternative (catching exceptions and returning 502 manually) would bypass the Result pattern.

### D3: Slice structure

Each slice follows the established pattern: `internal sealed class : ISlice` with nested Command/Query, Validator, Handler, and DTOs.

- **SendMessage**: Command (write operation) with Validator. Handler creates conversation if needed, persists user message, calls SemanticKernelService, persists assistant response.
- **GetConversations**: Query (read). No validator needed. Returns user's conversations.
- **GetConversationMessages**: Query (read). No validator needed. Returns messages for a conversation after ownership check.
- **CreateConversation**: Command (write). Validator for title. Returns 201 with conversation details.

### D4: Conversation auto-creation in SendMessage

When no `conversationId` is provided, SendMessage auto-creates a conversation. The title is set to the first 100 characters of the message, truncated at a word boundary.

**Decision**: Implement truncation by finding the last space before position 100. If no space exists (single long word), truncate at 100 characters.

### D5: Ownership enforcement

Both GetConversationMessages and SendMessage (with existing conversationId) must verify the conversation belongs to the authenticated user.

**Decision**: Return 404 (not 403) for conversations that exist but belong to another user, to prevent conversation ID enumeration. Use the same `ChatErrors.ConversationNotFound` error for both cases.

## Risks / Trade-offs

- **[No pagination]** -> Large conversation histories could be slow. Acceptable for MVP; add cursor-based pagination in a follow-up.
- **[Single DB round-trip for ownership check + message fetch]** -> Use a single query with `Where(c => c.Id == id && c.UserId == userId)` to combine existence and ownership checks.
- **[AI failure leaves orphaned user message]** -> Per spec, the user message is persisted before AI processing. If AI fails, the user message remains. This is acceptable as it shows the user what they sent.
