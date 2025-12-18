# Conversación 1: Validación y Arquitectura del Sistema

**Herramienta:** Claude Code (claude-sonnet-4.5)
**Duración:** ~3 horas (distribuidas en varias sesiones)
**Archivos creados:** 1 (PropuestaProyecto.md)
**Archivos modificados:** 2 (IdeaProyecto.md, DescripciónProyecto.md)
**Tokens utilizados:** ~45,000

---

## Resumen

Esta conversación documenta el proceso completo de validación y diseño arquitectónico del proyecto GolMetrics, desde la idea inicial hasta la definición técnica completa. Se validó la viabilidad del proyecto, se ajustó el scope temporal, se generó documentación formal, y se definió la estrategia de IA con Semantic Kernel + Google Gemini.

**Logros principales:**

-   ✅ Validación de viabilidad del proyecto
-   ✅ Definición de scope (2010-2025)
-   ✅ Propuesta técnica completa con 11 secciones
-   ✅ Decisión de arquitectura de IA (Semantic Kernel + Gemini)
-   ✅ Estrategia de consultas ilimitadas con función genérica universal

---

## Prompt 1: Análisis de Viabilidad del Proyecto

### Contexto

Primera validación de la idea del proyecto contra los requisitos del máster.

### Prompt

```
Necesito validar la viabilidad de mi proyecto final del máster. He adjuntado dos documentos: la descripción oficial del proyecto (@DescripciónProyecto.md) y mi propuesta inicial (@IdeaProyecto.md).

Analiza si mi propuesta cumple con los requisitos. Específicamente:
1. ¿Mi idea se ajusta a las expectativas del proyecto final?
2. ¿Cuál sería el flujo básico del sistema que propongo?
3. ¿Puedes acceder a la documentación de API-Football v3 para validar que existen los endpoints necesarios?

Esta etapa es solo análisis y validación. Nada de código aún.
```

### Resultado

-   ✅ La idea se ajustaba completamente a los requisitos del proyecto
-   Se identificó el flujo básico: Usuario → Frontend → Backend → IA/NLP → API-Football
-   Se detectó limitación de acceso a documentación de API-Football (solo scripts de tracking)
-   Se proporcionó información general sobre endpoints principales de API-Football

### Decisiones Tomadas

-   Validar que el proyecto cumple con todos los requisitos (documentación, tests, backend, frontend, infraestructura)
-   Necesidad de caché agresivo debido a límites de API gratuita (100 req/día)

---

## Prompt 2: Reducción de Scope Temporal

### Contexto

Evaluación del alcance reducido del proyecto (2010-2025 en lugar de datos históricos completos).

### Prompt

```
He refinado el alcance del proyecto basándome en tu feedback anterior. Los archivos @IdeaProyecto.md y @DescripciónProyecto.md están actualizados.

El cambio principal es limitar el scope temporal a 2010-2025 (solo fútbol moderno), en lugar de datos históricos completos. Necesito tu opinión sobre:
1. ¿Es este alcance más realista y factible para un proyecto de máster?
2. ¿Qué ventajas y limitaciones identificas con este scope reducido?

Por separado, descargué la especificación OpenAPI de API-Football (@openapi.yaml), pero tiene 20,625 líneas. ¿Qué estrategia recomiendas para extraer solo la información relevante y hacerla más manejable?
```

### Resultado

-   ✅ Scope reducido (2010-2025) es totalmente factible y más realista
-   Se identificaron ventajas del BYOK (Bring Your Own Key)
-   Se propusieron 3 opciones para compactar el openapi.yaml:
    1. Extraer solo endpoints relevantes (Recomendado)
    2. Usar script para extraer schema compacto
    3. Crear documentación resumida manual

### Decisiones Tomadas

-   Limitar scope temporal a 2010-2025 (alineado con cobertura de API-Football)
-   Implementar BYOK para evitar costos de API
-   Crear archivo `api-football-summary.md` con endpoints principales
-   Implementar rate limiting por usuario y caché agresivo

---

## Prompt 3: Propuesta

### Contexto

Necesidad de formalizar el proyecto para aprobación.

### Prompt

```
Necesito el siguiente documento:

Una propuesta técnica formal del proyecto, basada en @IdeaProyecto.md. Debe incluir: alcance, stack tecnológico, arquitectura, y plan de entregas.
```

### Resultado

-   Se generó `PropuestaProyecto.md` con 11 secciones completas

### Decisiones Tomadas

