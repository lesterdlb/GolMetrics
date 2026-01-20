# Documentación Técnica - GolMetrics

Este directorio contiene la documentación técnica completa del proyecto GolMetrics, organizada por áreas funcionales.

> Cuando el desarrollo comience, esta documentación podrá ser actualizada y ampliada según sea necesario.

## Estructura de Documentación

### 📋 01. Producto

-   [**Project Overview**](01-product/ProjectOverview.md) - Visión general, objetivos y alcance del proyecto
-   [**User Stories**](01-product/UserStories.md) - Historias de usuario detalladas con criterios de aceptación

### 🏗️ 02. Arquitectura

-   [**Architecture**](02-architecture/Architecture.md) - Diseño del sistema, decisiones técnicas y diagramas C4
-   [**Data Model**](02-architecture/DataModel.md) - Modelo de datos, entidades y relaciones
-   [**Security**](02-architecture/Security.md) - Estrategia de seguridad, autenticación y cifrado

### 🔌 03. API

-   [**API Specification**](03-api/ApiSpec.md) - Especificación detallada de endpoints REST

### 🎨 04. Frontend

-   [**Frontend Integration**](04-frontend/FrontendIntegration.md) - Arquitectura del frontend, componentes y estado
-   [**UI Design**](04-frontend/UIDesign.md) - Wireframes, flujos de usuario y guía de diseño

### ⚙️ 05. Operaciones

-   [**Deployment Guide**](05-operations/DeploymentGuide.md) - Instrucciones de despliegue y configuración
-   [**Testing Strategy**](05-operations/Testing.md) - Estrategia de testing (unitarios, integración, E2E)
-   [**Work Tickets**](05-operations/WorkTickets.md) - Tickets de trabajo técnicos estilo Jira

---

## Convenciones

-   **Formato:** Todos los documentos están en Markdown
-   **Idioma:** Español (código y comentarios en inglés según CLAUDE.md global)
-   **Diagramas:** Mermaid para diagramas técnicos
-   **Versionado:** Se actualiza junto con el código en cada PR

## Relación con Entregables

El directorio `/Entregables` contiene el **resumen ejecutivo** para las entregas del curso.
Este directorio `/docs` contiene la **documentación técnica detallada** para el desarrollo.

```
/Entregables/README.md   → Resumen para evaluación del TA
/docs                    → Referencia técnica completa para desarrollo
```

---

**Última actualización:** 2025-10-10
