# Work Tickets - GolMetrics

## Formato de Tickets

```markdown
### [ID] Título del Ticket

**Epic:** [Epic Name]
**Prioridad:** High / Medium / Low
**Estimación:** S (1-2d) / M (3-5d) / L (1-2w)
**Historias Relacionadas:** [US-XXX]

**Descripción:**
[Descripción detallada del trabajo a realizar]

**Criterios de Aceptación:**
-   [ ] Criterio 1
-   [ ] Criterio 2

**Tareas Técnicas:**
1. [ ] Subtarea 1
2. [ ] Subtarea 2

**Dependencias:**
-   Depende de: [Ticket ID]
```

---

## TICK-001: Setup del Proyecto y Base de Datos

**Epic:** Infraestructura
**Prioridad:** High
**Estimación:** M (3 días)
**Historias Relacionadas:** Prerequisito para todas las US

**Descripción:**
Configurar la estructura base del repositorio con backend (.NET 9), frontend (React + Vite), base de datos PostgreSQL y Entity Framework Core con las entidades iniciales.

**Criterios de Aceptación:**

-   [ ] Repositorio GitHub con estructura de carpetas definida
-   [ ] Backend: Proyecto .NET 9 Web API funcional con Minimal API
-   [ ] Frontend: Proyecto Vite + React + TypeScript funcional
-   [ ] Docker Compose configurado con PostgreSQL 16
-   [ ] EF Core configurado con PostgreSQL
-   [ ] Entidades base creadas: User, Conversation, Message, CachedQuery
-   [ ] Primera migración aplicada exitosamente
-   [ ] README.md con instrucciones de setup local

**Tareas Técnicas:**

1. [ ] `dotnet new webapi -n GolMetrics.Api`
2. [ ] `npm create vite@latest golmetrics-web -- --template react-ts`
3. [ ] Crear `docker-compose.yml` con PostgreSQL
4. [ ] Instalar paquetes: `Npgsql.EntityFrameworkCore.PostgreSQL`, `MediatR`, `FluentValidation`
5. [ ] Crear `AppDbContext` en `/Infrastructure/Persistence`
6. [ ] Definir entidades en `/Core/Domain/Entities`
7. [ ] Configurar Fluent API en `/Infrastructure/Persistence/Configurations`
8. [ ] `dotnet ef migrations add InitialCreate`
9. [ ] Configurar .gitignore, .editorconfig
10. [ ] GitHub Actions workflow básico (build + test)

---

## TICK-002: Sistema de Autenticación Completo

**Epic:** Autenticación
**Prioridad:** High
**Estimación:** M (3 días)
**Historias Relacionadas:** US-001, US-002, US-003, US-004

**Descripción:**
Implementar registro, login, logout con JWT y configuración de API Key (BYOK) usando ASP.NET Core Identity.

**Criterios de Aceptación:**

-   [ ] Endpoint `POST /api/auth/register` funcional
-   [ ] Endpoint `POST /api/auth/login` funcional con JWT
-   [ ] Validación de email único y password seguro
-   [ ] Password hasheado con PBKDF2 (Identity default)
-   [ ] JWT con claims: user_id, email (expiración 7 días)
-   [ ] Middleware de autenticación configurado
-   [ ] Endpoint `PUT /api/user/api-key` para BYOK
-   [ ] API Key almacenada encriptada (AES-256)
-   [ ] Validación de API Key contra API-Football antes de guardar
-   [ ] Tests unitarios de handlers y validators

**Tareas Técnicas:**

1. [ ] Instalar `Microsoft.AspNetCore.Identity.EntityFrameworkCore`
2. [ ] Instalar `Microsoft.AspNetCore.Authentication.JwtBearer`
3. [ ] Crear `Features/Auth/Register.cs` (Vertical Slice)
4. [ ] Crear `Features/Auth/Login.cs` (Vertical Slice)
5. [ ] Crear `Features/User/UpdateApiKey.cs`
6. [ ] Implementar `JwtTokenGenerator` service
7. [ ] Implementar `ApiKeyEncryptionService` (AES-256)
8. [ ] Configurar Identity y JWT en `Program.cs`
9. [ ] Agregar `[Authorize]` a endpoints protegidos
10. [ ] Tests unitarios con xUnit + FluentAssertions

**Dependencias:**

-   Depende de: TICK-001

---

## TICK-003: Integración con Semantic Kernel y Gemini

**Epic:** IA/NLP
**Prioridad:** High
**Estimación:** M (3 días)
**Historias Relacionadas:** US-005a, US-005b

**Descripción:**
Integrar Microsoft.Extensions.AI + Semantic Kernel con Google Gemini para procesamiento de lenguaje natural y function calling.

**Criterios de Aceptación:**

-   [ ] Semantic Kernel configurado y funcionando
-   [ ] Conexión con Gemini API verificada
-   [ ] Plugin `FootballPlugin` con funciones para las 5 consultas Must-Have:
    -   `GetTopScorers`
    -   `GetStandings`
    -   `GetRecentResults`
    -   `GetUpcomingMatches`
    -   `GetTeamStatistics`
