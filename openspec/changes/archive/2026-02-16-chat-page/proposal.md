## Why

TICK-015: The chat page currently renders with mock/hardcoded data and uses an incorrect API endpoint (`/api/chat` instead of `/api/chat/message`). The backend API is fully implemented (4 endpoints for conversations and messaging), but the frontend lacks API integration, conversation management, and proper message rendering. Users cannot actually chat with the AI assistant or manage conversations.

## What Changes

- Replace mock data with real API calls to all 4 backend chat endpoints
- Add a conversation sidebar with list, selection, and new-conversation creation
- Create a Zustand chat store for conversation and message state management
- Create a chat API service module using the existing Axios instance
- Render assistant messages as Markdown (including tables) using `react-markdown`
- Add loading/typing indicators during AI response processing
- Add error toast notifications for API failures
- Add empty state when no conversations exist
- Auto-scroll to latest message on new messages

## Capabilities

### New Capabilities

(none - no new specs needed; this implements existing `chat` and `frontend-features` spec requirements)

### Modified Capabilities

- `frontend-features`: Chat Screen requirements are being fully implemented (conversation sidebar, real API calls, markdown rendering, loading states, error toasts, empty state)

## Impact

- **Frontend files modified**: `ChatPage.tsx`, `types.ts`
- **Frontend files created**: chat store (`store/chat-store.ts`), chat service (`services/chat-service.ts`), conversation sidebar component, markdown message renderer
- **Dependencies**: `react-markdown` (already installed per frontend-core spec), may need `remark-gfm` for GitHub-flavored Markdown table support
- **Backend**: No changes required - all endpoints are already implemented
- **API endpoints consumed**: `POST /api/chat/message`, `GET /api/conversations`, `GET /api/conversations/{id}/messages`, `POST /api/conversations`
