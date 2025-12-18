# User Stories - GolMetrics

## Formato de Historias

Cada historia sigue el formato estándar:

```
Como [rol]
Quiero [funcionalidad]
Para [beneficio]

Criterios de Aceptación:
- [ ] Criterio 1
- [ ] Criterio 2

Notas Técnicas:
- Detalles de implementación relevantes

Prioridad: Must-Have / Should-Have / Nice-to-Have
Estimación: S (1-2 días) / M (3-5 días) / L (1-2 semanas)
```

---

## Epic 1: Autenticación y Gestión de Usuario

### US-001: Registro de Usuario

**Como** visitante nuevo
**Quiero** registrarme con email y contraseña
**Para** acceder a la plataforma y guardar mi historial de consultas

**Criterios de Aceptación:**

-   [ ] El formulario valida formato de email (regex estándar)
-   [ ] La contraseña debe tener mínimo 8 caracteres, al menos 1 mayúscula, 1 número
-   [ ] Se envía email de confirmación (opcional para MVP, puede omitirse)
-   [ ] Si el email ya existe, muestra error claro: "Este email ya está registrado"
-   [ ] Tras registro exitoso, redirige al login o inicia sesión automáticamente
-   [ ] La contraseña se almacena hasheada (nunca en texto plano)

**Notas Técnicas:**

-   Backend: ASP.NET Core Identity con configuración personalizada
-   Hash: PBKDF2 (por defecto en Identity) o Argon2
-   Validación: FluentValidation en el handler del comando `RegisterCommand`

**Prioridad:** Must-Have
**Estimación:** S (1 día)

---

### US-002: Login de Usuario

**Como** usuario registrado
**Quiero** iniciar sesión con mi email y contraseña
**Para** acceder a mi historial y configuraciones

**Criterios de Aceptación:**

-   [ ] Acepta email y contraseña en formulario
-   [ ] Si credenciales incorrectas: "Email o contraseña incorrectos" (nunca especificar cuál falló)
-   [ ] Tras login exitoso, genera JWT válido con expiración de 7 días
-   [ ] El token incluye claims: `user_id`, `email`, `role` (opcional)
-   [ ] Frontend almacena token en `localStorage` o `sessionStorage`
-   [ ] Redirige al dashboard/chat tras login

**Notas Técnicas:**

-   JWT firmado con clave secreta (configurada en appsettings.json)
-   Middleware de autenticación valida token en cada request protegido
-   Refresh tokens (opcional para MVP)

**Prioridad:** Must-Have
**Estimación:** S (1 día)

---

### US-003: Logout de Usuario

**Como** usuario autenticado
**Quiero** cerrar sesión
**Para** proteger mi cuenta en dispositivos compartidos

**Criterios de Aceptación:**

-   [ ] Botón "Cerrar sesión" visible en header/sidebar
-   [ ] Al cerrar sesión, elimina token del frontend
-   [ ] Redirige al login
-   [ ] No permite acceder a rutas protegidas sin volver a iniciar sesión

**Notas Técnicas:**

-   Frontend: Limpia `localStorage`/`sessionStorage`
-   Backend: (Opcional) Lista negra de tokens revocados si se implementa

**Prioridad:** Must-Have
**Estimación:** S (0.5 días)

---

### US-004: Configurar API Key de API-Football (BYOK)

**Como** usuario autenticado
**Quiero** ingresar mi propia API Key de API-Football en mi perfil
**Para** realizar consultas sin depender de límites globales del sistema

**Criterios de Aceptación:**

-   [ ] Campo de texto en página de "Configuración" o "Perfil"
-   [ ] Al guardar, valida que la key sea válida (hace request de prueba a API-Football)
-   [ ] Si la key es inválida, muestra error: "API Key inválida. Verifica tu clave en api-football.com"
-   [ ] La key se almacena encriptada en la base de datos (AES-256)
-   [ ] El sistema prioriza usar la key del usuario sobre la del sistema (si existe)
-   [ ] Permite actualizar o eliminar la key posteriormente

**Notas Técnicas:**

-   Validación: Request a `/status` o `/timezone` de API-Football
-   Encriptación: `System.Security.Cryptography.Aes` con clave maestra en secrets
-   Modelo: `User.EncryptedApiKey` (string cifrado)

**Prioridad:** Must-Have
**Estimación:** M (2 días)

---

## Epic 2: Consultas de Chat

### US-005a: Interpretación de Intención Simple

**Como** usuario autenticado
**Quiero** hacer una pregunta directa sobre un dato específico (goleadores, tabla, resultado)
**Para** obtener la información sin navegar

**Criterios de Aceptación:**