-   [ ] Function calling automático basado en intención del usuario
-   [ ] Manejo de contexto conversacional básico
-   [ ] Logs de interacciones con LLM

**Tareas Técnicas:**

1. [ ] Instalar `Microsoft.SemanticKernel`
2. [ ] Instalar `Microsoft.SemanticKernel.Connectors.Google`
3. [ ] Instalar `Microsoft.Extensions.AI.Abstractions`
4. [ ] Crear `Infrastructure/AI/SemanticKernelService.cs`
5. [ ] Crear `Infrastructure/AI/Plugins/FootballPlugin.cs`
6. [ ] Decorar funciones con `[KernelFunction]` y `[Description]`
7. [ ] Configurar DI para Semantic Kernel en `Program.cs`
8. [ ] Implementar prompt del sistema para contexto de fútbol
9. [ ] Tests de integración con mock de Gemini

**Dependencias:**

-   Depende de: TICK-002

---

## TICK-004: Endpoint de Chat y Sistema de Caché

**Epic:** Chat Core
**Prioridad:** High
**Estimación:** L (5 días)
**Historias Relacionadas:** US-006, US-007, US-008

**Descripción:**
Endpoint principal de chat que procesa mensajes, integra con Semantic Kernel, guarda mensajes y respuestas, e implementa caché para reducir llamadas a API-Football.

**Criterios de Aceptación:**

-   [ ] `POST /api/chat/message` funcional
-   [ ] Guarda mensaje del usuario en BD antes de procesar
-   [ ] Procesa con Semantic Kernel y obtiene respuesta
-   [ ] Guarda respuesta de IA en BD
-   [ ] Retorna respuesta formateada en Markdown (tablas)
-   [ ] Caché implementado en PostgreSQL
-   [ ] Hash SHA-256 de parámetros como clave de caché
-   [ ] TTL diferenciado: 30d históricos, 1h actuales, 5min en vivo
-   [ ] Lógica `GetOrSetAsync` en CacheService
-   [ ] Logs indican hit/miss de caché

**Tareas Técnicas:**

1. [ ] Crear `Features/Chat/SendMessage.cs` (Vertical Slice completa)
2. [ ] Implementar handler con MediatR
3. [ ] Integrar con `SemanticKernelService`
4. [ ] Crear `Infrastructure/Services/CacheService.cs`
5. [ ] Implementar hash de parámetros (SHA-256)
6. [ ] Lógica de consultar caché antes de API-Football
7. [ ] Guardar resultado en caché tras consulta exitosa
8. [ ] Formatear respuestas como tablas Markdown
9. [ ] Manejo de errores y timeouts
10. [ ] Tests unitarios del handler
11. [ ] Tests de integración del caché

**Dependencias:**

-   Depende de: TICK-003

---

## TICK-005: Frontend de Autenticación

**Epic:** Frontend
**Prioridad:** High
**Estimación:** M (2 días)
**Historias Relacionadas:** US-001, US-002, US-003, US-004

**Descripción:**
Páginas de login, registro y configuración de API Key con formularios validados e integración con el backend.

**Criterios de Aceptación:**

-   [ ] Página `/login` funcional y estilizada
-   [ ] Página `/register` funcional y estilizada
-   [ ] Página `/settings` para configurar API Key
-   [ ] Validación de formularios con React Hook Form + Zod
-   [ ] Integración con authService (Axios)
-   [ ] Almacena JWT en localStorage tras login
-   [ ] Redirige a `/chat` tras autenticación exitosa
-   [ ] Manejo de errores (email duplicado, credenciales incorrectas)
-   [ ] Rutas protegidas (redirect a login si no autenticado)

**Tareas Técnicas:**

1. [ ] Instalar shadcn/ui, Tailwind CSS, React Hook Form, Zod
2. [ ] Crear `LoginPage.tsx` y `RegisterPage.tsx`
3. [ ] Crear `SettingsPage.tsx` para API Key
4. [ ] Implementar `authService.ts` (Axios interceptors)
5. [ ] Crear `authStore.ts` (Zustand o Context)
6. [ ] Implementar `ProtectedRoute` component
7. [ ] Estilizar con shadcn/ui components
8. [ ] Tests con React Testing Library

**Dependencias:**

-   Depende de: TICK-002

---

## TICK-006: Frontend de Chat UI

**Epic:** Frontend
**Prioridad:** High
**Estimación:** L (5 días)
**Historias Relacionadas:** US-006, US-009, US-010

**Descripción:**
Interfaz principal del chatbot con sidebar de conversaciones, área de mensajes e input de texto.

**Criterios de Aceptación:**

