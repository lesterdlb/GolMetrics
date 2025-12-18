# Project Overview - GolMetrics

## 1. Visión del Producto

### 1.1. Problema a Resolver

Actualmente, acceder a estadísticas específicas de fútbol requiere:

-   Navegar por múltiples menús en sitios web especializados
-   Conocer la estructura exacta de datos (IDs de ligas, temporadas, etc.)
-   Alternar entre diferentes plataformas para datos históricos vs. actuales
-   Invertir tiempo considerable en búsquedas simples

**Ejemplo de fricción:**

> Pregunta: "Quiénes son los máximos goleadores de la Premier League esta temporada?"
>
> Proceso actual:
>
> 1. Abrir sitio web de estadísticas (ESPN, Transfermarkt, etc.)
> 2. Navegar: Ligas > Premier League > 2024-25 > Estadísticas > Goleadores
> 3. Esperar carga de página con anuncios
> 4. Localizar la tabla correcta entre múltiples widgets
>
> Tiempo estimado: 1-2 minutos

**Solución con GolMetrics:**

> Escribir: "Goleadores Premier 2024"
> Respuesta inmediata con tabla formateada:
>
> | #   | Jugador    | Equipo    | Goles |
> | --- | ---------- | --------- | ----- |
> | 1   | E. Haaland | Man City  | 15    |
> | 2   | M. Salah   | Liverpool | 12    |
> | 3   | C. Palmer  | Chelsea   | 11    |
>
> Tiempo estimado: 10 segundos

### 1.2. Propuesta de Valor

GolMetrics democratiza el acceso a estadísticas avanzadas de fútbol mediante:

1. **Interfaz conversacional:** Chat natural, sin necesidad de conocer estructuras técnicas
2. **Respuestas instantáneas:** Cache inteligente + IA reduce latencia
3. **Datos fiables:** API-Football como fuente única de verdad
4. **BYOK (Bring Your Own Key):** Control total del usuario sobre cuotas y costos
5. **Historial persistente:** Revisar consultas anteriores sin rehacer búsquedas

### 1.3. Público Objetivo

| Segmento                   | Necesidad Principal                             | Frecuencia de Uso       |
| -------------------------- | ----------------------------------------------- | ----------------------- |
| **Aficionados casuales**   | Consultas puntuales (goleadores, clasificación) | Baja (1-2 veces/semana) |
| **Periodistas deportivos** | Datos para artículos/análisis                   | Media (diaria)          |
| **Apostadores**            | Estadísticas para decisiones informadas         | Alta (varias veces/día) |
| **Analistas de datos**     | Extracción rápida de datasets específicos       | Media (semanal)         |

## 2. Alcance del MVP

### 2.1. Funcionalidades Core (Must-Have)

#### F1. Sistema de Autenticación

-   Registro con email/contraseña
-   Login/Logout con JWT
-   Recuperación de contraseña (opcional para MVP)

#### F2. Configuración de API Key

-   Campo seguro en perfil de usuario
-   Validación de API Key válida
-   Almacenamiento encriptado (AES-256)

#### F3. Interfaz de Chat

-   Input de texto natural
-   Visualización de mensajes (usuario + asistente)
-   Indicador de "typing" mientras la IA procesa
-   Renderizado de tablas y listas

#### F4. Consultas Soportadas (5 tipos Must-Have)

| Tipo de Consulta          | Ejemplo                              | Endpoint API-Football       |
| ------------------------- | ------------------------------------ | --------------------------- |
| 1. Máximos goleadores     | "Goleadores Premier 2024"            | `/players/topscorers`       |
| 2. Clasificación          | "Tabla de La Liga"                   | `/standings`                |
| 3. Resultados recientes   | "Últimos partidos del Madrid"        | `/fixtures?team=541&last=5` |
| 4. Próximos partidos      | "Próximo partido del Barcelona"      | `/fixtures?team=529&next=1` |
| 5. Estadísticas de equipo | "Stats del Liverpool esta temporada" | `/teams/statistics`         |

**Consultas Should-Have (post-MVP):**

-   Enfrentamientos directos (`/fixtures/headtohead`)
-   Información de jugador (`/players?search=`)
-   Máximos asistentes (`/players/topassists`)

#### F5. Sistema de Caché

