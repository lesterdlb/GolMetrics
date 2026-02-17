import api from '@/services/api';
import type { Conversation, Message, SendMessageResponse } from '@/types';

interface ConversationResponse {
  id: string;
  title: string;
  createdAt: string;
  updatedAt: string | null;
}

interface MessageResponse {
  id: string;
  content: string;
  role: string;
  timestamp: string;
}

export async function getConversations(): Promise<Conversation[]> {
  const { data } = await api.get<ConversationResponse[]>('/api/conversations');
  return data;
}

export async function getConversationMessages(conversationId: string): Promise<Message[]> {
  const { data } = await api.get<MessageResponse[]>(
    `/api/conversations/${conversationId}/messages`,
  );
  return data.map((m) => ({
    id: m.id,
    role: m.role.toLowerCase() as Message['role'],
    content: m.content,
    timestamp: new Date(m.timestamp).toLocaleTimeString('en-US', {
      hour12: false,
      hour: '2-digit',
      minute: '2-digit',
    }),
  }));
}

export async function sendMessage(
  content: string,
  conversationId?: string,
): Promise<SendMessageResponse> {
  const { data } = await api.post<SendMessageResponse>('/api/chat/message', {
    content,
    conversationId: conversationId ?? null,
  });
  return data;
}

export async function createConversation(title: string): Promise<Conversation> {
  const { data } = await api.post<Conversation>('/api/conversations', { title });
  return data;
}