-   [ ] El sistema identifica correctamente la intención (ej: `GetTopScorers`)
-   [ ] Extrae entidades nombradas básicas (Liga, Equipo, Jugador)
-   [ ] Si falta un dato obligatorio (ej: liga), asume un default razonable o pide aclaración simple
-   [ ] Ejecuta la función correspondiente y devuelve el resultado

**Prioridad:** Must-Have
**Estimación:** M (3 días)

---

### US-005b: Manejo de Contexto Conversacional

**Como** usuario
**Quiero** hacer preguntas de seguimiento ("¿Y quiénes son del Chelsea?")
**Para** refinar mi búsqueda sin repetir toda la frase anterior

**Criterios de Aceptación:**

-   [ ] El sistema recuerda la última consulta realizada (ej: Goleadores Premier)
-   [ ] Al preguntar "¿Y del Chelsea?", aplica el filtro sobre el contexto anterior
-   [ ] Mantiene el contexto durante la sesión de chat activa

**Prioridad:** Must-Have
**Estimación:** M (3 días)

---

### US-006: Visualizar Respuestas Formateadas

**Como** usuario
**Quiero** que las respuestas de estadísticas sean fáciles de leer
**Para** entender rápidamente la información sin procesar texto plano

**Criterios de Aceptación:**

-   [ ] Respuestas de goleadores/asistentes se muestran como tabla HTML o Markdown
-   [ ] Incluye columnas: Posición, Nombre, Equipo, Goles/Asistencias
-   [ ] Las clasificaciones muestran: Posición, Equipo, PJ, PG, PE, PP, Pts
-   [ ] Los resultados de partidos muestran: Fecha, Local, Visitante, Resultado
-   [ ] Usa íconos o badges para estado (Victoria: verde, Empate: amarillo, Derrota: rojo)

**Ejemplo de salida:**

```markdown
**Máximos goleadores Premier League 2024:**

| #   | Jugador        | Equipo          | Goles |
| --- | -------------- | --------------- | ----- |
| 1   | Erling Haaland | Manchester City | 27    |
| 2   | Harry Kane     | Tottenham       | 23    |
| 3   | Mohamed Salah  | Liverpool       | 19    |
```

**Notas Técnicas:**

-   Frontend: Renderiza Markdown con librería `react-markdown`
-   Backend: Formatea respuesta usando string interpolation o template

**Prioridad:** Must-Have
**Estimación:** S (1 día)

---

### US-007: Sistema de Caché de Consultas

**Como** usuario del sistema (técnico)
**Quiero** que consultas frecuentes se cacheen automáticamente
**Para** reducir latencia y consumo de cuota de API externa

**Criterios de Aceptación:**

-   [ ] Antes de llamar a API-Football, verifica si existe en caché
-   [ ] La clave de caché es un hash de: `endpoint + parámetros ordenados`
-   [ ] Si existe y no ha expirado (`expires_at > NOW()`), devuelve resultado cacheado
-   [ ] Si no existe o expiró, consulta API-Football y guarda en caché
-   [ ] TTL diferenciado:
    -   Datos históricos (temporadas anteriores): 30 días
    -   Clasificaciones de temporada actual: 1 hora
    -   Partidos en vivo: 5 minutos
-   [ ] Logs indican hit/miss de caché para debugging

**Notas Técnicas:**

-   Tabla: `cached_queries` (id, query_hash, endpoint, params_json, response_data_jsonb, expires_at)
-   Hash: `SHA256(endpoint + JsonSerializer.Serialize(sortedParams))`
-   PostgreSQL JSONB para `response_data`

**Prioridad:** Must-Have
**Estimación:** M (3 días)

---

## Epic 3: Historial de Conversaciones

### US-008: Persistir Mensajes Automáticamente

**Como** usuario
**Quiero** que todas mis conversaciones se guarden automáticamente
**Para** no perder información consultada previamente

**Criterios de Aceptación:**

-   [ ] Cada mensaje enviado/recibido se guarda en BD antes de mostrarse en UI
-   [ ] Estructura: `conversation_id`, `role` (user/assistant), `content`, `timestamp`
-   [ ] Si es la primera pregunta del usuario, crea nueva conversación automáticamente
-   [ ] El título de la conversación se genera de la primera pregunta (primeros 50 caracteres)

**Notas Técnicas:**

-   Tabla: `conversations` (id, user_id, title, created_at, updated_at)
-   Tabla: `messages` (id, conversation_id, role, content, timestamp)
-   Usar transacciones para garantizar consistencia

**Prioridad:** Must-Have
**Estimación:** S (1 día)

---

### US-009: Listar Conversaciones Anteriores

