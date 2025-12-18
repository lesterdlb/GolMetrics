> Detalla en esta sección los prompts principales utilizados durante la creación del proyecto, que justifiquen el uso de asistentes de código en todas las fases del ciclo de vida del desarrollo. Esperamos un máximo de 3 por sección, principalmente los de creación inicial o los de corrección o adición de funcionalidades que consideres más relevantes.
> Puedes añadir adicionalmente la conversación completa como link o archivo adjunto si así lo consideras

## Índice

1. [Descripción general del producto](#1-descripción-general-del-producto)
2. [Arquitectura del sistema](#2-arquitectura-del-sistema)
    - [2.1. Diagrama de arquitectura](#21-diagrama-de-arquitectura)
    - [2.2. Descripción de componentes principales](#22-descripción-de-componentes-principales)
    - [2.3. Descripción de alto nivel del proyecto y estructura de ficheros](#23-descripción-de-alto-nivel-del-proyecto-y-estructura-de-ficheros)
    - [2.4. Infraestructura y despliegue](#24-infraestructura-y-despliegue)
    - [2.5. Seguridad](#25-seguridad)
    - [2.6. Tests](#26-tests)
3. [Modelo de datos](#3-modelo-de-datos)
4. [Especificación de la API](#4-especificación-de-la-api)
5. [Historias de usuario](#5-historias-de-usuario)
6. [Tickets de trabajo](#6-tickets-de-trabajo)
7. [Pull requests](#7-pull-requests)

---

## 1. Descripción general del producto

**Prompt 1: Análisis de Viabilidad del Proyecto**

**Contexto:**
Primera validación de la idea del proyecto contra los requisitos del máster AI4Devs para confirmar viabilidad.

**Prompt original:**

```
Necesito validar la viabilidad de mi proyecto final del máster. He adjuntado dos documentos: la descripción oficial del proyecto (@DescripciónProyecto.md) y mi propuesta inicial (@IdeaProyecto.md).

Analiza si mi propuesta cumple con los requisitos. Específicamente:
1. ¿Mi idea se ajusta a las expectativas del proyecto final?
2. ¿Cuál sería el flujo básico del sistema que propongo?
3. ¿Puedes acceder a la documentación de API-Football v3 para validar que existen los endpoints necesarios?

Esta etapa es solo análisis y validación. Nada de código aún.
```

**Resultado:**
La idea se ajustó completamente a los requisitos del proyecto final. Se identificó el flujo básico (Usuario → Frontend → Backend → IA/NLP → API-Football → Cache → Respuesta). Se detectó limitación de acceso a documentación completa de API-Football.

**Decisiones tomadas:**

-   Validación confirmada de que el proyecto cumple con todos los requisitos
-   Necesidad de caché agresivo debido a límites de API gratuita (100 req/día)
-   Identificación temprana de BYOK (Bring Your Own Key) como estrategia

---

**Prompt 2: Reducción de Scope Temporal**

**Contexto:**
Evaluación del alcance reducido del proyecto (2010-2025 vs datos históricos completos) para asegurar realismo.

**Prompt original:**

```
He refinado el alcance del proyecto basándome en tu feedback anterior. Los archivos @IdeaProyecto.md y @DescripciónProyecto.md están actualizados.

El cambio principal es limitar el scope temporal a 2010-2025 (solo fútbol moderno), en lugar de datos históricos completos. Necesito tu opinión sobre:
1. ¿Es este alcance más realista y factible para un proyecto de máster?
2. ¿Qué ventajas y limitaciones identificas con este scope reducido?

Por separado, descargué la especificación OpenAPI de API-Football (@openapi.yaml), pero tiene 20,625 líneas. ¿Qué estrategia recomiendas para extraer solo la información relevante y hacerla más manejable?
```

**Resultado:**
Scope reducido (2010-2025) validado como totalmente factible y realista. Se identificaron ventajas del BYOK y se propusieron 3 opciones para compactar openapi.yaml (extraer endpoints relevantes, script de extracción, documentación manual).

**Decisiones tomadas:**

-   Limitar scope temporal a 2010-2025 (alineado con cobertura de API-Football)
-   Implementar BYOK para evitar costos de API
-   Crear documentación resumida de API-Football con endpoints principales
-   Implementar rate limiting por usuario y caché agresivo

---

**Prompt 3: Generación de Propuesta Técnica Formal**

**Contexto:**
Necesidad de formalizar el proyecto en un documento estructurado para aprobación académica.

**Prompt original:**

```
Necesito el siguiente documento:

Una propuesta técnica formal del proyecto, basada en @IdeaProyecto.md. Debe incluir: alcance, stack tecnológico, arquitectura, y plan de entregas.
```

**Resultado:**
Se generó `PropuestaProyecto.md` con 11 secciones completas: Resumen ejecutivo, Alcance MVP (Must-Have y Should-Have), Stack tecnológico completo, Arquitectura con diagramas, Modelo de datos, Estrategia de IA (inicial: regex vs IA generativa), Plan de entregas, Gestión de riesgos, Criterios de éxito.

**Decisiones tomadas:**

-   Estructura formal con Must-Have (3-5 historias requeridas) y Should-Have (1-2 opcionales)
-   Stack tecnológico: .NET 9, React, PostgreSQL, Semantic Kernel + Gemini
-   Estrategia de IA inicial con dos opciones: Regex/Patrones vs IA Generativa
-   Plan de 3 entregas (17 dic, 21 ene, 3 feb)

---

## 2. Arquitectura del Sistema

### 2.1. Diagrama de arquitectura

**Prompt 1: Integración de Semantic Kernel**

**Contexto:**
Investigación personal sobre herramientas de orquestación de IA para simplificar implementación.

**Prompt original:**

```
Descubrí Semantic Kernel, un framework de Microsoft para orquestar modelos de IA como Gemini. Después de investigar, me parece que podría simplificar significativamente la implementación del punto 6 de @PropuestaProyecto.md.

¿Puedes evaluar si Semantic Kernel sería una buena opción para este proyecto?
Específicamente, ¿cómo impactaría en la complejidad de implementación y el tiempo de desarrollo?
```

**Resultado:**
Se confirmó que Semantic Kernel es ideal para el proyecto. Se explicó cómo simplifica la implementación de Function Calling mediante Plugins nativos de .NET. Se actualizó tiempo estimado de 4-5 días a 3-4 días.

**Decisiones tomadas:**

-   Integrar Semantic Kernel como orquestador de IA
-   Ventajas: SDK nativo de .NET, Function Calling simplificado, Multi-LLM, Memoria conversacional incluida
-   Nueva recomendación: Ir directo con Opción B (IA Generativa) desde MVP en lugar de enfoque híbrido
-   Reducción de complejidad y tiempo de implementación

---

**Prompt 2: Explicación Detallada de Integración E2E**

**Contexto:**
Necesidad de entender el flujo técnico completo del sistema para validar arquitectura.

**Prompt original:**

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

**Resultado:**
Se explicó flujo completo de 11 pasos detallados. Se clarificó que Semantic Kernel automatiza múltiples llamadas a Gemini (no se necesitan llamadas manuales dobles). Se mostró estructura de Function Call de Gemini. Se explicaron aspectos adicionales: autenticación, almacenamiento, caché, rate limiting.

**Decisiones tomadas:**

-   Semantic Kernel maneja orquestación completa (no se necesitan llamadas manuales dobles a Gemini)
-   Implementar repositorios para conversaciones y mensajes
-   Usar API Key del usuario desde base de datos (BYOK)
-   Implementar caché con TTL variable (30 días para históricos, 1 hora para actuales)
-   Usar Polly para rate limiting y retry con exponential backoff

---

**Prompt 3: Estrategia de Función Genérica Universal**

**Contexto:**
Explorar posibilidad de no limitarse a 8-10 consultas predefinidas y manejar cualquier tipo de pregunta.

**Prompt original:**

```
El flujo que explicaste tiene sentido, pero me quedé pensando en la limitación de 8-10 consultas predefinidas.

¿Sería técnicamente posible manejar cualquier tipo de pregunta sobre fútbol, no solo las predefinidas? Si es así, ¿qué arquitectura o estrategia recomendarías para lograrlo sin explotar la complejidad del sistema?
```

**Resultado:**
Se presentaron 4 estrategias diferentes: 1) Función Genérica "Universal" (Recomendado), 2) Dar a Gemini el Spec Completo de API-Football, 3) Auto-generación de Funciones desde OpenAPI, 4) Híbrido - Específicas + Fallback Genérico. Se recomendó Estrategia 1: solo 2-3 funciones en total (`search_entity`, `execute_api_football_query`, `format_response`).

**Decisiones tomadas:**

-   Implementar función genérica universal `execute_api_football_query` para máxima flexibilidad
-   Proporcionar documentación resumida de API-Football en system prompt (evitar 20,625 líneas)
-   Ventajas: Maneja el 100% de preguntas posibles, solo ~200 líneas de código, fácil de testear
-   Si casos comunes fallan, añadir funciones específicas (enfoque híbrido)
-   Trade-off aceptado: Menos control/validación a cambio de máxima flexibilidad

---

### 2.2. Descripción de componentes principales

**Prompt 1: Aclaración de Estrategia de IA**

**Contexto:**
Confusión sobre cómo funciona la interpretación de lenguaje natural y si limitarse a 8-10 consultas era restricción técnica o de scope.

**Prompt original:**

```
El punto 4 de la propuesta (@PropuestaProyecto.md) sobre interpretación de lenguaje natural me generó dudas. Ayúdame a entender:

1. ¿Por qué limitar a solo 8-10 tipos de consultas? ¿Es una restricción técnica o de scope?
2. ¿Podría un modelo de IA (como OpenAI o Gemini) interpretar cualquier pregunta en lenguaje natural y convertirla automáticamente en la llamada correcta a API-Football?
3. Si es posible, ¿cuál de estos modelos tiene capa gratuita viable para un MVP?

Este aspecto es crítico para el proyecto y necesito aclarar las opciones antes de decidir.
```

**Resultado:**
Se explicó diferencia entre Opción A (Regex/Patrones - 8-10 consultas) y Opción B (IA Generativa - consultas ilimitadas). Se comparó OpenAI vs Gemini para capas gratuitas. Se propuso estrategia híbrida: empezar con regex, evolucionar a IA.

**Decisiones tomadas:**

-   Opción A (Regex): 8-10 consultas predefinidas, sin costos, implementación rápida (2-3 días)
-   Opción B (Gemini): Function Calling, flexible, gratuito (1500 req/día), implementación más compleja (4-5 días)
-   Estrategia recomendada: Híbrida (MVP con regex, evolucionar a Gemini)
-   Comparativa de proveedores: OpenAI no tiene capa gratuita perpetua, Gemini sí (15 req/min, 1500 req/día, sin tarjeta de crédito)

---

**Prompt 2: Incorporar Semantic Kernel en Documentación**

**Contexto:**
Actualizar propuesta técnica con nueva arquitectura que incluye Semantic Kernel.

**Prompt original:**

```
Perfecto. Procede a actualizar @PropuestaProyecto.md para integrar Semantic Kernel en la arquitectura propuesta.
```

**Resultado:**
Se actualizó sección 3.1 (Stack Tecnológico Backend) con Semantic Kernel. Se expandió sección 6.2 (Opción B) con ejemplos de Plugins. Se reestructuró sección 6.3 (Estrategia Recomendada) mostrando ventajas de SK. Se añadieron referencias a documentación oficial de Semantic Kernel y Gemini Connector.

**Decisiones tomadas:**

-   Cambio de arquitectura: "Opción B: IA Generativa (Google Gemini)" → "Opción B: IA Generativa (Semantic Kernel + Google Gemini)"
-   Tiempo estimado reducido: 4-5 días → 3-4 días
-   Recomendación actualizada: Directo con Semantic Kernel desde MVP (en lugar de híbrida)
-   Presentar SK como ventaja técnica diferenciadora

---

**Prompt 3: Cambio de Stack Tecnológico**

**Contexto:**
Decisión de cambiar frontend de Angular a React e integrar Microsoft.Extensions.AI como capa de abstracción.

**Prompt original:**

```
He tomado dos decisiones técnicas importantes que necesito reflejar en la documentación:

1. Frontend: Cambiaré de Angular a React 18+. Por favor, actualiza todos los documentos relevantes (CLAUDE.md, PropuestaProyecto.md) para reflejar este cambio, incluyendo el stack de librerías asociadas (UI library, state management, testing).

2. IA/NLP: Utilizaré Microsoft.Extensions.AI junto con Semantic Kernel para la comunicación con LLMs. Actualiza la arquitectura para reflejar esta capa de abstracción adicional y sus ventajas.
```

**Resultado:**
Se actualizaron CLAUDE.md y PropuestaProyecto.md con: Frontend cambiado a React 18+ (MUI/Tailwind CSS, Axios/fetch, React Context/Zustand, Jest + React Testing Library). IA/NLP actualizado a Microsoft.Extensions.AI + Semantic Kernel + Gemini. Se explicó ventaja principal: abstracción agnóstica del proveedor.

**Decisiones tomadas:**

-   Frontend: Angular → React 18+ (mayor ecosistema, mejor para chatbot UI, más conocido)
-   State Management: React Context / Zustand (simplicidad para MVP)
-   Testing Frontend: Jasmine + Karma → Jest + React Testing Library
-   IA/NLP: Añadido Microsoft.Extensions.AI como capa de abstracción
-   Ventaja crítica: Abstracción agnóstica del proveedor (cambiar entre OpenAI, Azure, Google sin reescribir código)

---

### 2.3. Descripción de alto nivel del proyecto y estructura de ficheros

**Prompt 1: Consulta sobre Organización del Proyecto**

**Contexto:**
Evaluar la estructura actual del proyecto y proponer mejoras organizativas para documentación técnica.

**Prompt original:**

```
Estoy organizando la estructura de documentación del proyecto y necesito tu feedback sobre dos aspectos:

1. **README de entrega (@Entregables/README.md)**: Tengo un primer borrador, pero creo que debería ser más conciso. ¿Está bien como resumen o necesita ajustes?

2. **Documentación técnica detallada**: Propongo crear un directorio `docs/` con documentación oficial más profunda. Los documentos que tengo en mente son:
- ProjectOverview.md
- Architecture.md
- DataModel.md
- ApiSpec.md
- FrontendIntegration.md
- UserStories.md
- WorkTickets.md
- DeploymentGuide.md
- Testing.md.

¿Esta estructura tiene sentido? ¿Recomendarías otra organización?

Dame tu opinión profesional sobre estos dos puntos.
```

**Resultado:**
README de entrega validado como buen resumen ejecutivo. Se propuso estructura refinada de docs/ organizada en 5 categorías (01-product, 02-architecture, 03-api, 04-frontend, 05-operations) con README.md como índice principal.

**Decisiones tomadas:**

-   Mantener README.md conciso como resumen ejecutivo
-   Crear estructura de docs/ por categorías temáticas
-   Separar documentación técnica (docs/) de entregables académicos (Entregables/)
-   Incluir README.md en docs/ como índice de navegación

---

**Prompt 2: Solicitud de Implementación con Especificaciones Arquitectónicas**

**Contexto:**
Implementar mejoras propuestas con detalles específicos de arquitectura Vertical Slice Híbrida.

**Prompt original:**

```
Perfecto, procede con la implementación pero tengo algunas especificaciones técnicas importantes:

1. **Justificación del stack en README.md**: La arquitectura será Vertical Slice con elementos de Clean Architecture. La estructura del backend será:
   - `/Core` (subcarpetas: Application y Domain)
   - `/Extensions`
   - `/Features` (vertical slices)
   - `/Infrastructure`
   - `/Middlewares`
   - `DependencyInjection.cs`

Librerías: MediatR, FluentValidation, Microsoft.AspNetCore.Identity, Bogus, Serilog

Pregunta: ¿Qué alternativa a AutoMapper recomiendas? Estoy evaluando opciones.

2. **Generar estructura de docs/**: Crea la estructura con plantillas iniciales en cada archivo para facilitar el llenado posterior.

¿Necesitas aclaraciones adicionales antes de proceder?
```

**Resultado:**
Claude hizo preguntas de aclaración sobre estilo arquitectónico exacto (Clean vs Vertical Slice vs Híbrida), contenido de /Features, alternativa a AutoMapper (recomendó Mapperly), y estructura de cada feature.

**Decisiones tomadas:**

-   Esperar aclaraciones antes de proceder (buena práctica de ingeniería)
-   Maperly recomendado como alternativa a AutoMapper (source generator, rendimiento superior)
-   Validación de entendimiento antes de implementar
-   Preguntas específicas sobre organización de Features

---

**Prompt 3: Aclaraciones Arquitectónicas Finales**

**Contexto:**
Responder a preguntas de aclaración de Claude sobre la arquitectura Vertical Slice Híbrida.

**Prompt original:**

```
Respondo a tus preguntas de aclaración:

1. **Arquitectura**: Opción C - Híbrida. Vertical Slice Architecture como base, pero con elementos de Clean Architecture (Core/Application/Domain) para evitar un folder "common" genérico. Los elementos compartidos vivirán en Core con estructura clara.

2. **Contenido de /Features**: Vertical slices completos. Cada feature contendrá Command/Query/Handler/Validator en un único archivo .cs para mantener la cohesión. Ejemplo: `Features/Chat/SendMessage.cs` incluiría: endpoint (Minimal API), command, handler, DTO, y validator.

3. **Mapper**: Opción A - Mapperly (source generator). Me gusta su rendimiento y verificación en tiempo de compilación.

4. **Estructura de features**: Como mencioné en punto 2, todo en un archivo por slice. Formato: `Features/{FeatureName}/{ActionName}.cs`

Procede con la implementación.
```

**Resultado:**
Se completó justificación del stack en README.md con tabla de tecnologías, ventajas de Arquitectura Vertical Híbrida, estructura del backend, decisiones técnicas (Mapperly, MediatR, PostgreSQL para caché). Se generó estructura completa de docs/ con plantillas en cada archivo.

**Decisiones tomadas:**

-   Arquitectura Vertical Slice Híbrida (cohesión alta, acoplamiento bajo)
-   Cada feature en un único archivo .cs (Command/Query/Handler/Validator/Endpoint)
-   Mapperly como mapper (source generator, verificación en compilación)
-   MediatR para CQRS pattern
-   Estructura de Features: `Features/{FeatureName}/{ActionName}.cs`

---

### 2.4. Infraestructura y despliegue

**Prompt 1: Consulta sobre Metodología OpenSpec**

**Contexto:**
Explorar la integración de OpenSpec como metodología de desarrollo para mejorar precisión de modelos de IA.

**Prompt original:**

```
He descubierto OpenSpec, una metodología de trabajo diseñada para mejorar la precisión de los modelos de IA durante el desarrollo de software. No es para la documentación del proyecto, sino para el proceso de desarrollo en sí.

¿Puedes investigar OpenSpec usando Context7 y explicarme:
1. ¿Qué es exactamente y cómo funciona?
2. ¿Dónde encajaría en la estructura actual del proyecto?
3. ¿Cómo debería integrarlo en el workflow de desarrollo?

Necesito entender si vale la pena adoptarlo y cómo se complementaría con la estructura de documentación que acabamos de crear.
```

**Resultado:**
Investigación con Context7 reveló que OpenSpec es una metodología spec-driven con workflow de 3 etapas (Proposal→Design→Tasks). Se recomendó ubicación en raíz (`/openspec`) con estructura de specs, changes y archive. Se explicó integración con flujo actual: WorkTickets.md define QUÉ hacer, /openspec/changes/ guía CÓMO implementar.

**Decisiones tomadas:**

-   Adoptar OpenSpec para Entrega 2 (desarrollo de código - 21 de enero)
-   Crear /openspec en raíz del proyecto con AGENTS.md, /specs, /changes, /archive
-   Workflow integrado: Planificación (docs/WorkTickets.md) → Desarrollo (openspec/changes/) → Implementación
-   Ventajas: Previene scope creep, documentación viva, sistema de deltas para cambios

---

**Prompt 2: Configuración de Docker Compose**

**Contexto:**
Necesidad de entorno de desarrollo reproducible con PostgreSQL, .NET Web API y React frontend.

**Prompt original:**

```
Configura Docker Compose para el proyecto con PostgreSQL 16, .NET 10 Web API, y React frontend. Incluye volúmenes persistentes para la base de datos y configuración de variables de entorno para desarrollo local. Asegúrate de que los servicios puedan comunicarse entre sí y que el hot reload funcione para desarrollo.
```

**Resultado:**
docker-compose.yml con 3 servicios (postgres, api, web), configuración de redes y volúmenes, variables de entorno para desarrollo, mapeo de puertos (PostgreSQL:5432, API:5000, Web:3000).

**Decisiones tomadas:**

-   PostgreSQL 16 en contenedor con volumen persistente
-   .NET Web API con hot reload habilitado
-   React frontend con Vite en modo desarrollo
-   Red bridge para comunicación entre servicios
-   Variables de entorno configurables (DB connection, API URLs)

---

**Prompt 3: Pipeline CI/CD con GitHub Actions**

**Contexto:**
Automatización de tests y despliegue para asegurar calidad en cada cambio.

**Prompt original:**

```
Crea un workflow de GitHub Actions que ejecute tests en cada PR, genere reporte de cobertura con Coverlet, y despliegue automáticamente a Render.com cuando se mergea a main. Incluye jobs separados para build, test, coverage y deploy. El deploy debe esperar a que los tests pasen exitosamente.
```

**Resultado:**
.github/workflows/ci-cd.yml con 4 jobs (build, test con cobertura >70%, coverage report, deploy condicional a main). Configuración de secretos (RENDER_API_KEY, DATABASE_URL).

**Decisiones tomadas:**

-   Tests obligatorios antes de merge (status check en GitHub)
-   Cobertura mínima >70% (falla si no se cumple)
-   Deploy automático solo en rama main
-   Reporte de cobertura como artifact
-   Uso de Render.com para hosting (plan gratuito disponible)

---

### 2.5. Seguridad

**Prompt 1: Estrategia BYOK y Encriptación de API Keys**

**Contexto:**
Necesidad de que usuarios aporten su propia API Key de API-Football de forma segura.

**Prompt original:**

```
Implementa el sistema BYOK (Bring Your Own Key) donde cada usuario proporciona su propia API Key de API-Football. La key debe almacenarse encriptada en la base de datos usando AES-256 con una clave maestra almacenada en secrets. Antes de guardar la key, valida que sea válida haciendo un request de prueba al endpoint /status de API-Football.
```

**Resultado:**
Modelo BYOK definido con API Key almacenada encriptada (AES-256), validación contra API-Football antes de guardar, clave maestra en variables de entorno (nunca en código). Servicio `ApiKeyEncryptionService` con métodos Encrypt/Decrypt.

**Decisiones tomadas:**

-   Usuarios aportan su propia API Key (evita costos del sistema, permite aprovechar planes premium)
-   Encriptación AES-256 con clave maestra en appsettings/secrets (nunca en base de datos)
-   Validación de key al guardar (request a /status de API-Football)
-   Modelo: User.EncryptedApiKey (string cifrado, no plaintext)

---

**Prompt 2: Autenticación JWT con ASP.NET Core Identity**

**Contexto:**
Necesidad de autenticación segura sin mantener sesiones en servidor.

**Prompt original:**

```
Implementa autenticación JWT con ASP.NET Core Identity. El token debe expirar en 7 días, incluir claims (user_id, email, role), y estar firmado con clave secreta configurada en appsettings.json. Configura middleware para validar token en cada request protegido. La contraseña debe hashearse con PBKDF2 (por defecto en Identity).
```

**Resultado:**
JWT con expiración de 7 días, claims estándar (user_id, email, role opcional), middleware de autenticación configurado, contraseñas hasheadas con PBKDF2 (Identity default). Servicio `JwtTokenGenerator` para generar tokens.

**Decisiones tomadas:**

-   ASP.NET Core Identity para gestión de usuarios (robusto y estándar de .NET)
-   JWT sin refresh tokens para MVP (simplicidad)
-   Clave secreta en variables de entorno (appsettings.json solo para desarrollo)
-   Atributo [Authorize] en endpoints protegidos
-   Política de contraseña: mínimo 8 caracteres, 1 mayúscula, 1 número

---

**Prompt 3: Rate Limiting y HTTPS Obligatorio**

**Contexto:**
Protección contra abuso de API y seguridad en tránsito de datos.

**Prompt original:**

```
Configura rate limiting por usuario (100 requests/día alineado con plan Free de API-Football) usando caché de contadores en PostgreSQL. Implementa HTTPS obligatorio en producción con certificado de Let's Encrypt. Usa Polly para retry con exponential backoff en llamadas a API-Football externa (máximo 3 intentos).
```

**Resultado:**
Rate limiting con tabla `RateLimits` en PostgreSQL (UserId, Date, RequestCount), middleware que verifica contador antes de ejecutar request. HTTPS forzado en producción (Heroku/Render proveen certificados). Polly configurado con política de retry (3 intentos, backoff exponencial 2s, 4s, 8s).

**Decisiones tomadas:**

-   100 requests/día por usuario (alineado con API-Football Free tier)
-   Contador en PostgreSQL (más robusto que memoria para múltiples instancias)
-   Solo HTTPS en producción (HTTP permitido en desarrollo local)
-   Retry automático con Polly para resiliencia (3 intentos máximo)
-   Errores 429 (Too Many Requests) si se excede límite

---

### 2.6. Tests

**Prompt 1: Estrategia de Testing Multi-Nivel**

**Contexto:**
Necesidad de cobertura >70% con tests en diferentes niveles (unitarios, integración, E2E).

**Prompt original:**

```
Define la estrategia de testing del proyecto incluyendo tests unitarios (handlers, validators, services), tests de integración (con Testcontainers para PostgreSQL), y al menos 1 test E2E (Login → Enviar mensaje → Recibir respuesta). Usa xUnit, FluentAssertions, y Playwright para E2E. Configura Coverlet para reporte de cobertura con mínimo 70%.
```

**Resultado:**
Estrategia de 3 niveles: unitarios (handlers, validators con mocks), integración (Testcontainers para PostgreSQL real), E2E (Playwright para flujo completo). xUnit como framework, FluentAssertions para asserts legibles, Coverlet para cobertura >70%.

**Decisiones tomadas:**

-   Tests unitarios: xUnit + NSubstitute para mocks + FluentAssertions
-   Tests de integración: Testcontainers (PostgreSQL real en contenedor temporal)
-   Tests E2E: Playwright (Login → Chat → Respuesta)
-   Cobertura >70% obligatoria (falla CI/CD si no se cumple)
-   Testcontainers para aislamiento (base de datos limpia por test)

---

**Prompt 2: Tests Unitarios de Handlers con MediatR**

**Contexto:**
Validar lógica de negocio en Features (Vertical Slices) sin dependencias externas.

**Prompt original:**

```
Crea tests unitarios para los handlers de Auth (Register, Login) y Chat (SendMessage). Mockea dependencias externas (API-Football, Semantic Kernel, PostgreSQL) usando NSubstitute. Valida que FluentValidation rechaza inputs inválidos con mensajes claros. Usa patrón Arrange-Act-Assert.
```

**Resultado:**
Tests unitarios para cada handler (RegisterHandler, LoginHandler, SendMessageHandler), mocks de dependencias (IApiFootballClient, ISemanticKernelService, IAppDbContext), validación de FluentValidation con asserts de mensajes específicos. Patrón AAA aplicado consistentemente.

**Decisiones tomadas:**

-   NSubstitute para mocks (sintaxis fluida, fácil de leer)
-   Tests de validación separados (un test por regla de validación)
-   FluentAssertions para asserts expresivos: `result.Should().BeEquivalentTo(expected)`
-   Arrange-Act-Assert pattern en todos los tests
-   Nombrado: `{MethodName}_{Scenario}_{ExpectedResult}` (ej: `Login_WithInvalidCredentials_ReturnsError`)

---

**Prompt 3: Tests de Integración con Testcontainers**

**Contexto:**
Validar flujo completo con base de datos real sin afectar datos de desarrollo.

**Prompt original:**

```
Implementa tests de integración que levanten PostgreSQL con Testcontainers, apliquen migraciones de Entity Framework, y validen flujos completos: 1) Registro → Login → Obtener perfil, 2) Guardar mensaje → Recuperar conversación. Limpia base de datos entre tests usando respawn o truncate. Usa WebApplicationFactory para levantar API en memoria.
```

**Resultado:**
Tests con PostgreSQL real en contenedor (Testcontainers), migraciones aplicadas automáticamente (EF Core), cleanup entre tests (Respawn library), WebApplicationFactory para API en memoria. Flujos validados: autenticación completa, persistencia de conversaciones.

**Decisiones tomadas:**

-   Testcontainers para PostgreSQL real (más confiable que mocks para validar queries)
-   WebApplicationFactory para levantar API en memoria (sin necesidad de servidor real)
-   Respawn library para limpiar BD entre tests (más rápido que recrear contenedor)
-   Base de datos limpia por test (aislamiento garantizado)
-   Validación de migraciones (si migraciones fallan, test falla)

---

## 3. Modelo de Datos

**Prompt 1: Definición de Entidades Principales**

**Contexto:**
Definición del modelo de datos simplificado para MVP basado en funcionalidades Must-Have.

**Prompt original:**

```
Define las entidades principales del modelo de datos para el chatbot de estadísticas de fútbol. Basándote en las funcionalidades Must-Have (autenticación, chat, historial, caché), diseña entidades con sus atributos principales, tipos de datos, y relaciones. Prioriza simplicidad sobre completitud para el MVP.
```

**Resultado:**
4 entidades definidas: User (Id, Email, PasswordHash, EncryptedApiKey, CreatedAt), Conversation (Id, UserId, Title, CreatedAt, UpdatedAt), Message (Id, ConversationId, Role, Content, Timestamp), CachedQuery (Id, QueryType, Parameters JSON, Response JSON, ExpiresAt, CreatedAt).

**Decisiones tomadas:**

-   User con API Key encriptada (AES-256 para seguridad)
-   Conversation con título autogenerado desde primer mensaje
-   Message con role (user/assistant) para contexto conversacional
-   CachedQuery con JSONB para flexibilidad (evitar columnas rígidas)
-   Relaciones: User 1:N Conversation, Conversation 1:N Message
-   CachedQuery sin FK (tabla independiente para máxima flexibilidad)

---

**Prompt 2: Generación de Diagrama ERD en Mermaid**

**Contexto:**
Necesidad de visualizar relaciones entre entidades para documentación y comunicación del diseño.

**Prompt original:**

```
Genera un diagrama ERD en formato Mermaid para las 4 entidades (User, Conversation, Message, CachedQuery). Incluye tipos de datos específicos de PostgreSQL, claves primarias/foráneas, y relaciones con cardinalidad (1:N). Marca campos unique y campos nullable.
```

**Resultado:**
Diagrama Mermaid con relaciones claras: User 1:N Conversation (FK: UserId), Conversation 1:N Message (FK: ConversationId). Tipos de datos PostgreSQL: UUID para IDs, VARCHAR para strings, TEXT para contenido largo, JSONB para datos flexibles, TIMESTAMP para fechas.

**Decisiones tomadas:**

-   UUID como tipo de ID (mejor para sistemas distribuidos, sin colisiones)
-   User.Email con índice UNIQUE (garantiza unicidad a nivel de BD)
-   CachedQuery.QueryHash con índice UNIQUE (evita duplicados, mejora performance)
-   Conversation.Title tipo VARCHAR(200) (suficiente para títulos autogenerados)
-   Message.Content tipo TEXT (sin límite de longitud para respuestas complejas)
-   Cascade delete en Conversation→Message (simplifica limpieza)

---

**Prompt 3: Configuración de Fluent API en Entity Framework**

**Contexto:**
Configurar restricciones, índices y relaciones en Entity Framework Core usando Fluent API.

**Prompt original:**

```
Configura Fluent API para las entidades con: índices (User.Email unique, CachedQuery.QueryHash unique), restricciones de cascade delete (Conversation → Messages), y conversión de tipos (EncryptedApiKey string cifrado). Organiza configuraciones en archivos separados bajo /Infrastructure/Persistence/Configurations siguiendo patrón IEntityTypeConfiguration.
```

**Resultado:**
Archivos de configuración separados (UserConfiguration.cs, ConversationConfiguration.cs, MessageConfiguration.cs, CachedQueryConfiguration.cs), índices definidos con `HasIndex().IsUnique()`, cascade delete con `OnDelete(DeleteBehavior.Cascade)`, conversión de EncryptedApiKey manejada en servicio separado.

**Decisiones tomadas:**

-   Índice único en User.Email (evita duplicados, mejora búsquedas)
-   Índice único en CachedQuery.QueryHash (previene cache duplicado, performance)
-   Cascade delete en Conversation→Message (al borrar conversación, borra mensajes)
-   Archivo por entidad (UserConfiguration.cs, ConversationConfiguration.cs, etc.)
-   IEntityTypeConfiguration<T> para separación de concerns
-   Restricción NOT NULL en campos obligatorios (User.Email, Message.Content)

---

## 4. Especificación de la API

**Prompt 1: Análisis de Limitación de API-Football y Planes**

**Contexto:**
Evaluación de si planes de pago de API-Football resuelven limitación con consultas complejas.

**Prompt original:**

```
He identificado una limitación crítica con consultas complejas. El ejemplo "Compara el rendimiento de Haaland y Mbappé en partidos away" parece requerir múltiples llamadas a la API.

Necesito evaluar dos aspectos:

1. **Planes de API-Football**: ¿Los planes de pago (Pro o Ultra) ofrecen endpoints adicionales o funcionalidades que simplifiquen este tipo de consultas? Usa Context7 para investigar la documentación oficial y comparar capacidades entre planes.

2. **Decisión de scope**: Considerando el alcance del proyecto (MVP en ~30 horas), ¿es más sensato enfocarse en consultas simples (1-2 API calls) para el MVP y dejar consultas complejas como feature avanzado post-MVP?

Necesito datos concretos para tomar una decisión informada sobre la arquitectura.
```

**Resultado:**
Investigación con Context7 confirmó limitación: no existe endpoint directo para stats de jugador por home/away. Workaround requiere ~42 API calls (1 búsqueda jugador + 1 fixtures + 19 stats por partido × 2 jugadores). Análisis de capacidad: Free 100 req/día (2 consultas complejas), Pro 7,200 (171), Ultra 75,000 (1,785).

**Decisiones tomadas:**

-   Mantener enfoque simple para MVP (consultas de 1-2 API calls)
-   Las 5 consultas Must-Have son alcanzables con plan Free (100 req/día por usuario)
-   Consultas complejas como comparaciones away son feature avanzado post-MVP
-   Modelo BYOK es suficiente para escala inicial
-   Limitación es técnica (no existe endpoint), no de planes de pago

---

**Prompt 2: Actualización de Ejemplo Realista**

**Contexto:**
Cambiar ejemplo complejo por consulta alcanzable para MVP y reflejar scope realista.

**Prompt original:**

```
Confirmado. Procede a actualizar @docs/01-product/ProjectOverview.md con un ejemplo de consulta más realista y alcanzable para el MVP.

Cambia el ejemplo complejo (Haaland vs Mbappé en partidos away) por una consulta simple que requiera solo 1-2 llamadas a la API, como "Máximos goleadores de una liga".
```

**Resultado:**
Actualización de ProjectOverview.md: ejemplo "Haaland vs Mbappé away" (60+ calls) reemplazado por "¿Quiénes son los máximos goleadores de la Premier League?" (1 call). Métrica corregida de "8 de 10 tipos de consultas" a "5 de 5 Must-Have". Incluye tabla de ejemplo formateada en respuesta.

**Decisiones tomadas:**

-   Ejemplo nuevo: GET /players/topscorers?league=39&season=2024 (1 API call)
-   Respuesta formateada en tabla Markdown con nombre, equipo, goles
-   Métrica actualizada a "5 de 5 Must-Have funcionando" (más realista)
-   Ejemplos deben reflejar capacidades reales del MVP

---

**Prompt 3: Análisis de Escalabilidad y Alternativas de APIs**

**Contexto:**
Profundizar en capacidad del plan Ultra y explorar alternativas a API-Football en el mercado.

**Prompt original:**

```
Quiero profundizar en dos aspectos antes de tomar una decisión final:

1. **Capacidad realista del plan Ultra**: Con 75,000 requests/día, ¿cuántos usuarios activos podría soportar si el 30% de las consultas son complejas (tipo "Haaland vs Mbappé away", ~42 calls) y el 70% son simples (~2 calls)?

Calcula escenarios realistas de carga: usuarios concurrentes, consultas por usuario promedio, y capacidad diaria total. No espero miles de usuarios, pero necesito saber si puedo escalar de 10 a 100 usuarios.

2. **Alternativas a API-Football**: Investiga con Context7 si existen otras APIs de estadísticas de fútbol que ofrezcan:
   - Stats de jugador desglosadas por home/away (sin múltiples calls)
   - Mejor relación precio/requests
   - Cobertura similar (ligas principales 2010-2025)

APIs candidatas: Sportmonks, FootyStats, o cualquier otra que encuentres.

Necesito datos concretos para decidir si API-Football es la mejor opción o si debo considerar un cambio de proveedor.
```

**Resultado:**
Investigación con Context7 de alternativas (Sportmonks $150/mes ilimitado\*, FootyStats $49/mes 3,600 req/día). Cálculo de capacidad Ultra: ~500-1,000 usuarios activos diarios con mix 70% simples/30% complejas. Descubrimiento crítico: **ninguna API del mercado ofrece stats jugador home/away directo** (es limitación de toda la industria).

**Decisiones tomadas:**

-   API-Football es la mejor opción (precio $29/mes Ultra, capacidad 75k req/día, documentación completa)
-   Sportmonks: más caro, sistema de "includes" complica cálculos, tampoco tiene stats jugador home/away
-   FootyStats: solo 3,600 req/día (insuficiente), enfocado en stats de equipos no jugadores
-   Conclusión: Es limitación de toda la industria (datos de Opta Sports no desglosan jugadores por local/visitante)
-   Plan Ultra soporta 500-1,000 usuarios activos/día (suficiente para MVP y escala inicial)

---

## 5. Historias de Usuario

**Prompt 1: Creación de Historias en Formato Gherkin**

**Contexto:**
Necesidad de formalizar requisitos funcionales en formato estándar Gherkin para guiar desarrollo.

**Prompt original:**

```
Crea 20 historias de usuario en formato Gherkin (Como/Quiero/Para) basadas en las funcionalidades Must-Have y Should-Have de @CLAUDE.md. Organízalas en 5 Epics: Autenticación, Consultas de Chat, Historial, Consultas Soportadas (Must-Have), y Funcionalidades Adicionales (Should-Have).

Incluye para cada historia:
- Criterios de aceptación técnicos y verificables
- Notas de implementación con stack específico
- Prioridad (Must-Have / Should-Have)
- Estimación (S: 1-2 días, M: 3-5 días, L: 1-2 semanas)

Prioriza 15 historias Must-Have que cubran las 6 funcionalidades críticas del MVP.
```

**Resultado:**
20 historias completas organizadas en 5 Epics: Epic 1 - Autenticación (US-001 a US-004), Epic 2 - Consultas de Chat (US-005a, US-005b, US-006, US-007), Epic 3 - Historial (US-008 a US-010), Epic 4 - Consultas Soportadas Must-Have (US-011 a US-015), Epic 5 - Should-Have (US-016 a US-020). Formato Gherkin consistente con criterios de aceptación técnicos, notas de implementación con librerías específicas, prioridad y estimación por historia.

**Decisiones tomadas:**

-   15 Must-Have (US-001 a US-015): cubren autenticación, BYOK, chat con NLP, historial, caché, 5 tipos de consultas
-   5 Should-Have (US-016 a US-020): enfrentamientos directos, info jugador, asistentes, dashboard, exportación CSV
-   Formato Gherkin estándar: Como [rol] / Quiero [acción] / Para [beneficio]
-   Criterios de aceptación técnicos: incluyen validaciones específicas, librerías (FluentValidation, ASP.NET Core Identity), políticas de contraseña
-   Estimaciones: S (1-2d) para CRUD simple, M (3-5d) para features con IA/NLP, L (1-2w) para UI compleja

---

**Prompt 2: Refinamiento de Criterios de Aceptación**

**Contexto:**
Asegurar que criterios sean específicos, medibles, verificables y alineados con stack tecnológico.

**Prompt original:**

```
Revisa las 20 historias de @docs/01-product/UserStories.md y refina los criterios de aceptación para que sean específicos, medibles, y alineados con el stack tecnológico (React, .NET 10, PostgreSQL).

Por ejemplo:
- "La contraseña debe tener mínimo 8 caracteres, 1 mayúscula, 1 número"
- "JWT con expiración de 7 días"
- "Caché con TTL diferenciado: 30 días para datos históricos, 1 hora para actuales"

Asegúrate de que cada criterio pueda validarse automáticamente en tests o manualmente en QA.
```

**Resultado:**
Criterios de aceptación refinados con valores concretos: contraseña mínimo 8 chars + 1 mayúscula + 1 número, JWT 7 días, caché TTL diferenciado (30d históricos / 1h actuales), validaciones con regex estándar, mensajes de error específicos ("Email o contraseña incorrectos"), tecnologías explícitas (FluentValidation, PBKDF2, System.Security.Cryptography.Aes).

**Decisiones tomadas:**

-   Validaciones específicas: regex para email, política de contraseña clara (8 chars, 1 mayúscula, 1 número)
-   JWT: expiración 7 días, claims (user_id, email, role opcional)
-   Caché TTL: 30 días para datos históricos (goleadores 2010-2023), 1 hora para datos actuales (resultados recientes, próximos partidos)
-   Encriptación API Key: AES-256 con clave maestra en secrets
-   Mensajes de error genéricos para seguridad ("Email o contraseña incorrectos" sin especificar cuál falló)

---

**Prompt 3: Validación de Priorización Must-Have vs Should-Have**

**Contexto:**
Confirmar que la priorización alinea con scope del MVP y plan de entregas.

**Prompt original:**

```
Valida que las 15 historias Must-Have (US-001 a US-015) cubren completamente las 6 funcionalidades críticas de @CLAUDE.md:
1. Sistema de autenticación (registro, login, logout)
2. Configuración de API Key (BYOK)
3. Interfaz de chat con NLP
4. Historial de conversaciones
5. Sistema de caché
6. 5 tipos de consultas soportadas (goleadores, clasificación, resultados, próximos, stats equipo)

Confirma que las 5 historias Should-Have (US-016 a US-020) son realmente opcionales y no bloquean la Entrega 2 (21 de enero). Si alguna Should-Have es crítica, reclasifícala como Must-Have.
```

**Resultado:**
Validación completa confirmada. Las 15 Must-Have cubren las 6 funcionalidades críticas: US-001 a US-004 (autenticación + BYOK), US-005a/b (NLP con Semantic Kernel), US-006 (respuestas formateadas), US-007 (caché), US-008 a US-010 (historial), US-011 a US-015 (5 tipos de consultas). Las 5 Should-Have son opcionales para MVP: US-016 (enfrentamientos), US-017 (info jugador), US-018 (asistentes), US-019 (dashboard admin), US-020 (exportación CSV).

**Decisiones tomadas:**

-   Priorización correcta: 15 Must-Have cubren MVP funcional
-   5 Should-Have para Entrega 3 (post-MVP, 3 de febrero)
-   MVP es funcional con solo las 15 Must-Have
-   Ninguna Should-Have bloquea funcionalidad crítica
-   Dashboard admin (US-019) y exportación (US-020) son nice-to-have, no críticos

---

## 6. Tickets de Trabajo

**Prompt 1: Consolidación de Historias en Tickets Implementables**

**Contexto:**
Necesidad de consolidar 20 historias en unidades de trabajo técnicas implementables por capas.

**Prompt original:**

```
Crea 7 tickets de trabajo que mapeen las 20 historias de @docs/01-product/UserStories.md. Organiza por capas técnicas:
- TICK-001: Setup del Proyecto y Base de Datos (prerequisito)
- TICK-002: Backend Auth (US-001, US-002, US-003, US-004)
- TICK-003: Backend IA/NLP con Semantic Kernel (US-005a, US-005b)
- TICK-004: Backend Chat y Caché (US-006, US-007, US-008)
- TICK-005: Frontend Auth (US-001, US-002, US-003, US-004)
- TICK-006: Frontend Chat UI (US-006, US-009, US-010)
- TICK-007: Tests y Despliegue (todas las historias)

Incluye para cada ticket:
- Historias relacionadas (US-XXX)
- Estimación (S/M/L)
- Dependencias (TICK-XXX)
- Criterios de aceptación técnicos
- Tareas específicas (ej: "Instalar MediatR", "Crear Features/Auth/Login.cs")
```

**Resultado:**
7 tickets consolidados con mapeo claro a historias: TICK-001 (Setup, M-3d), TICK-002 (Auth Backend, M-3d, US-001 a US-004), TICK-003 (Semantic Kernel, M-3d, US-005a/b), TICK-004 (Chat Backend + Cache, L-5d, US-006/007/008), TICK-005 (Auth Frontend, M-2d, US-001 a US-004), TICK-006 (Chat UI, L-5d, US-006/009/010), TICK-007 (Tests + Deploy, M-4d, todas). Cada ticket con 6-10 criterios de aceptación y 8-15 tareas técnicas específicas.

**Decisiones tomadas:**

-   Estructura por capas: setup → backend → frontend → deploy
-   TICK-002 y TICK-003 pueden empezar en paralelo tras TICK-001
-   TICK-005 puede empezar tras TICK-002 (Auth Backend completo)
-   TICK-006 depende de TICK-004 (Chat Backend + endpoints)
-   Estimación total: ~25 días teóricos (setup 3d + auth 5d + IA 3d + chat 5d + frontend 7d + tests 4d)
-   Tickets paralelos donde sea posible para optimizar tiempo

---

**Prompt 2: Definición de Dependencias y Camino Crítico**

**Contexto:**
Asegurar orden lógico de implementación, evitar bloqueos, e identificar camino crítico.

**Prompt original:**

```
Revisa los 7 tickets de @docs/05-operations/WorkTickets.md y valida que las dependencias sean correctas.

Por ejemplo:
- TICK-002 (Auth Backend) debe completarse antes de TICK-005 (Auth Frontend)
- Pero TICK-005 puede empezar en paralelo con TICK-003 (Semantic Kernel)

Genera un diagrama de dependencias en formato texto que muestre:
1. El camino crítico (secuencia más larga)
2. Tickets que pueden ejecutarse en paralelo
3. Estimación del tiempo mínimo del proyecto si se trabaja en paralelo vs secuencial
```

**Resultado:**
Diagrama de dependencias validado: TICK-001 → TICK-002 → [TICK-005 en paralelo con TICK-003] → TICK-004 → TICK-006 → TICK-007. Camino crítico: TICK-001 → TICK-002 → TICK-003 → TICK-004 → TICK-006 → TICK-007 (21 días). Tiempo optimizado con paralelismo: 19 días (TICK-005 en paralelo con TICK-003).

**Decisiones tomadas:**

-   Camino crítico: Setup → Auth Backend → Semantic Kernel → Chat Backend → Chat Frontend → Tests/Deploy
-   TICK-005 (Auth Frontend) puede empezar tras TICK-002, en paralelo con TICK-003
-   Backend Auth bloquea Frontend Auth (necesita endpoints /register, /login)
-   Chat Backend bloquea Chat Frontend (necesita endpoint /chat/send)
-   Tests al final tras todo el código (TICK-007 depende de todos)
-   Tiempo secuencial: 25 días | Tiempo con paralelismo: 23 días (ahorro de 2 días)

---

**Prompt 3: Validación de Estimaciones y Distribución por Entregas**

**Contexto:**
Validar que estimaciones son realistas para el scope del proyecto y distribuir trabajo en 3 entregas.

**Prompt original:**

```
Valida las estimaciones de los 7 tickets: ¿25 días totales es realista para implementar el MVP?

Considerando que es un solo desarrollador trabajando ~30 horas totales distribuidas en 3 entregas (documentación + código + tests), ¿las estimaciones son teóricas o literales?

Ajusta si es necesario o explica la discrepancia. Además, distribuye los tickets en las 3 entregas:
- Entrega 1 (17 dic): Solo documentación (ya completada)
- Entrega 2 (21 ene): Backend + frontend funcional (MVP ejecutable)
- Entrega 3 (3 feb): Tests, deploy, refinamientos
```

**Resultado:**
Aclaración de que 25 días son estimación teórica para dimensionar trabajo, no literal para 1 persona en 30 horas. Distribución: Entrega 1 (documentación completa), Entrega 2 (TICK-001 a TICK-004 + TICK-005 parcial = backend funcional con auth básico), Entrega 3 (TICK-005 completo + TICK-006 + TICK-007 = frontend completo, tests, deploy).

**Decisiones tomadas:**

-   Estimaciones teóricas: indican complejidad relativa, no tiempo literal
-   Entrega 2 prioriza backend funcional: Setup + Auth + Semantic Kernel + Chat Backend
-   Frontend Auth básico en Entrega 2 (login/register mínimo)
-   Chat UI completo y tests para Entrega 3
-   30 horas reales distribuidas: ~10h documentación, ~15h código, ~5h tests/deploy
-   Priorización: funcionalidad core primero (backend), UI después (frontend), calidad al final (tests)

---

## 7. Pull Requests

**Estado:** N/A para Entrega 1 (solo documentación, sin código ni PRs)

**Nota:** Esta sección se completará en la Entrega 3 (3 de febrero) cuando el código esté desplegado y los pull requests estén creados. Los pull requests documentarán la implementación de los tickets TICK-001 a TICK-007, mostrando el proceso de revisión, commits, y merge a la rama principal.

**Ejemplo de prompts futuros (para Entrega 3):**

**Prompt 1 (futuro):** "Crea un pull request para TICK-002 (Sistema de Autenticación) con descripción detallada de cambios, testing realizado, y screenshots de endpoints funcionando en Postman."

**Prompt 2 (futuro):** "Revisa el PR #5 de integración de Semantic Kernel. Valida que cumple con los criterios de aceptación de TICK-003 y sugiere mejoras de código."

**Prompt 3 (futuro):** "Genera el changelog automático desde los pull requests mergeados para la release v1.0.0-final."

---

## Conversaciones Completas

Las conversaciones completas documentadas están disponibles en:

-   [01 - Validación y Arquitectura del Sistema](./conversaciones/01-validacion-arquitectura.md)
-   [02 - Stack Tecnológico y Diseño Visual](./conversaciones/02-stack-y-diseno.md)
-   [03 - Documentación y Metodología OpenSpec](./conversaciones/03-documentacion-metodologia.md)
-   [04 - Análisis de Limitaciones de API-Football](./conversaciones/04-limitaciones-api.md)

---

**Última actualización:** 16 de Diciembre de 2025