-   Hash de parámetros de consulta como clave
-   Almacenamiento en PostgreSQL (tabla `cached_queries`)
-   TTL diferenciado:
    -   Datos históricos (2010-2023): 30 días
    -   Temporada actual (clasificaciones): 1 hora
    -   Partidos en vivo: 5 minutos

#### F6. Historial de Conversaciones

-   Persistencia automática de mensajes
-   Listado de conversaciones en sidebar
-   Carga de chat anterior al hacer clic
-   Crear nueva conversación

### 2.2. Funcionalidades Opcionales (Should-Have)

-   **Dashboard administrativo:** Métricas de uso (consultas/día, tipos más frecuentes)
-   **Exportación de datos:** Descargar resultados en CSV/JSON
-   **Sugerencias automáticas:** "También te puede interesar..." basado en contexto
-   **Soporte multi-idioma:** Inglés/Español para preguntas y respuestas

### 2.3. Fuera de Alcance (Out of Scope para MVP)

-   ❌ Apuestas integradas o recomendaciones de apuestas
-   ❌ Análisis predictivo con ML (ej: "¿Quién ganará el próximo partido?")
-   ❌ Datos en tiempo real de partidos en vivo (scores minute-by-minute)
-   ❌ Comparación de jugadores con visualizaciones avanzadas
-   ❌ Integración con redes sociales
-   ❌ Aplicación móvil nativa

## 3. Métricas de Éxito

### 3.1. Técnicas

-   **Disponibilidad:** >95% uptime
-   **Latencia p95:** <3 segundos para respuestas cacheadas, <5s para consultas nuevas
-   **Tasa de error:** <2% de respuestas incorrectas o fallidas
-   **Cobertura de tests:** >70%

### 3.2. Producto

-   **Precisión de respuestas:** >90% de consultas resueltas correctamente sin intervención
-   **Tipos de consultas soportadas:** 5 de 5 Must-Have funcionando en producción
-   **Tiempo medio de respuesta:** <4 segundos end-to-end

### 3.3. Usuario (Post-MVP)

-   **Retención semanal:** >30% de usuarios realizan al menos 2 consultas/semana
-   **Consultas por sesión:** Promedio de 3-5 consultas por sesión de chat
-   **NPS (Net Promoter Score):** >40 entre early adopters

## 4. Roadmap Post-MVP

### Fase 2 (Q2 2025)

-   Ampliar a 15-20 tipos de consultas
-   Soporte multi-idioma (inglés)
-   Dashboard de métricas para usuarios (mis estadísticas más consultadas)

### Fase 3 (Q3 2025)

-   Integración con más fuentes de datos (complementar API-Football)
-   Visualizaciones gráficas (charts, heatmaps)
-   Comparador de jugadores/equipos

### Fase 4 (Q4 2025)

-   Análisis predictivo básico (tendencias, no apuestas)
-   API pública para desarrolladores
-   Aplicación móvil (React Native)

## 5. Riesgos y Mitigaciones

| Riesgo                                | Probabilidad | Impacto | Mitigación                                           |
| ------------------------------------- | ------------ | ------- | ---------------------------------------------------- |
| Límites de API-Football agotados      | Media        | Alto    | BYOK + Cache agresivo (TTL largos)                   |
| IA no entiende consulta               | Alta         | Medio   | Fallback a sugerencias + mejora iterativa de prompts |
| Latencia alta en respuestas           | Media        | Alto    | Cache + CDN + optimización de queries                |
| Cambios en estructura de API-Football | Baja         | Alto    | Tests de integración + versionado de API             |
| Seguridad de API Keys                 | Media        | Crítico | Encriptación AES-256 + validación en cada request    |

## 6. Dependencias Externas

### 6.1. Críticas

-   **API-Football:** Fuente única de datos (plan gratuito: 100 req/día)
    -   Alternativa: SportsData.io (más limitada pero gratuita)
-   **Google Gemini API:** Modelo LLM para NLP
    -   Alternativa: OpenAI GPT-4 / Azure OpenAI / Anthropic Claude

### 6.2. Secundarias

-   **PostgreSQL:** Base de datos (puede sustituirse por MySQL/SQL Server)
-   **Docker Hub:** Registro de imágenes (puede sustituirse por GitHub Packages)

---

**Última actualización:** 2025-12-07
**Version:** 1.1 (MVP simplificado a 5 consultas)
