# Conversaciones de Diseño y Desarrollo - GolMetrics

Este directorio contiene el registro completo de todas las conversaciones con asistentes de IA durante el diseño, planificación y desarrollo del proyecto GolMetrics.

---

## Índice de Conversaciones

### [01 - Validación y Arquitectura](./01-validacion-arquitectura.md)

**Fecha:** 2025-10-27 a 2025-12-14
**Duración:** ~3 horas
**Prompts:** 9

**Temas principales:**

-   Validación de viabilidad del proyecto
-   Definición de scope temporal (2010-2025)
-   Generación de propuesta técnica formal
-   Descubrimiento de Semantic Kernel
-   Decisión de arquitectura de IA (Microsoft.Extensions.AI + Semantic Kernel + Gemini)
-   Estrategia de consultas ilimitadas con función genérica universal

**Archivos creados:** 1 (PropuestaProyecto.md)
**Archivos modificados:** 2 (IdeaProyecto.md, DescripciónProyecto.md)

---

### [02 - Stack Tecnológico y Diseño Visual](./02-stack-y-diseno.md)

**Fecha:** 2025-12-14
**Duración:** ~2 horas
**Prompts:** 8

**Temas principales:**

-   Actualización de stack (Angular → React)
-   Integración de Microsoft.Extensions.AI
-   Exploración de naming del chatbot (3 opciones)
-   Diseño de 3 propuestas visuales diferenciadas
-   Generación de prompts de diseño bilingües
-   Creación de 13 prompts para generación de imágenes con IA

**Archivos creados:** 2 (visual-design-prompts.md, image-generation-prompts.md)
**Archivos modificados:** 2 (CLAUDE.md, PropuestaProyecto.md)

---

### [03 - Documentación y Metodología](./03-documentacion-metodologia.md)

**Fecha:** 2025-12-14
**Duración:** ~1.5 horas
**Prompts:** 4
**Herramientas MCP:** Context7 (OpenSpec)

**Temas principales:**

-   Creación de estructura de documentación técnica (`docs/`)
-   Justificación de Arquitectura Vertical Híbrida
-   Descubrimiento de OpenSpec como metodología de desarrollo
-   Formato mejorado para documentación de prompts

**Archivos creados:** 9 (estructura completa de docs/)
**Archivos modificados:** 1 (Entregables/README.md)

---

### [04 - Limitaciones de API-Football](./04-limitaciones-api.md)

**Fecha:** 2025-12-14
**Duración:** ~45 minutos
**Prompts:** 4
**Herramientas MCP:** Context7 (API-Football, Sportmonks, FootyStats)

**Temas principales:**

-   Análisis de limitaciones técnicas de API-Football
-   Investigación de planes de API (Free, Pro, Ultra)
-   Comparativa con alternativas del mercado
-   Corrección de precio del plan Ultra ($149→$29/mes)
-   Validación de enfoque MVP con consultas simples
-   Documentación técnica de limitaciones

**Archivos creados:** 1 (docs/02-architecture/APILimitations.md)
**Archivos modificados:** 1 (docs/01-product/ProjectOverview.md)

---

## Estadísticas Generales

### Por Herramienta

| Herramienta                         | Conversaciones | Prompts Totales | Tokens Estimados |
| ----------------------------------- | -------------- | --------------- | ---------------- |
| **Claude Code (claude-sonnet-4.5)** | 4              | 25              | ~126,000         |

### Por Fase del Proyecto

| Fase                      | Conversaciones | Prompts | Archivos Creados | Archivos Modificados |
| ------------------------- | -------------- | ------- | ---------------- | -------------------- |
| **Análisis y Producto**   | 1              | 9       | 1                | 2                    |
| **Arquitectura y Diseño** | 1              | 8       | 2                | 2                    |
| **Documentación**         | 1              | 4       | 9                | 1                    |
| **Investigación Técnica** | 1              | 4       | 1                | 1                    |
| **TOTAL**                 | **4**          | **25**  | **13**           | **6**                |

### Herramientas MCP Utilizadas

| Herramienta MCP | Conversaciones | Uso                                                             |
| --------------- | -------------- | --------------------------------------------------------------- |
| **Context7**    | 2              | Investigación de OpenSpec, API-Football, Sportmonks, FootyStats |

