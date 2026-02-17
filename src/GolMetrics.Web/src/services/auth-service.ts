import { jwtDecode } from 'jwt-decode';
import api from '@/services/api';
import type { AuthUser, LoginResponse } from '@/types/auth';

interface JwtPayload {
  sub: string;
  email: string;
}

interface AuthResult {
  token: string;
  user: AuthUser;
}

function extractUser(token: string): AuthUser {
  const payload = jwtDecode<JwtPayload>(token);
  return { id: payload.sub, email: payload.email };
}

export async function login(email: string, password: string): Promise<AuthResult> {
  const { data } = await api.post<LoginResponse>('/api/auth/login', { email, password });
  return { token: data.accessToken, user: extractUser(data.accessToken) };
}

export async function register(email: string, password: string): Promise<AuthResult> {
  const { data } = await api.post<LoginResponse>('/api/auth/register', { email, password });
  return { token: data.accessToken, user: extractUser(data.accessToken) };
}