**Como** usuario autenticado
**Quiero** ver una lista de mis conversaciones pasadas
**Para** retomar consultas previas sin empezar de cero

**Criterios de Aceptación:**

-   [ ] Sidebar izquierdo muestra lista de conversaciones ordenadas por `updated_at DESC`
-   [ ] Cada ítem muestra: título, fecha de última actualización
-   [ ] Límite inicial: 20 conversaciones más recientes (paginación opcional)
-   [ ] Al hacer clic en una conversación, carga sus mensajes en el área principal

**Notas Técnicas:**

-   Endpoint: `GET /api/conversations?limit=20&offset=0`
-   Response: `[{ id, title, updated_at, preview (primeros 100 chars) }]`

**Prioridad:** Must-Have
**Estimación:** S (1 día)

---

### US-010: Cargar Conversación Existente

**Como** usuario
**Quiero** hacer clic en una conversación antigua y ver todos sus mensajes
**Para** revisar datos que consulté anteriormente

**Criterios de Aceptación:**

-   [ ] Al seleccionar conversación, se carga en el área de chat
-   [ ] Muestra todos los mensajes en orden cronológico (user → assistant → user...)
-   [ ] Mantiene el scroll en el último mensaje
-   [ ] Permite continuar la conversación enviando nuevos mensajes

**Notas Técnicas:**

-   Endpoint: `GET /api/conversations/{id}/messages`
-   Response: `[{ role, content, timestamp }]`

**Prioridad:** Must-Have
**Estimación:** S (1 día)

---

## Epic 4: Tipos de Consultas Soportadas

### US-011: Consulta de Máximos Goleadores

**Como** aficionado
**Quiero** preguntar "¿Quiénes son los goleadores de [liga] [temporada]?"
**Para** conocer el top 10 de artilleros

**Criterios de Aceptación:**

-   [ ] Reconoce variantes: "goleadores", "máximos goleadores", "pichichis", "top scorers"
-   [ ] Extrae liga (Premier, La Liga, Serie A, Bundesliga, Ligue 1)
-   [ ] Extrae temporada (2024, 2023, etc.) - por defecto: temporada actual
-   [ ] Llama a `/players/topscorers?league={id}&season={year}`
-   [ ] Devuelve tabla con: Posición, Nombre, Equipo, Goles

**Ejemplos:**

-   "Goleadores de la Premier 2024"
-   "Top scorers La Liga"
-   "Pichichis Bundesliga 2022"

**Prioridad:** Must-Have
**Estimación:** S (incluido en US-005)

---

### US-012: Consulta de Clasificación

**Como** aficionado
**Quiero** preguntar "¿Cómo va la tabla de [liga]?"
**Para** ver la clasificación actual

**Criterios de Aceptación:**

-   [ ] Reconoce: "clasificación", "tabla", "standings", "posiciones"
-   [ ] Devuelve top 10 equipos por defecto (opción de expandir)
-   [ ] Columnas: Pos, Equipo, PJ, PG, PE, PP, GF, GC, DG, Pts

**Prioridad:** Must-Have
**Estimación:** S

---

### US-013: Consulta de Resultados Recientes

**Como** aficionado
**Quiero** preguntar "Últimos partidos del [equipo]"
**Para** ver los resultados más recientes de mi equipo favorito

**Criterios de Aceptación:**

-   [ ] Reconoce: "últimos partidos", "resultados recientes", "cómo le fue al"
-   [ ] Extrae nombre del equipo
-   [ ] Llama a `/fixtures?team={id}&last=5`
-   [ ] Muestra los últimos 5 partidos con: Fecha, Rival, Resultado, Local/Visitante

**Ejemplos:**

-   "Últimos partidos del Real Madrid"
-   "Cómo le fue al Barcelona esta semana"

**Prioridad:** Must-Have
**Estimación:** S (incluido en US-005)

---

### US-014: Consulta de Próximos Partidos

**Como** aficionado
**Quiero** preguntar "Próximo partido del [equipo]"
**Para** saber cuándo juega mi equipo

**Criterios de Aceptación:**

-   [ ] Reconoce: "próximo partido", "cuándo juega", "siguiente partido"
-   [ ] Extrae nombre del equipo
-   [ ] Llama a `/fixtures?team={id}&next=3`
-   [ ] Muestra próximos 1-3 partidos con: Fecha, Hora, Rival, Competición

**Ejemplos:**

-   "Próximo partido del Liverpool"
-   "Cuándo juega el Atlético"

**Prioridad:** Must-Have
**Estimación:** S (incluido en US-005)

---

### US-015: Consulta de Estadísticas de Equipo

