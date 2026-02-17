## 1. Dependencies and Setup

- [x] 1.1 Install `remark-gfm` and `sonner` npm packages in `src/GolMetrics.Web/`
- [x] 1.2 Add `<Toaster />` from `sonner` to the app root in `src/GolMetrics.Web/src/App.tsx`
- [x] 1.3 Verify: `npm run build` succeeds in `src/GolMetrics.Web/`

## 2. Types and API Service

- [x] 2.1 Update `src/GolMetrics.Web/src/types.ts`: remove `StatsData` interface, remove `type` and `data` fields from `Message`, add `Conversation` interface (`id`, `title`, `createdAt`, `updatedAt`), add `SendMessageResponse` interface (`content`, `conversationId`)
- [x] 2.2 Create `src/GolMetrics.Web/src/services/chat-service.ts` with functions: `getConversations()`, `getConversationMessages(id)`, `sendMessage(content, conversationId?)`, `createConversation(title)` — all using the existing `api` Axios instance
- [x] 2.3 Verify: `npm run build` succeeds (may have temporary type errors in components using old types — that's OK, fixed in later steps)

## 3. Chat Store

- [x] 3.1 Create `src/GolMetrics.Web/src/store/chat-store.ts` with Zustand store: state (`conversations`, `activeConversationId`, `messages`, `isLoadingConversations`, `isLoadingMessages`, `isSending`), actions (`fetchConversations`, `selectConversation`, `sendMessage`, `startNewConversation`)
- [x] 3.2 `fetchConversations()` SHALL call `chatService.getConversations()` and update state; show error toast on failure
- [x] 3.3 `selectConversation(id)` SHALL set `activeConversationId`, call `chatService.getConversationMessages(id)`, and populate `messages`; show error toast on failure
- [x] 3.4 `sendMessage(content)` SHALL immediately add user message to `messages`, call `chatService.sendMessage()`, add assistant response to `messages`, update sidebar if new conversation was created; show error toast on failure
- [x] 3.5 `startNewConversation()` SHALL clear `activeConversationId` and `messages`
- [x] 3.6 Verify: `npm run build` succeeds

## 4. UI Components

- [x] 4.1 Create `src/GolMetrics.Web/src/components/chat/MarkdownContent.tsx`: wraps `react-markdown` with `remark-gfm`, applies Tailwind prose styles for tables, code blocks, lists, bold, etc.
- [x] 4.2 Create `src/GolMetrics.Web/src/components/chat/ConversationSidebar.tsx`: renders conversation list from store, "New Conversation" button, highlights active conversation, shows skeleton loaders while loading
- [x] 4.3 Create `src/GolMetrics.Web/src/components/chat/TypingIndicator.tsx`: animated dots indicator styled as an assistant message bubble
- [x] 4.4 Create `src/GolMetrics.Web/src/components/chat/EmptyState.tsx`: centered illustration with prompt text for when no conversations exist
- [x] 4.5 Update `src/GolMetrics.Web/src/components/chat/MessageBubble.tsx`: use `MarkdownContent` for assistant messages instead of plain text; remove `stats-card` type handling
- [x] 4.6 Remove `src/GolMetrics.Web/src/components/chat/StatCard.tsx` (no longer used)
- [x] 4.7 Verify: `npm run build` succeeds

## 5. Chat Page Integration

- [x] 5.1 Rewrite `src/GolMetrics.Web/src/pages/ChatPage.tsx`: replace mock data and raw fetch with `useChatStore`; add two-column layout (sidebar + main area); call `fetchConversations()` on mount; wire message input to `sendMessage()`; show `TypingIndicator` when `isSending`; show `EmptyState` when no conversations and no active chat; auto-scroll to latest message
- [x] 5.2 Add responsive sidebar toggle: hidden by default on mobile (`< md`), show via hamburger button as overlay
- [x] 5.3 Verify: `npm run build` succeeds

## 6. End-to-End Verification

- [ ] 6.1 Manual test: login, see empty state, type a message, verify conversation appears in sidebar, verify assistant response renders as Markdown (USER)
- [ ] 6.2 Manual test: select different conversations from sidebar, verify messages load correctly (USER)
- [ ] 6.3 Manual test: verify error toast appears when backend is unreachable (USER)
- [ ] 6.4 Manual test: verify responsive sidebar toggle on mobile viewport (USER)