### Distribución de Prompts por Tipo

| Tipo de Tarea                   | Prompts | Porcentaje |
| ------------------------------- | ------- | ---------- |
| **Decisiones arquitectónicas**  | 7       | 28.0%      |
| **Diseño visual**               | 6       | 24.0%      |
| **Generación de documentación** | 5       | 20.0%      |
| **Análisis y validación**       | 4       | 16.0%      |
| **Investigación técnica**       | 3       | 12.0%      |

---

## Decisiones Técnicas Principales

### Stack Tecnológico Final

**Backend:**

-   .NET 9 Web API
-   PostgreSQL
-   Entity Framework Core
-   ASP.NET Core Identity / JWT
-   Microsoft.Extensions.AI + Semantic Kernel + Google Gemini
-   MediatR, FluentValidation, Mapperly, Serilog
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

### Arquitectura

**Estilo:** Vertical Slice Architecture Híbrida (con elementos de Clean Architecture)

**Características:**

-   Cada feature en un único archivo .cs
-   Cohesión alta, acoplamiento bajo
-   MediatR para CQRS
-   Mapperly para mapping (source generator)

### Estrategia de IA

**Enfoque:** Microsoft.Extensions.AI + Semantic Kernel + Google Gemini

**Ventajas:**

-   Abstracción agnóstica de proveedor
-   Function Calling simplificado
-   Consultas ilimitadas con función genérica universal
-   Gratuito (1500 req/día de Gemini)

### Modelo de Negocio

**BYOK (Bring Your Own Key):**

-   Usuarios aportan su propia API Key de API-Football
-   Plan Free: 100 req/día (suficiente para MVP)
-   Plan Ultra: $29/mes para 75,000 req/día (si necesitan escalar)

---

## Metodologías Adoptadas

### OpenSpec

**Adoptado para:** Entrega 2 (desarrollo de código)

**Workflow:**

1. `/docs/WorkTickets.md` - Define QUÉ tickets hacer
2. `/openspec/changes/tick-xxx/` - Guía CÓMO implementar
3. Código - Implementación guiada por spec
4. `/PROMPTS_LOGS/` - Registro de uso de IA

### Documentación Viva

**Estructura de docs/:**

-   `01-product/` - Visión y requisitos
-   `02-architecture/` - Decisiones técnicas
-   `03-api/` - Especificación de endpoints
-   `04-frontend/` - Integración y diseño
-   `05-operations/` - Deploy, testing, tickets

---

## Métricas de Productividad

### Tiempo Ahorrado (Estimado)

| Actividad                         | Tiempo Manual   | Tiempo con IA | Ahorro          |
| --------------------------------- | --------------- | ------------- | --------------- |
| Investigación de APIs             | 4-5 horas       | 30 minutos    | ~4 horas        |
| Generación de propuesta           | 3-4 horas       | 1 hora        | ~3 horas        |
| Diseño de 3 propuestas visuales   | 6-8 horas       | 2 horas       | ~6 horas        |
| Creación de estructura de docs    | 2-3 horas       | 30 minutos    | ~2.5 horas      |
| Prompts de generación de imágenes | 2-3 horas       | 45 minutos    | ~2 horas        |
| **TOTAL**                         | **17-23 horas** | **~5 horas**  | **~17.5 horas** |

**Ahorro de tiempo:** ~78% (17.5h de 22.5h promedio)

### Calidad de Outputs

| Tipo de Output                  | Ajustes Requeridos | Calificación |
| ------------------------------- | ------------------ | ------------ |
| Análisis técnico                | Mínimos (5-10%)    | ⭐⭐⭐⭐⭐   |
| Generación de código de ejemplo | Menores (10-20%)   | ⭐⭐⭐⭐     |
| Documentación técnica           | Mínimos (5-15%)    | ⭐⭐⭐⭐⭐   |
| Propuestas de diseño            | Sin ajustes (0%)   | ⭐⭐⭐⭐⭐   |
| Investigación con Context7      | Sin ajustes (0%)   | ⭐⭐⭐⭐⭐   |

---

## Lecciones Aprendidas

### Sobre Uso de IA

1. **Context7 es invaluable:** Acceso a documentación actualizada de APIs y herramientas emergentes ahorra horas de investigación manual