-   Estructura de propuesta formal con:
    -   Resumen ejecutivo
    -   Alcance MVP (Must-Have y Should-Have)
    -   Stack tecnológico completo
    -   Arquitectura con diagramas
    -   Modelo de datos
    -   Estrategia de IA (inicial: regex vs IA generativa)
    -   Plan de entregas
    -   Gestión de riesgos
    -   Criterios de éxito

---

## Prompt 4: Aclaración de Estrategia de IA

### Contexto

Confusión sobre cómo funciona la interpretación de lenguaje natural.

### Prompt

```
El punto 4 de la propuesta (@PropuestaProyecto.md) sobre interpretación de lenguaje natural me generó dudas. Ayúdame a entender:

1. ¿Por qué limitar a solo 8-10 tipos de consultas? ¿Es una restricción técnica o de scope?
2. ¿Podría un modelo de IA (como OpenAI o Gemini) interpretar cualquier pregunta en lenguaje natural y convertirla automáticamente en la llamada correcta a API-Football?
3. Si es posible, ¿cuál de estos modelos tiene capa gratuita viable para un MVP?

Este aspecto es crítico para el proyecto y necesito aclarar las opciones antes de decidir.
```

### Resultado

-   Se explicó diferencia entre Opción A (Regex/Patrones) y Opción B (IA Generativa)
-   Se comparó OpenAI vs Gemini para capas gratuitas
-   Se propuso estrategia híbrida: empezar con regex, evolucionar a IA

### Decisiones Tomadas

-   **Opción A (Regex):** 8-10 consultas predefinidas, sin costos, implementación rápida (2-3 días)
-   **Opción B (Gemini):** Function Calling, flexible, gratuito (1500 req/día), implementación más compleja (4-5 días)
-   **Estrategia recomendada:** Híbrida (MVP con regex, evolucionar a Gemini)

**Comparativa de proveedores:**

-   OpenAI: ❌ No tiene capa gratuita perpetua, requiere tarjeta de crédito
-   Gemini: ✅ Capa gratuita (15 req/min, 1500 req/día), sin tarjeta de crédito

---

## Prompt 5: Actualización de Propuesta con Enfoque Híbrido

### Contexto

Necesidad de clarificar ambas opciones en la propuesta.

### Prompt

```
Actualiza @PropuestaProyecto.md para reflejar la estrategia híbrida que
discutimos (Opción A: Regex vs Opción B: Gemini).
```

### Resultado

-   Se actualizó sección 6 de `PropuestaProyecto.md` con:
    -   Opción A detallada (8 ejemplos concretos de consultas)
    -   Opción B detallada (ejemplo de Function Calling)
    -   Estrategia Híbrida Recomendada

### Decisiones Tomadas

-   Mostrar pensamiento crítico sobre decisiones técnicas

---

## Prompt 6: Integración de Semantic Kernel

### Contexto

Investigación personal sobre herramientas de orquestación de IA.

### Prompt

```
Descubrí Semantic Kernel, un framework de Microsoft para orquestar modelos de IA como Gemini. Después de investigar, me parece que podría simplificar significativamente la implementación del punto 6 de @PropuestaProyecto.md.

¿Puedes evaluar si Semantic Kernel sería una buena opción para este proyecto?
Específicamente, ¿cómo impactaría en la complejidad de implementación y el tiempo de desarrollo?
```

### Resultado

-   Se confirmó que Semantic Kernel es ideal para el proyecto
-   Se explicó cómo simplifica la implementación de Function Calling
-   Se actualizó tiempo estimado: 3-4 días (reducción de 1 día)

### Decisiones Tomadas

-   **Integrar Semantic Kernel como orquestador de IA**
-   Ventajas identificadas:
    -   SDK nativo de .NET
    -   Function Calling simplificado mediante Plugins
    -   Multi-LLM (fácil cambiar entre modelos)
    -   Manejo de contexto conversacional incluido
    -   Planner automático
-   Nueva recomendación: **Ir directo con Opción B** podría ser viable desde MVP

---

## Prompt 7: Incorporar Semantic Kernel en Documentación

### Contexto

Actualizar propuesta con nueva arquitectura.

### Prompt

```
Perfecto. Procede a actualizar @PropuestaProyecto.md para integrar Semantic Kernel en la arquitectura propuesta.
```

### Resultado

-   Se actualizó sección 3.1 (Stack Tecnológico Backend)
-   Se expandió sección 6.2 (Opción B) con Semantic Kernel
-   Se reestructuró sección 6.3 (Estrategia Recomendada)
-   Se añadieron referencias a documentación de Semantic Kernel

**Cambios principales:**

