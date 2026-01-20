# API Specification - GolMetrics

## Base URL

-   **Desarrollo:** `https://localhost:7000`
-   **Producción:** `https://api.golmetrics.com` (pendiente)

## Autenticación

Todos los endpoints excepto `/auth/*` requieren JWT en header:

```http
Authorization: Bearer {jwt_token}
```

---

## Endpoints

### 1. Autenticación

#### POST /api/auth/register

```json
// Request
{
  "email": "user@example.com",
  "password": "SecurePass123"
}

// Response 201
{
  "userId": "a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11",
  "email": "user@example.com",
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}

// Error 400
{
  "error": "Email ya registrado"
}
```

#### POST /api/auth/login

```json
// Request
{
  "email": "user@example.com",
  "password": "SecurePass123"
}

// Response 200
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiresAt": "2024-10-17T14:00:00Z"
}

// Error 401
{
  "error": "Email o contraseña incorrectos"
}
```

---

### 2. Usuario

#### PUT /api/user/api-key

```json
// Request
{
  "apiKey": "your-api-football-key"
}

// Response 200
{
  "message": "API Key actualizada correctamente"
}

// Error 400
{
  "error": "API Key inválida. Verifica tu clave en api-football.com"
}
```

#### GET /api/user/profile

```json
// Response 200
{
	"id": "a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11",
	"email": "user@example.com",
	"hasApiKey": true,
	"createdAt": "2024-10-01T10:00:00Z"
}
```

---

### 3. Chat

#### POST /api/chat/message

```json
// Request
{
  "conversationId": "b1eebc99-9c0b-4ef8-bb6d-6bb9bd380a22",
  "content": "¿Quiénes son los goleadores de la Premier 2024?"
}

// Response 200
{
  "response": "**Máximos goleadores Premier League 2024:**\n\n| # | Jugador | Equipo | Goles |\n|---|---------|--------|-------|\n| 1 | E. Haaland | Man City | 27 |",
  "timestamp": "2024-10-10T14:00:03Z"
}

// Error 429 (Rate Limit)
{
  "error": "Límite de consultas alcanzado. Intenta en 1 minuto."
}
```

#### GET /api/conversations

```json
// Response 200
{
	"conversations": [
		{
			"id": "b1eebc99-9c0b-4ef8-bb6d-6bb9bd380a22",
			"title": "Goleadores Premier League 2024",
			"updatedAt": "2024-10-10T14:05:00Z",
			"messageCount": 4
		}
	],
	"total": 15
}
```

#### GET /api/conversations/{id}/messages

```json
// Response 200
{
	"messages": [
		{
			"id": "c1eebc99-9c0b-4ef8-bb6d-6bb9bd380a33",
			"role": "user",
			"content": "¿Quiénes son los goleadores de la Premier 2024?",
			"timestamp": "2024-10-10T14:00:00Z"
		},
		{
			"id": "d1eebc99-9c0b-4ef8-bb6d-6bb9bd380a44",
			"role": "assistant",
			"content": "**Máximos goleadores...**",
			"timestamp": "2024-10-10T14:00:03Z"
		}
	]
}
```

#### POST /api/conversations

```json
// Request (opcional, crea automáticamente si no se provee conversationId)
{
  "title": "Nueva consulta"
}

// Response 201
{
  "id": "new-conversation-id",
  "title": "Nueva consulta",
  "createdAt": "2024-10-10T15:00:00Z"
}
```

---

## Códigos de Error

| Código | Significado           | Ejemplo                    |
| ------ | --------------------- | -------------------------- |
| 200    | OK                    | Operación exitosa          |
| 201    | Created               | Recurso creado             |
| 400    | Bad Request           | Validación fallida         |
| 401    | Unauthorized          | JWT inválido o expirado    |
| 403    | Forbidden             | Acceso denegado a recurso  |
| 404    | Not Found             | Conversación no encontrada |
| 429    | Too Many Requests     | Rate limit excedido        |
| 500    | Internal Server Error | Error del servidor         |

---

## Testing con cURL

```bash
# Login
curl -X POST https://localhost:7000/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"user@example.com","password":"SecurePass123"}'

# Enviar mensaje
curl -X POST https://localhost:7000/api/chat/message \
  -H "Authorization: Bearer {token}" \
  -H "Content-Type: application/json" \
  -d '{"conversationId":"b1ee...","content":"Goleadores Premier 2024"}'
```

---

**Última actualización:** 2025-10-10