2. **Prompts iterativos son mejores:** Conversaciones largas con refinamiento progresivo generan mejores resultados que prompts únicos complejos

3. **Especificar formato ayuda:** Pedir formato específico (tablas markdown, estructura de archivo, paletas de colores en HEX) reduce ajustes posteriores

4. **Versiones originales + mejoradas:** Mantener prompts originales del usuario + versión mejorada por IA documenta el proceso de aprendizaje

### Sobre Arquitectura

1. **Validar antes de ejemplos:** El ejemplo "Haaland vs Mbappé en partidos away" parecía simple pero requería 42 API calls

2. **Investigar alternativas siempre:** La comparativa de 3 APIs demostró que API-Football es la mejor opción, no por ser la única sino por balance

3. **Semantic Kernel es game-changer:** Simplificó la arquitectura de IA drásticamente y redujo tiempo de implementación en 1-2 días

4. **Vertical Slice > Clean Architecture pura:** Para proyectos pequeños/medianos, ofrece mejor productividad y mantenibilidad

### Sobre Documentación

1. **Estructura jerárquica por categorías:** Organizar docs/ en 01-product, 02-architecture, etc. facilita navegación

2. **Plantillas aceleran trabajo:** Crear estructuras con placeholders permite llenar posteriormente sin perder el ritmo

3. **Tracking de prompts importa:** Documentar uso de IA demuestra valor y justifica adopción de herramientas

### Sobre Diseño

1. **3 propuestas > 1:** Ofrecer opciones diferenciadas (Pizarra Táctica, Estadio de Noche, Cromos) permite mejor decisión

2. **Prompts bilingües útiles:** Tener español + inglés permite usar herramientas de IA en ambos idiomas

3. **Múltiples versiones de prompts:** Short/Full/Midjourney/Detailed permite iteración progresiva en generación de imágenes

---

## Próximos Pasos

### Para Entrega 1 (17 de Diciembre)

-   [ ] Completar historias de usuario formalizadas
-   [ ] Generar tickets de trabajo detallados
-   [ ] Documentar endpoints en formato OpenAPI
-   [ ] Crear diagrama ERD en mermaid
-   [ ] Especificar prácticas de seguridad
-   [ ] Decidir nombre final del chatbot
-   [ ] Generar mockups visuales

### Para Entrega 2 (21 de Enero)

-   [ ] Inicializar `/openspec` en raíz del proyecto
-   [ ] Implementar 5 consultas Must-Have
-   [ ] Desarrollar backend con Semantic Kernel + Gemini
-   [ ] Crear frontend con React + UI seleccionada
-   [ ] Implementar autenticación y caché

### Para Entrega 3 (3 de Febrero)

-   [ ] Completar suite de tests (>70% cobertura)
-   [ ] Configurar pipeline CI/CD
-   [ ] Desplegar sistema completo
-   [ ] Generar video demo
-   [ ] Finalizar estadísticas de uso de IA

---

## Archivos Relacionados

### Documentación del Proyecto

-   `/docs/` - Documentación técnica completa
-   `/Entregables/README.md` - Resumen ejecutivo para entrega
-   `/Entregables/prompts.md` - Top 3 prompts por sección (extracto)

### Prompts y Diseño

-   `/PROMPTS_LOGS/` - Registro detallado de todos los prompts utilizados
-   `visual-design-prompts.md` - Prompts de diseño conceptual (español + inglés)
-   `image-generation-prompts.md` - 13 prompts para generación de imágenes

### Propuesta Técnica

-   `PropuestaProyecto.md` - Propuesta formal con 11 secciones
-   `CLAUDE.md` - Instrucciones del proyecto para Claude Code

---

## Contacto y Créditos

**Estudiante:** Lester David López Bustillo
**Proyecto:** GolMetrics - Chatbot de Estadísticas de Fútbol
**Programa:** AI4Devs - Proyecto Final
**Herramientas IA:** Claude Code (claude-sonnet-4.5), Context7 MCP Server

**Período de diseño:** 27 de Octubre - 14 de Diciembre 2025
**Total de horas con IA:** ~7.25 horas
**Total de horas ahorradas:** ~17.5 horas
**Eficiencia:** 78% de reducción de tiempo

---

**Última actualización:** 16 de Diciembre de 2025
