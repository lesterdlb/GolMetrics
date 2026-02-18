# Registro de Uso de IA - GolMetrics

Este proyecto ha sido desarrollado utilizando una metodología de **Desarrollo Guiado por Especificaciones (Spec-Driven Development)** mediante el framework **OpenSpec**. En lugar de interacciones informales con la IA, se utilizó un flujo de trabajo estructurado donde cada cambio fue precedido por un diseño técnico formal, permitiendo que asistentes como Claude Code y Gemini CLI ejecutaran implementaciones precisas y coherentes con la arquitectura definida.

## Índice

1. [Descripción general del producto](#1-descripción-general-del-producto)
2. [Arquitectura del sistema](#2-arquitectura-del-sistema)
3. [Modelo de datos](#3-modelo-de-datos)
4. [Especificación de la API](#4-especificación-de-la-api)
5. [Historias de usuario](#5-historias-de-usuario)
6. [Tickets de trabajo](#6-tickets-de-trabajo)
7. [Desarrollo con OpenSpec](#7-desarrollo-con-openspec)
8. [Refinamiento Humano y Correcciones](#8-refinamiento-humano-y-correcciones)

---

## 1. Descripción general del producto

**Prompt:**
"Actúa como un Product Manager senior en tecnología deportiva. Analiza la viabilidad de un chatbot que simplifique el acceso a estadísticas de fútbol complejas. Genera una visión de producto que destaque la eliminación de la fricción en la búsqueda de datos, una propuesta de valor basada en el modelo BYOK (Bring Your Own Key) y define el alcance de un MVP funcional."

---

## 2. Arquitectura del Sistema

**Prompt:**
"Diseña una arquitectura moderna para una aplicación Full-Stack (.NET 10 y React 19). La solución debe seguir el patrón de Vertical Slice Architecture para maximizar la cohesión. Define la integración de Semantic Kernel para la orquestación de IA y describe cómo se estructurarán las capas de abstracción core para asegurar que el sistema sea testable y escalable."

---

## 3. Modelo de Datos

**Prompt:**
"Genera un esquema de base de datos relacional en PostgreSQL que integre ASP.NET Identity para la gestión de usuarios. Incluye entidades para conversaciones, mensajes con roles, y una tabla de caché técnica que utilice columnas JSONB para almacenar respuestas de APIs externas de forma eficiente. Asegura que el modelo incluya campos de auditoría y tokens de concurrencia."

---

## 5. Historias de Usuario

**Prompt:**
"Define historias de usuario detalladas para el flujo E2E de GolMetrics. Cada historia debe seguir el formato estándar e incluir criterios de aceptación técnicos y funcionales, enfocándose especialmente en la seguridad de las claves de API del usuario y la precisión de las respuestas del chatbot mediante el uso de herramientas (plugins) de IA."

---

## 6. Tickets de Trabajo

**Prompt:**
"Basado en las especificaciones de arquitectura y las historias de usuario, genera un roadmap de desarrollo desglosado en 20 tickets técnicos atómicos. Organiza los tickets de forma incremental: primero la infraestructura core, luego la identidad, el pipeline de IA, las funcionalidades de chat y finalmente el despliegue automatizado."

---

## 7. Desarrollo con OpenSpec

En esta fase se utilizaron comandos de **OpenSpec** para guiar a la IA. El flujo no consistió en pedir código directamente, sino en validar artefactos de diseño antes de la implementación.

**Ejemplo de comando de flujo:**
`/opsx:ff TICK-010: Semantic Kernel service and FootballPlugin`

Este comando ordenó a la IA analizar las especificaciones existentes (`openspec/specs/`) y generar un plan de implementación (`proposal.md`, `design.md`, `tasks.md`) coherente con todo el contexto del proyecto antes de escribir una sola línea de C#.

---

## 8. Refinamiento Humano y Correcciones

A pesar de la precisión de OpenSpec, fue necesaria la intervención humana para resolver detalles complejos de integración:

1.  **Registro de TimeProvider:** La IA generó el servicio de caché pero olvidó registrar el `TimeProvider.System` en el contenedor de DI. Se corrigió manualmente en `DependencyInjection.cs`.
2.  **Ciclo de Vida del Kernel:** Inicialmente, la IA propuso el `Kernel` como Singleton, lo cual causaba conflictos con dependencias Scoped (como el cliente de API). Se corrigió utilizando el patrón de `.Clone()` por cada petición para inyectar plugins dependientes del usuario.
3.  **Configuración de Nginx en Render:** Se ajustó manualmente el archivo `nginx.render.conf` para asegurar que el ruteo de la SPA funcionara correctamente en el puerto dinámico asignado por Render.
