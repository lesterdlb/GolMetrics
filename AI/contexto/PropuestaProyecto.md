# Propuesta de Proyecto Final - AI4Devs

**Estudiante:** [Tu Nombre]
**Fecha:** 27 de Octubre de 2025
**Tutor:** Jorge / Inés

---

## 1. Resumen

**Nombre del Proyecto:** FootballStats AI (nombre tentativo)

**Descripción:** Chatbot web con inteligencia artificial capaz de responder consultas sobre estadísticas de fútbol en lenguaje natural, utilizando datos reales de competiciones modernas (2010-2025).

**Propósito:** Permitir a aficionados, periodistas y apostadores consultar estadísticas de fútbol de manera conversacional, sin necesidad de navegar por múltiples sitios o conocer estructuras de datos complejas.

---

## 2. Alcance del MVP

### 2.1 Funcionalidades Must-Have

1. **Sistema de autenticación**

    - Registro de usuarios
    - Login/Logout básico
    - Gestión de sesiones

2. **Configuración de API Key**

    - Permitir a usuarios ingresar su propia API Key de API-Football
    - Validación y almacenamiento seguro de credenciales
    - Manejo de límites de uso (100 requests/día en plan gratuito)

3. **Interfaz de Chat**

    - Chat conversacional donde el usuario escribe preguntas en lenguaje natural
    - Interpretación de consultas mediante IA (procesamiento de lenguaje natural)
    - Respuestas formateadas con estadísticas relevantes

4. **Consultas soportadas (inicialmente 8-10 tipos)**

    - Máximos goleadores de una liga/temporada
    - Clasificación actual de una competición
    - Estadísticas de un equipo específico
    - Historial de enfrentamientos entre equipos
    - Resultados recientes de un equipo
    - Información de jugadores
    - Próximos partidos de una liga
    - Estadística de una temporada específica

5. **Historial de conversaciones**

    - Almacenamiento de todas las interacciones del usuario
    - Visualización del historial similar a ChatGPT
    - Capacidad de revisar consultas pasadas

6. **Caché de datos**
    - Sistema de caché en PostgreSQL para reducir llamadas a la API externa
    - Almacenamiento de consultas frecuentes
    - TTL (Time To Live) configurable según tipo de dato

### 2.2 Funcionalidades Should-Have (opcionales)

1. Panel administrativo para visualizar métricas de uso
2. Exportación de estadísticas consultadas (PDF/CSV)
3. Sugerencias de preguntas populares

---

## 3. Stack Tecnológico

### 3.1 Backend

-   **Framework:** .NET 10 (Web API)
-   **Base de datos:** PostgreSQL
-   **ORM:** Entity Framework Core
-   **Autenticación:** ASP.NET Core Identity / JWT
-   **IA/NLP:** Microsoft.Extensions.AI + Semantic Kernel + Google Gemini API (ver Sección 6)
-   **Testing:** xUnit

### 3.2 Frontend

-   **Framework:** React 18+
-   **UI Library:** Material-UI (MUI) / Tailwind CSS / shadcn/ui
-   **HTTP Client:** Axios / fetch API
-   **State Management:** React Context / Zustand
-   **Testing:** Jest + React Testing Library

### 3.3 Infraestructura