```
Antes: "Opción B: IA Generativa (Google Gemini)"
Ahora: "Opción B: IA Generativa (Semantic Kernel + Google Gemini)"

Tiempo: 4-5 días → 3-4 días
Recomendación: Híbrida → Directo con Semantic Kernel (con híbrida como alternativa)
```

### Decisiones Tomadas

-   Presentar Semantic Kernel como ventaja técnica
-   Justificar enfoque directo con IA desde MVP
-   Mantener opción híbrida como alternativa conservadora

---

## Prompt 8: Explicación Detallada de Integración E2E

### Contexto

Necesidad de entender el flujo completo de la aplicación.

### Prompt

```
Necesito entender el flujo técnico completo end-to-end del sistema. Vamos a usar un ejemplo concreto: un usuario pregunta "¿Cuántos goles marcó el Real Madrid en 2023?"

Mi entendimiento del flujo es:
1. Frontend (Angular) envía pregunta → API (.NET)
2. API usa Semantic Kernel para enviar a Gemini [pregunta + funciones disponibles]
3. Gemini responde con... ¿qué exactamente? (Esta es mi duda principal)
4. API ejecuta la llamada correspondiente a API-Football
5. API envía [pregunta original + datos] a Gemini nuevamente
6. Gemini genera respuesta en lenguaje natural
7. API devuelve respuesta → Frontend → Usuario

Mis preguntas específicas:
1. ¿Este flujo es correcto o me falta/sobra algún paso?
2. ¿Qué estructura tiene la respuesta de Gemini en el paso 3? ¿Cómo la interpreto para saber qué endpoint de API-Football llamar?
3. ¿Qué otros componentes críticos faltan en este flujo (caché, autenticación, persistencia de conversaciones)?

Por ahora no modifiques archivos, solo explícame el flujo técnico.
```

### Resultado

-   Se explicó flujo completo paso a paso (11 pasos detallados)
-   Se clarificó que Semantic Kernel **automatiza** múltiples llamadas a Gemini
-   Se mostró estructura de Function Call de Gemini
-   Se explicaron aspectos adicionales: autenticación, almacenamiento, caché, rate limiting

**Flujo identificado:**

```
Usuario (Angular) → HTTP POST → .NET API → Semantic Kernel
                                              ↓
                        Gemini ← [Pregunta + Funciones disponibles]
                                              ↓
                        Gemini → functionCall(get_team_statistics, {params})
                                              ↓
                        SK ejecuta Plugin → API-Football
                                              ↓
                        API-Football → Datos
                                              ↓
                        SK → Gemini [Contexto + Resultado]
                                              ↓
                        Gemini → Respuesta en lenguaje natural
                                              ↓
                        .NET API → Angular → Usuario
```

### Decisiones Tomadas

-   Semantic Kernel maneja orquestación completa (no se necesitan llamadas manuales dobles a Gemini)
-   Implementar repositorios para conversaciones y mensajes
-   Usar API Key del usuario desde base de datos
-   Implementar caché con TTL variable
-   Usar Polly para rate limiting y retry con exponential backoff

---

## Prompt 9: Manejar Cualquier Tipo de Pregunta

### Contexto

Explorar posibilidad de no limitarse a 8-10 consultas predefinidas.

### Prompt

```
El flujo que explicaste tiene sentido, pero me quedé pensando en la limitación de 8-10 consultas predefinidas.

¿Sería técnicamente posible manejar cualquier tipo de pregunta sobre fútbol, no solo las predefinidas? Si es así, ¿qué arquitectura o estrategia recomendarías para lograrlo sin explotar la complejidad del sistema?
```

### Resultado

-   Se presentaron 4 estrategias diferentes:
    1. **Función Genérica "Universal"** (Recomendado)
    2. Dar a Gemini el Spec Completo de API-Football
    3. Auto-generación de Funciones desde OpenAPI
    4. Híbrido - Específicas + Fallback Genérico

**Estrategia 1 (Recomendada): Función Genérica Universal**

```csharp
// Solo 2-3 funciones en total:

1. search_entity(type, name)
   → Busca equipos/jugadores/ligas para obtener IDs

2. execute_api_football_query(endpoint, params)
   → Ejecuta cualquier llamada a API-Football

3. format_response(data, context) [opcional]
   → Ayuda a Gemini a formatear respuestas complejas
```

**Ventajas identificadas:**

-   ✅ Maneja el 100% de preguntas posibles
-   ✅ Solo ~200 líneas de código
-   ✅ Fácil de testear
-   ✅ Gemini decide qué endpoints usar
-   ✅ Documentación resumida en system prompt

### Decisiones Tomadas