**Como** aficionado
**Quiero** preguntar "Estadísticas del [equipo] esta temporada"
**Para** conocer el rendimiento general del equipo

**Criterios de Aceptación:**

-   [ ] Reconoce: "estadísticas", "stats", "rendimiento", "números"
-   [ ] Extrae equipo y temporada (default: actual)
-   [ ] Llama a `/teams/statistics?team={id}&season={year}&league={id}`
-   [ ] Muestra: Partidos jugados, Victorias/Empates/Derrotas, Goles a favor/en contra

**Ejemplos:**

-   "Stats del Manchester City 2024"
-   "Rendimiento del Bayern esta temporada"

**Prioridad:** Must-Have
**Estimación:** S (incluido en US-005)

---

## Epic 5: Funcionalidades Should-Have (Opcionales)

### US-016: Consulta de Enfrentamientos Directos

**Como** aficionado
**Quiero** saber el historial de partidos entre dos equipos
**Para** ver quién domina el historial reciente

**Criterios de Aceptación:**

-   [ ] Reconoce patrones como "Madrid vs Barca", "Historial City Arsenal"
-   [ ] Identifica los IDs de los dos equipos involucrados
-   [ ] Llama a `/fixtures/headtohead` de API-Football
-   [ ] Muestra los últimos 5-10 enfrentamientos con fecha y resultado
-   [ ] Muestra un resumen (Ganados A / Empates / Ganados B)

**Prioridad:** Should-Have
**Estimación:** S (1 día)

---

### US-017: Consulta de Información de Jugador

**Como** aficionado
**Quiero** preguntar "Datos de [jugador]"
**Para** conocer estadísticas individuales de un jugador

**Criterios de Aceptación:**

-   [ ] Reconoce: "datos de", "info de", "estadísticas de [nombre]"
-   [ ] Busca jugador por nombre en `/players?search={name}`
-   [ ] Muestra: Nombre, Edad, Nacionalidad, Equipo actual, Goles/Asistencias temporada

**Prioridad:** Should-Have
**Estimación:** S (1 día)

---

### US-018: Consulta de Máximos Asistentes

**Como** aficionado
**Quiero** preguntar "Asistentes de [liga]"
**Para** conocer los jugadores con más asistencias

**Criterios de Aceptación:**

-   [ ] Reconoce: "asistentes", "asistencias", "top assists"
-   [ ] Extrae liga y temporada
-   [ ] Llama a `/players/topassists?league={id}&season={year}`
-   [ ] Devuelve tabla con: Posición, Nombre, Equipo, Asistencias

**Prioridad:** Should-Have
**Estimación:** S (incluido en US-005)

---

### US-019: Dashboard de Métricas

**Como** administrador
**Quiero** ver métricas de uso del sistema
**Para** entender patrones de consultas y optimizar el servicio

**Criterios de Aceptación:**

-   [ ] Panel en `/admin/metrics` (solo accesible para rol Admin)
-   [ ] Muestra: Total consultas/día, tipos más frecuentes, hit rate de caché
-   [ ] Gráfico de líneas de consultas en últimos 7 días

**Prioridad:** Should-Have
**Estimación:** M (3 días)

---

### US-020: Exportar Resultados

**Como** analista de datos
**Quiero** descargar los resultados de una consulta en CSV
**Para** procesarlos en Excel o herramientas de análisis

**Criterios de Aceptación:**

-   [ ] Botón "Exportar CSV" en mensajes del asistente con tablas
-   [ ] Descarga archivo con nombre: `golmetrics_export_{timestamp}.csv`
-   [ ] Mantiene el formato de columnas de la tabla

**Prioridad:** Should-Have
**Estimación:** S (1 día)

---

## Resumen de Prioridades

| Prioridad   | Cantidad | Historias                                          |
| ----------- | -------- | -------------------------------------------------- |
| Must-Have   | 15       | US-001 a US-015 (Auth, BYOK, Chat, 5 consultas, Historial) |
| Should-Have | 5        | US-016 a US-020 (3 consultas extra, Dashboard, Export)     |
| **TOTAL**   | **20**   |                                                    |

### Tipos de Consultas por Prioridad

| Prioridad   | Consultas                                                    |
| ----------- | ------------------------------------------------------------ |
| Must-Have   | Goleadores, Clasificación, Resultados recientes, Próximos partidos, Stats equipo |
| Should-Have | Enfrentamientos directos, Info jugador, Asistentes           |

**Estimación total MVP (Must-Have):** ~15-18 días de desarrollo

---

**Última actualización:** 2025-12-07
**Version:** 1.1 (Simplificado a 5 tipos de consultas Must-Have)