-   **Contenedores:** Docker + Docker Compose
-   **CI/CD:** GitHub Actions
-   **Despliegue:** Heroku / Railway / Render
-   **API Externa:** API-Football v3 (https://www.api-football.com)

---

## 4. Arquitectura del Sistema

### 4.1 Diagrama de Alto Nivel

```
┌─────────────┐
│   Usuario   │
└──────┬──────┘
       │
       v
┌─────────────────────────────────┐
│   Frontend (React SPA)          │
│   - Chat UI                     │
│   - Gestión de API Key          │
│   - Historial                   │
└──────────────┬──────────────────┘
               │ HTTPS/REST
               v
┌─────────────────────────────────┐
│   Backend (.NET Web API)        │
│   - Autenticación               │
│   - NLP Processor               │
│   - Query Builder               │
│   - Cache Manager               │
└───────┬──────────────┬──────────┘
        │              │
        v              v
┌──────────────┐  ┌─────────────┐
│  PostgreSQL  │  │API-Football │
│  - Users     │  │  External   │
│  - Chats     │  │   API       │
│  - Cache     │  └─────────────┘
└──────────────┘
```

### 4.2 Flujo de Consulta

1. Usuario envía pregunta en lenguaje natural
2. Backend interpreta la consulta (extrae: liga, equipo, jugador, fecha)
3. Verifica si existe en caché (PostgreSQL)
4. Si no existe, consulta API-Football con la API Key del usuario
5. Almacena resultado en caché
6. Formatea respuesta y la envía al frontend
7. Guarda interacción en historial

---

## 5. Modelo de Datos (Simplificado)

### 5.1 Entidades Principales

**User**

-   Id, Email, PasswordHash, ApiKey (encriptada), CreatedAt

**Conversation**

-   Id, UserId, Title, CreatedAt, UpdatedAt

**Message**

-   Id, ConversationId, Role (user/assistant), Content, Timestamp

**CachedQuery**

-   Id, QueryType, Parameters (JSON), Response (JSON), ExpiresAt, CreatedAt

---

## 6. Estrategia de IA

### 6.1 Opción A: Enfoque Pragmático (Patrones Predefinidos)

**Implementación:**

-   Procesamiento basado en patrones regex y palabras clave
-   Detección de entidades: liga, equipo, jugador, temporada
-   Mapeo directo de intenciones a endpoints específicos de API-Football
-   8-10 tipos de consultas predefinidas codificadas manualmente

**Ejemplo de consultas soportadas:**

1. "Máximos goleadores de [liga] [temporada]" → `/players/topscorers`
2. "Clasificación de [liga]" → `/standings`
3. "Estadísticas de [equipo] en [temporada]" → `/teams/statistics`
4. "Enfrentamientos entre [equipo A] y [equipo B]" → `/fixtures/headtohead`
5. "Resultados recientes de [equipo]" → `/fixtures?team=X&last=5`
6. "Información de [jugador]" → `/players?search=X`
7. "Próximos partidos de [liga]" → `/fixtures?league=X&next=10`
8. "Máximos asistentes de [liga]" → `/players/topassists`

**Ventajas:**

-   ✅ Sin dependencias de APIs externas de IA
-   ✅ Sin costos adicionales
-   ✅ Predecible y controlable
-   ✅ Fácil de testear
-   ✅ Implementación rápida (2-3 días)

**Desventajas:**

-   ❌ Inflexible (solo entiende frases predefinidas)
-   ❌ No maneja variaciones complejas
-   ❌ Cada nuevo tipo de consulta requiere código adicional

---

### 6.2 Opción B: IA Generativa (Microsoft.Extensions.AI + Semantic Kernel + Google Gemini)

**Implementación:**

-   **Microsoft.Extensions.AI** como abstracción unificada para proveedores de IA
-   **Semantic Kernel** como orquestador de IA (.NET SDK nativo)
-   Integración con Google Gemini 1.5 Flash (capa gratuita)
-   Function Calling mediante Plugins nativos de Semantic Kernel
-   Comprensión de lenguaje natural ilimitada
-   Manejo automático de contexto conversacional

**Arquitectura con Microsoft.Extensions.AI + Semantic Kernel:**

```
Usuario: "¿Quiénes son los máximos goleadores de la Premier League 2024?"
    ↓
Microsoft.Extensions.AI (IChatClient)
    ↓
Semantic Kernel + Gemini
    ↓
Selecciona función: get_top_scorers("Premier League", 2024)
    ↓
Plugin ejecuta: GET /players/topscorers?league=39&season=2024
    ↓
Respuesta formateada: "Los máximos goleadores de la Premier League 2024 son..."
```

**Ejemplo de código:**

```csharp
using Microsoft.Extensions.AI;
using Microsoft.SemanticKernel;

public class FootballStatsPlugin
{
    [KernelFunction("get_top_scorers")]
    [Description("Obtiene los máximos goleadores de una liga y temporada")]
    public async Task<string> GetTopScorers(
        [Description("Nombre de la liga")] string league,
        [Description("Año de la temporada")] int season
    )
    {
        var result = await _apiFootballService.GetTopScorers(league, season);
        return JsonSerializer.Serialize(result);
    }

    // 7-9 funciones más para otros tipos de consultas...
}

// Configuración con Microsoft.Extensions.AI
var kernel = Kernel.CreateBuilder()
    .AddGoogleAIGemini("API_KEY")
    .Build();
kernel.Plugins.AddFromType<FootballStatsPlugin>();

// IChatClient desde Semantic Kernel
IChatClient chatClient = kernel.GetRequiredService<IChatClient>();

// Ejecución automática
var response = await chatClient.CompleteAsync(userInput);
```

**Ventajas:**

-   ✅ Gratuito (Gemini: 15 req/min, 1500 req/día)
-   ✅ Flexible (entiende múltiples variaciones)
-   ✅ No requiere tarjeta de crédito
-   ✅ Escalable sin código adicional
-   ✅ Respuestas más naturales
-   ✅ **SDK nativo de .NET (integración simplificada)**
-   ✅ **Abstracción agnóstica del proveedor (Microsoft.Extensions.AI)**
-   ✅ **Planner automático (decide qué funciones llamar)**
-   ✅ **Memoria conversacional incluida**
-   ✅ **Multi-LLM (fácil cambiar entre modelos sin cambiar código)**

**Desventajas:**

-   ❌ Dependencia externa (límites de Google)
-   ❌ Requiere manejo de errores de IA
-   ❌ Tiempo de desarrollo: 3-4 días (reducido gracias a Semantic Kernel)

---

### 6.3 Estrategia Recomendada

**Enfoque Directo con Microsoft.Extensions.AI + Semantic Kernel (Recomendado para máster de IA):**

-   Implementar **Opción B** desde el MVP (Entrega 2)
-   Microsoft.Extensions.AI + Semantic Kernel reducen significativamente la complejidad de implementación
-   Demuestra uso real de IA desde el inicio (alineado con objetivos del máster)
-   Tiempo estimado: 3-4 días de desarrollo
-   Permite evolucionar fácilmente a modelos más avanzados sin cambiar código base
-   Abstracción unificada permite cambiar de proveedor (Google → OpenAI → Azure) sin reescribir lógica

**Enfoque Híbrido Evolutivo (Alternativa conservadora):**

**Fase MVP (Entrega 2 - 21 Enero):**

-   Implementar **Opción A** (Patrones) para validar arquitectura rápidamente
-   Demostrar flujo E2E funcionando con 5-8 consultas
-   Sin riesgos de dependencias externas
-   Tiempo estimado: 2-3 días

**Fase Avanzada (Entrega 3 - 3 Febrero):**

-   Migrar a **Opción B** (Microsoft.Extensions.AI + Semantic Kernel + Gemini)
-   Documentar comparativa de ambos enfoques
-   Registrar prompts y ajustes realizados
-   Tiempo estimado: 3 días adicionales

**Justificación de Enfoque Directo:**
Con Microsoft.Extensions.AI + Semantic Kernel, la Opción B es ahora técnicamente viable desde el inicio porque:

1. SDK nativo de .NET elimina complejidad de integración
2. Microsoft.Extensions.AI proporciona abstracción unificada (cambiar proveedores sin reescribir código)
3. Tiempo de desarrollo reducido a 3-4 días (antes 4-5)
4. Demuestra uso profesional de IA desde el MVP
5. Mayor valor para un proyecto de máster en IA
6. Tecnología oficial de Microsoft (credibilidad técnica)

**Justificación de Enfoque Híbrido:**
Mantiene bajo riesgo en caso de:

1. Limitaciones inesperadas de Gemini (rate limits)
2. Complejidad de integración no anticipada
3. Restricciones de tiempo

**Nota:** La decisión final entre enfoque directo o híbrido se tomará tras validación con el tutor, considerando: objetivos del máster (favor a IA), tiempo disponible, y tolerancia al riesgo.

---

## 7. Plan de Entregas

### 7.1 Entrega 1: Documentación Técnica (17 de Diciembre)

-   README completo
-   Arquitectura y diseño
-   Modelo de datos (ERD)
-   Historias de usuario
-   API Spec (Swagger)
-   Configuración inicial del repositorio

### 7.2 Entrega 2: Código Funcional (21 de Enero)

-   Backend funcional con endpoints básicos
-   Frontend con UI de chat
-   Autenticación implementada
-   Conexión con API-Football
-   Base de datos conectada
-   3-5 tipos de consultas funcionando

### 7.3 Entrega 3: Entrega Final (3 de Febrero)

-   Sistema completo desplegado
-   Suite de tests (unitarios + E2E)
-   Pipeline CI/CD funcionando
-   Documentación de uso
-   Video demo (2-3 minutos)
-   Registro de uso de IA (prompts y ajustes)

---

## 8. Justificación de Limitaciones

### 8.1 Scope Temporal (2010-2025)

-   API-Football tiene datos desde 2010
-   15 años de datos es suficiente para casos de uso reales
-   Reduce complejidad de manejo de datos históricos inconsistentes

### 8.2 BYOK (Bring Your Own Key)

-   Evita costos de API en el lado del sistema
-   Permite a usuarios aprovechar planes premium
-   Cumple con restricciones de tiempo (30 horas)
-   Añade responsabilidad al usuario sobre su uso

### 8.3 Consultas Predefinidas en MVP

-   8-10 tipos de consultas cubren el 80% de casos de uso
-   Permite validar arquitectura antes de escalar
-   Reducción de complejidad del NLP inicial

---

## 9. Gestión de Riesgos

| Riesgo                  | Probabilidad | Impacto | Mitigación                                  |
| ----------------------- | ------------ | ------- | ------------------------------------------- |
| Límites de API agotados | Media        | Alto    | BYOK + Caché agresivo                       |
| Complejidad NLP         | Alta         | Medio   | Empezar con regex, evolucionar gradualmente |
| Tiempo insuficiente     | Media        | Alto    | Priorización clara Must/Should-Have         |
| Cambios en API-Football | Baja         | Alto    | Versionado de API + Tests de integración    |

---

## 10. Criterios de Éxito

1. **Funcional:** Usuario puede hacer 8-10 tipos de consultas y obtener respuestas precisas
2. **Técnico:** Tests con >70% cobertura, pipeline CI/CD funcional
3. **UX:** Interfaz intuitiva, respuestas en <3 segundos
4. **Despliegue:** Sistema accesible públicamente y estable
5. **Documentación:** README claro, código bien documentado, prompts de IA registrados

---

## 11. Referencias

### APIs y Datos

-   API-Football: https://www.api-football.com
-   Documentación OpenAPI: `/openapi.yaml`

### Tecnologías de IA

-   Microsoft.Extensions.AI: https://devblogs.microsoft.com/dotnet/introducing-microsoft-extensions-ai-preview/
-   Microsoft.Extensions.AI GitHub: https://github.com/dotnet/extensions/tree/main/src/Libraries/Microsoft.Extensions.AI
-   Microsoft Semantic Kernel: https://learn.microsoft.com/en-us/semantic-kernel/
-   Semantic Kernel GitHub: https://github.com/microsoft/semantic-kernel
-   Google Gemini API: https://ai.google.dev/
-   Semantic Kernel + Gemini Connector: https://github.com/microsoft/semantic-kernel/tree/main/dotnet/src/Connectors/Connectors.Google

### Proyecto

-   Repositorio: [URL del repositorio GitHub]

---

**Firma del Estudiante:** \***\*\*\*\*\***\_\_\_\***\*\*\*\*\***
**Fecha:** \***\*\*\*\*\***\_\_\_\***\*\*\*\*\***
