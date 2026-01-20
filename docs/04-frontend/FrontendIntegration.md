# Frontend Integration - GolMetrics

## 1. Stack Tecnológico

| Tecnología   | Versión | Propósito         |
| ------------ | ------- | ----------------- |
| React        | 18+     | UI Library        |
| Vite         | 5+      | Build tool        |
| TypeScript   | 5+      | Type safety       |
| shadcn/ui    | Latest  | Component library |
| Tailwind CSS | 3+      | Styling           |
| Axios        | 1+      | HTTP client       |
| Zustand      | 4+      | State management  |
| React Router | 6+      | Routing           |

---

## 2. Estructura del Proyecto

```
/src/GolMetrics.Web
├── /public
├── /src
│   ├── /components          # Componentes reutilizables
│   │   ├── /ui             # shadcn/ui components
│   │   ├── Chat
│   │   │   ├── MessageBubble.tsx
│   │   │   ├── ChatInput.tsx
│   │   │   └── ConversationList.tsx
│   │   └── Auth
│   │       ├── LoginForm.tsx
│   │       └── RegisterForm.tsx
│   │
│   ├── /pages              # Páginas principales
│   │   ├── LoginPage.tsx
│   │   ├── RegisterPage.tsx
│   │   ├── ChatPage.tsx
│   │   └── SettingsPage.tsx
│   │
│   ├── /hooks              # Custom hooks
│   │   ├── useAuth.ts
│   │   ├── useChat.ts
│   │   └── useConversations.ts
│   │
│   ├── /services           # API calls
│   │   ├── api.ts
│   │   ├── authService.ts
│   │   └── chatService.ts
│   │
│   ├── /store              # Zustand stores
│   │   ├── authStore.ts
│   │   └── chatStore.ts
│   │
│   ├── /types              # TypeScript types
│   │   ├── auth.types.ts
│   │   └── chat.types.ts
│   │
│   ├── App.tsx
│   └── main.tsx
│
├── package.json
├── tsconfig.json
├── vite.config.ts
└── tailwind.config.js
```

---

## 3. Estado Global (Zustand)

### authStore.ts

```typescript
import create from 'zustand';

interface AuthState {
	token: string | null;
	user: User | null;
	isAuthenticated: boolean;
	login: (email: string, password: string) => Promise<void>;
	logout: () => void;
}

export const useAuthStore = create<AuthState>(set => ({
	token: localStorage.getItem('token'),
	user: null,
	isAuthenticated: !!localStorage.getItem('token'),

	login: async (email, password) => {
		const response = await authService.login(email, password);
		localStorage.setItem('token', response.token);
		set({ token: response.token, isAuthenticated: true });
	},

	logout: () => {
		localStorage.removeItem('token');
		set({ token: null, user: null, isAuthenticated: false });
	},
}));
```

---

## 4. Servicios API

### api.ts (Axios instance)

```typescript
import axios from 'axios';

const api = axios.create({
	baseURL: import.meta.env.VITE_API_URL || 'https://localhost:7000',
	headers: {
		'Content-Type': 'application/json',
	},
});

// Interceptor para JWT
api.interceptors.request.use(config => {
	const token = localStorage.getItem('token');
	if (token) {
		config.headers.Authorization = `Bearer ${token}`;
	}
	return config;
});

export default api;
```

### chatService.ts

```typescript
import api from './api';

export const chatService = {
	sendMessage: async (conversationId: string, content: string) => {
		const { data } = await api.post('/api/chat/message', {
			conversationId,
			content,
		});
		return data;
	},

	getConversations: async () => {
		const { data } = await api.get('/api/conversations');
		return data.conversations;
	},

	getMessages: async (conversationId: string) => {
		const { data } = await api.get(`/api/conversations/${conversationId}/messages`);
		return data.messages;
	},
};
```

---

## 5. Componentes Principales

### ChatInput.tsx

```typescript
import { useState } from 'react';
import { Button } from '@/components/ui/button';
import { Textarea } from '@/components/ui/textarea';

interface ChatInputProps {
	onSend: (message: string) => void;
	isLoading: boolean;
}

export const ChatInput = ({ onSend, isLoading }: ChatInputProps) => {
	const [input, setInput] = useState('');

	const handleSubmit = () => {
		if (input.trim()) {
			onSend(input);
			setInput('');
		}
	};

	return (
		<div className='flex gap-2 p-4'>
			<Textarea
				value={input}
				onChange={e => setInput(e.target.value)}
				onKeyDown={e => {
					if (e.key === 'Enter' && !e.shiftKey) {
						e.preventDefault();
						handleSubmit();
					}
				}}
				placeholder='Pregunta sobre estadísticas de fútbol...'
				disabled={isLoading}
			/>
			<Button onClick={handleSubmit} disabled={isLoading || !input.trim()}>
				Enviar
			</Button>
		</div>
	);
};
```

### MessageBubble.tsx

```typescript
import ReactMarkdown from 'react-markdown';

interface MessageBubbleProps {
	role: 'user' | 'assistant';
	content: string;
	timestamp: string;
}

export const MessageBubble = ({ role, content, timestamp }: MessageBubbleProps) => {
	const isUser = role === 'user';

	return (
		<div className={`flex ${isUser ? 'justify-end' : 'justify-start'} mb-4`}>
			<div
				className={`max-w-[70%] rounded-lg p-4 ${
					isUser ? 'bg-blue-500 text-white' : 'bg-gray-100 text-gray-900'
				}`}
			>
				{isUser ? <p>{content}</p> : <ReactMarkdown>{content}</ReactMarkdown>}
				<span className='text-xs opacity-70 mt-2 block'>
					{new Date(timestamp).toLocaleTimeString()}
				</span>
			</div>
		</div>
	);
};
```

---

## 6. Rutas

### App.tsx

```typescript
import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { useAuthStore } from './store/authStore';

function App() {
	const isAuthenticated = useAuthStore(state => state.isAuthenticated);

	return (
		<BrowserRouter>
			<Routes>
				<Route path='/login' element={<LoginPage />} />
				<Route path='/register' element={<RegisterPage />} />

				<Route
					path='/chat'
					element={isAuthenticated ? <ChatPage /> : <Navigate to='/login' />}
				/>
				<Route
					path='/settings'
					element={isAuthenticated ? <SettingsPage /> : <Navigate to='/login' />}
				/>

				<Route path='/' element={<Navigate to='/chat' />} />
			</Routes>
		</BrowserRouter>
	);
}
```

---

## 7. Variables de Entorno

### .env.example

```env
VITE_API_URL=https://localhost:7000
VITE_APP_NAME=GolMetrics
```

---

**Última actualización:** 2025-10-10
