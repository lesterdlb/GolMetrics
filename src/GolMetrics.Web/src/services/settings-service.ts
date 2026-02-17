import api from '@/services/api';
import type { UserProfile } from '@/types';

export async function getProfile(): Promise<UserProfile> {
  const { data } = await api.get<UserProfile>('/api/user/profile');
  return data;
}

export async function updateApiKey(key: string): Promise<void> {
  await api.put('/api/user/api-key', { apiKey: key });
}
