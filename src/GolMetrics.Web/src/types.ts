export interface Message {
  id: string;
  role: 'user' | 'assistant';
  content: string;
  timestamp: string;
}

export interface Conversation {
  id: string;
  title: string;
  createdAt: string;
  updatedAt: string | null;
}

export interface SendMessageResponse {
  content: string;
  conversationId: string;
}

export interface UserProfile {
  id: string;
  email: string;
  hasApiKey: boolean;
  createdAt: string;
}
