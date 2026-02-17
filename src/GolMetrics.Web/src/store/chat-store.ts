import { create } from 'zustand';
import { toast } from 'sonner';
import type { Conversation, Message } from '@/types';
import * as chatService from '@/services/chat-service';
import { getCurrentTime } from '@/lib/utils';

interface ChatState {
  conversations: Conversation[];
  activeConversationId: string | null;
  messages: Message[];
  isLoadingConversations: boolean;
  isLoadingMessages: boolean;
  isSending: boolean;
  fetchConversations: () => Promise<void>;
  selectConversation: (id: string) => Promise<void>;
  sendMessage: (content: string) => Promise<void>;
  startNewConversation: () => void;
}

export const useChatStore = create<ChatState>()((set, get) => ({
  conversations: [],
  activeConversationId: null,
  messages: [],
  isLoadingConversations: false,
  isLoadingMessages: false,
  isSending: false,

  fetchConversations: async () => {
    set({ isLoadingConversations: true });
    try {
      const conversations = await chatService.getConversations();
      set({ conversations });
    } catch {
      toast.error('Failed to load conversations');
    } finally {
      set({ isLoadingConversations: false });
    }
  },

  selectConversation: async (id: string) => {
    set({ activeConversationId: id, isLoadingMessages: true, messages: [] });
    try {
      const messages = await chatService.getConversationMessages(id);
      set({ messages });
    } catch {
      toast.error('Failed to load messages');
      set({ activeConversationId: null });
    } finally {
      set({ isLoadingMessages: false });
    }
  },

  sendMessage: async (content: string) => {
    const { activeConversationId } = get();

    const userMessage: Message = {
      id: crypto.randomUUID(),
      role: 'user',
      content,
      timestamp: getCurrentTime(),
    };

    set((state) => ({
      messages: [...state.messages, userMessage],
      isSending: true,
    }));

    try {
      const response = await chatService.sendMessage(
        content,
        activeConversationId ?? undefined,
      );

      const assistantMessage: Message = {
        id: crypto.randomUUID(),
        role: 'assistant',
        content: response.content,
        timestamp: getCurrentTime(),
      };

      set((state) => ({
        messages: [...state.messages, assistantMessage],
        activeConversationId: response.conversationId,
      }));

      if (!activeConversationId) {
        await get().fetchConversations();
      }
    } catch {
      toast.error('Failed to send message');
    } finally {
      set({ isSending: false });
    }
  },

  startNewConversation: () => {
    set({ activeConversationId: null, messages: [] });
  },
}));
