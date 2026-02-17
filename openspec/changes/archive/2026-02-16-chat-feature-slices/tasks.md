## 1. Core Infrastructure Updates

- [x] 1.1 Add `BadGateway` to `ErrorCategory` enum in `src/GolMetrics.API/Core/Results/ErrorCategory.cs`
- [x] 1.2 Add `BadGateway` mapping to `StatusCodes.Status502BadGateway` in `src/GolMetrics.API/Core/Results/ResultExtensions.cs`
- [x] 1.3 Update `EndpointNames.cs` to add `CreateConversation` name and route (`/api/conversations`), rename `GetConversation` to `GetConversationMessages` with route `/api/conversations/{id:guid}/messages`, and update `SendMessage` route to `/api/chat/message`

## 2. Chat Errors

- [x] 2.1 Create `src/GolMetrics.API/Features/Chat/ChatErrors.cs` with static error properties: `ConversationNotFound` (NotFound), `EmptyContent` (BadRequest), `ContentTooLong` (BadRequest), `AiProcessingFailed` (BadGateway)

## 3. CreateConversation Slice

- [x] 3.1 Create `src/GolMetrics.API/Features/Chat/CreateConversation.cs` with Command(`Title`), Response(`Id`, `Title`), Validator (title required), Handler (persist conversation, return 201)
- [x] 3.2 Apply `RequirePermissions(Permissions.Conversations.Write)` on the endpoint

## 4. GetConversations Slice

- [x] 4.1 Create `src/GolMetrics.API/Features/Chat/GetConversations.cs` with Query, Response(`Id`, `Title`, `CreatedAt`, `UpdatedAt`), Handler (return user's conversations ordered by UpdatedAtUtc desc)
- [x] 4.2 Apply `RequirePermissions(Permissions.Conversations.Read)` on the endpoint

## 5. GetConversationMessages Slice

- [x] 5.1 Create `src/GolMetrics.API/Features/Chat/GetConversationMessages.cs` with Query(`ConversationId`), Response(`Id`, `Content`, `Role`, `Timestamp`), Handler (verify ownership, return messages ordered by Timestamp asc, 404 if not found or not owned)
- [x] 5.2 Apply `RequirePermissions(Permissions.Conversations.Read)` on the endpoint

## 6. SendMessage Slice

- [x] 6.1 Create `src/GolMetrics.API/Features/Chat/SendMessage.cs` with Command(`Content`, `ConversationId?`), Response(`Content`, `ConversationId`), Validator (content required, max 4000 chars), Handler:
  - If `ConversationId` provided: verify exists and owned by user (404 if not)
  - If `ConversationId` null: auto-create conversation with title = first 100 chars of content truncated at word boundary
  - Persist user message (role User)
  - Call `ISemanticKernelService.ProcessMessageAsync` with conversation history
  - Persist assistant message (role Assistant)
  - Return 200 with assistant content and conversationId
  - On AI failure: return 502 with `ChatErrors.AiProcessingFailed`
- [x] 6.2 Apply `RequirePermissions(Permissions.Conversations.Write)` on the endpoint

## 7. Verification

- [x] 7.1 Run `dotnet build src/GolMetrics.API/` and verify no compilation errors
- [x] 7.2 Run `dotnet test tests/GolMetrics.API.Tests/` and verify existing tests pass