-   [ ] Layout con sidebar (conversaciones) y main area (chat)
-   [ ] Lista de conversaciones ordenada por fecha (más reciente primero)
-   [ ] Área de chat con mensajes (user bubble + assistant bubble)
-   [ ] Input de texto con auto-resize y botón enviar
-   [ ] Renderizado de Markdown en respuestas (tablas, listas, negrita)
-   [ ] Indicador "Escribiendo..." mientras procesa
-   [ ] Carga de conversación existente al hacer clic
-   [ ] Botón "+ Nueva conversación"
-   [ ] Responsive design (mobile-friendly)
-   [ ] Estados: loading, error, empty

**Tareas Técnicas:**

1. [ ] Crear `ChatPage.tsx` con layout principal
2. [ ] Componente `ConversationList.tsx` (sidebar)
3. [ ] Componente `MessageBubble.tsx` (user/assistant)
4. [ ] Componente `ChatInput.tsx` (textarea + submit)
5. [ ] Instalar e integrar `react-markdown` + `remark-gfm`
6. [ ] Implementar `chatService.ts` (API calls)
7. [ ] Crear `chatStore.ts` (estado de conversaciones y mensajes)
8. [ ] Manejo de estados (loading skeleton, error toast, empty state)
9. [ ] Responsive con Tailwind breakpoints
10. [ ] Tests con React Testing Library

**Dependencias:**

-   Depende de: TICK-004, TICK-005

---

## TICK-007: Tests y Despliegue

**Epic:** DevOps
**Prioridad:** High
**Estimación:** M (4 días)
**Historias Relacionadas:** Todas (criterio de entrega)

**Descripción:**
Suite de tests completa (unitarios, integración, E2E) y despliegue en producción con CI/CD.

**Criterios de Aceptación:**

-   [ ] Tests unitarios para handlers, validators, services
-   [ ] Tests de integración con Testcontainers (PostgreSQL)
-   [ ] Al menos 1 test E2E: Login -> Enviar mensaje -> Recibir respuesta
-   [ ] Cobertura de código >70%
-   [ ] Pipeline CI ejecuta tests en cada PR
-   [ ] Backend desplegado y accesible (Render/Railway)
-   [ ] Frontend desplegado (static site)
-   [ ] Base de datos PostgreSQL en producción
-   [ ] Variables de entorno/secrets configurados
-   [ ] HTTPS habilitado
-   [ ] URL pública funcional

**Tareas Técnicas:**

1. [ ] Escribir tests unitarios con xUnit + FluentAssertions
2. [ ] Configurar Testcontainers para tests de integración
3. [ ] Instalar Playwright para test E2E
4. [ ] Escribir test E2E del flujo principal
5. [ ] Configurar Coverlet para cobertura
6. [ ] Actualizar GitHub Actions con steps de test y coverage
7. [ ] Crear cuenta en Render.com (o Railway)
8. [ ] Configurar `render.yaml` o Dockerfile para deploy
9. [ ] Crear base de datos PostgreSQL en producción
10. [ ] Configurar secrets (JWT key, Gemini key, DB connection)
11. [ ] Deploy de backend (Docker)
12. [ ] Deploy de frontend (static site)
13. [ ] Verificar flujo completo en producción
14. [ ] Documentar URL de acceso

**Dependencias:**

-   Depende de: TICK-006

---

## Resumen de Tickets

| Ticket   | Descripción                         | Estimación | Dependencias |
| -------- | ----------------------------------- | ---------- | ------------ |
| TICK-001 | Setup del Proyecto y Base de Datos  | M (3d)     | -            |
| TICK-002 | Sistema de Autenticación Completo   | M (3d)     | TICK-001     |
| TICK-003 | Integración Semantic Kernel/Gemini  | M (3d)     | TICK-002     |
| TICK-004 | Endpoint de Chat y Sistema de Caché | L (5d)     | TICK-003     |
| TICK-005 | Frontend de Autenticación           | M (2d)     | TICK-002     |
| TICK-006 | Frontend de Chat UI                 | L (5d)     | TICK-004,005 |
| TICK-007 | Tests y Despliegue                  | M (4d)     | TICK-006 |

**Total:** 7 tickets, ~25 días estimados

---

## Diagrama de Dependencias

```
TICK-001 (Setup)
    |
    v
TICK-002 (Auth Backend)
    |
    +---> TICK-005 (Auth Frontend)
    |           |
    v           |
TICK-003 (Semantic Kernel)
    |           |
    v           |
TICK-004 (Chat Backend + Caché)
    |           |
    +-----------+
    |
    v
TICK-006 (Chat Frontend)
    |
    v
TICK-007 (Tests + Deploy)
```

---

## Mapeo Tickets a Entregas

| Entrega   | Tickets                    | Descripción                        |
| --------- | -------------------------- | ---------------------------------- |
| Entrega 1 | -                          | Documentación (ya completada)      |
| Entrega 2 | TICK-001 a TICK-006        | Código funcional (MVP ejecutable)  |
| Entrega 3 | TICK-007 + refinamientos   | Tests, deploy, documentación final |

---

**Última actualización:** 2025-12-07
**Versión:** 2.0 (Consolidado de 11 a 7 tickets)