-   **Recomendación final:** Empezar con Estrategia 1 (Función Genérica)
-   Proporcionar documentación resumida de API-Football en system prompt
-   Si casos comunes fallan, añadir funciones específicas (Estrategia 4 híbrida)
-   Trade-off aceptado: Menos control/validación a cambio de máxima flexibilidad

**Ejemplo de documentación resumida:**

```
ENDPOINTS PRINCIPALES:
- /teams?search=X → Buscar equipo
- /players/topscorers?league=X&season=Y → Goleadores
- /fixtures/headtohead?h2h=X-Y → Enfrentamientos directos
- /standings?league=X&season=Y → Clasificación

COMPETICIONES COMUNES:
- La Liga: 140, Premier League: 39, Champions: 2

EQUIPOS COMUNES:
- Real Madrid: 541, Barcelona: 529
```

---

## Decisiones Técnicas Finales

### Stack Tecnológico Definitivo

**Backend:**

-   .NET 10 Web API
-   PostgreSQL
-   Entity Framework Core
-   ASP.NET Core Identity / JWT
-   **Microsoft.Extensions.AI + Semantic Kernel + Google Gemini**
-   xUnit

**Frontend:**

-   React 18+
-   Material-UI / Tailwind CSS / shadcn/ui
-   Axios / fetch API
-   React Context / Zustand
-   Jest + React Testing Library

**Infraestructura:**

-   Docker + Docker Compose
-   GitHub Actions
-   Heroku / Railway / Render
-   API-Football v3

### Arquitectura de IA Seleccionada

**Enfoque Final:** Opción B (Microsoft.Extensions.AI + Semantic Kernel + Gemini) desde MVP

**Razones:**

1. Semantic Kernel reduce complejidad de implementación (3-4 días)
2. Demuestra uso real de IA desde el inicio (alineado con máster)
3. Microsoft.Extensions.AI proporciona abstracción agnóstica de proveedor
4. Gemini gratuito (1500 req/día) es suficiente para desarrollo
5. Mayor valor para proyecto de máster en IA

**Implementación:**

-   Función genérica universal (`execute_api_football_query`) para flexibilidad máxima
-   Documentación resumida de API-Football en system prompt
-   Opción de añadir funciones específicas para casos comunes si es necesario

### Funcionalidades MVP Final

**Must-Have:**

1. Sistema de autenticación (registro, login/logout)
2. Configuración de API Key personal (BYOK)
3. Interfaz de chat con NLP mediante Semantic Kernel + Gemini
4. Consultas ilimitadas (no restringidas a 8-10 tipos)
5. Historial de conversaciones
6. Sistema de caché en PostgreSQL

**Should-Have (Opcionales):**

1. Panel administrativo para métricas
2. Exportación de estadísticas (PDF/CSV)
3. Sugerencias de preguntas populares

---

## Lecciones Aprendidas

### Proceso de Diseño

1. **Validación iterativa:** Cada decisión técnica fue validada antes de continuar
2. **Investigación proactiva:** El descubrimiento de Semantic Kernel mejoró significativamente el diseño
3. **Flexibilidad:** El scope se ajustó para ser más realista (2010-2025)
4. **Pragmatismo:** Se priorizó solución viable sobre solución perfecta

### Decisiones de IA

1. **Semantic Kernel es game-changer:** Simplifica drásticamente la integración de LLMs
2. **Gemini gratuito es suficiente:** 1500 req/día cubre desarrollo y pruebas
3. **Function Calling > Regex:** Más flexible y escalable para NLP
4. **Función genérica > Funciones específicas:** Máxima flexibilidad con mínimo código

### Gestión de Riesgos

1. **BYOK mitiga costos:** Usuarios aportan su propia API Key
2. **Caché agresivo:** Reduce dependencia de límites de API
3. **Estrategia híbrida disponible:** Fallback a regex si IA falla
4. **Documentación resumida:** Evita prompts de 20,625 líneas

---

## Referencias Clave

### Documentación Técnica

-   Microsoft.Extensions.AI: https://devblogs.microsoft.com/dotnet/introducing-microsoft-extensions-ai-preview/
-   Semantic Kernel: https://learn.microsoft.com/en-us/semantic-kernel/
-   Google Gemini API: https://ai.google.dev/
-   API-Football: https://www.api-football.com

### Archivos del Proyecto

-   Propuesta: `PropuestaProyecto.md`
-   Descripción del proyecto: `DescripciónProyecto.md`
-   Idea original: `IdeaProyecto.md`

---

**Última actualización:** 14 de Diciembre de 2025
