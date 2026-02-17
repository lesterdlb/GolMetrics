## Context

The backend chat API is fully implemented with 4 endpoints: `POST /api/chat/message`, `GET /api/conversations`, `GET /api/conversations/{id}/messages`, and `POST /api/conversations`. The frontend has a `ChatPage.tsx` with hardcoded mock data and an incorrect API call to `/api/chat` using raw `fetch`. There is no conversation management, no chat state store, and no proper API service layer. The auth infrastructure (Zustand store, Axios with JWT interceptors, protected routes) is already in place.

## Goals / Non-Goals

**Goals:**
- Connect ChatPage to real backend API endpoints via Axios
- Implement conversation sidebar with list, selection, and creation
- Create Zustand chat store for conversations and messages state
- Create chat API service module using the existing Axios instance
- Render assistant Markdown responses (including tables) using `react-markdown`
- Show loading/typing indicator while AI processes responses
- Display error toasts on API failures
- Show empty state when no conversations exist
- Auto-scroll to latest message

**Non-Goals:**
- Message streaming/SSE (backend returns full response, not streamed)
- Message editing or deletion
- Conversation deletion or renaming
- Offline support or optimistic updates
- Stats-card special rendering (existing `StatCard` component can remain but AI responses are plain text/Markdown)

## Decisions

### 1. Chat store with Zustand (not React Query)

The app already uses Zustand for auth state. For consistency and simplicity, the chat store will manage conversations list, active conversation, and messages in a single Zustand store. React Query would add a dependency and a different state management pattern for no clear benefit at this scale.

### 2. Chat service module using Axios instance

Create `services/chat-service.ts` that imports the existing `api` Axios instance (which already handles JWT injection and 401 logout). This keeps HTTP concerns separated from UI and store logic. Functions: `getConversations()`, `getConversationMessages(id)`, `sendMessage(content, conversationId?)`, `createConversation(title)`.

### 3. react-markdown + remark-gfm for Markdown rendering

`react-markdown` is already installed per the frontend-core spec. Add `remark-gfm` plugin to support GitHub-flavored Markdown tables, which the AI assistant frequently returns for football statistics. Create a `MarkdownContent` component that wraps `react-markdown` with appropriate styling.

### 4. Layout: sidebar + main chat area

Split ChatPage into a two-column layout: collapsible sidebar on the left for conversation list, main chat area on the right. On mobile, the sidebar will overlay as a drawer. The sidebar contains the conversation list and a "New Conversation" button.

### 5. Toast notifications via shadcn/ui Sonner

Use `sonner` (shadcn/ui's recommended toast library) for error notifications. This provides a consistent, minimal toast system without building custom toast state management.

### 6. Remove stats-card message type

The current `stats-card` type with structured `StatsData` was designed for mock data. The real AI returns Markdown text. Simplify the `Message` type to always be text content. The `StatCard` component and `StatsData` interface become unused and should be removed.

## Risks / Trade-offs

- **[No streaming]** AI responses may take several seconds. The typing indicator mitigates perceived latency, but users won't see partial responses. This is acceptable since the backend doesn't support streaming. -> Mitigation: clear typing indicator with animated dots.
- **[Full conversation reload]** Switching conversations fetches all messages each time. No client-side caching between conversation switches. -> Mitigation: acceptable for MVP; conversations are typically small. Could add per-conversation message caching in the store later.
- **[Sidebar on mobile]** Two-column layout needs careful responsive handling. -> Mitigation: sidebar as overlay/drawer on screens < md breakpoint, toggled via hamburger button.
