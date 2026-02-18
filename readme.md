# GolMetrics - Chatbot de Estadísticas de Fútbol

GolMetrics es un producto final desarrollado para el curso **AI4Devs**. Es un chatbot inteligente que permite consultar estadísticas de fútbol en tiempo real mediante lenguaje natural, utilizando .NET 10, React 19 y Google Gemini.

**URL de producción:** [https://golmetrics-web.onrender.com](https://golmetrics-web.onrender.com)

## Índice

0. [Ficha del proyecto](#0-ficha-del-proyecto)
1. [Descripción general del producto](#1-descripción-general-del-producto)
2. [Arquitectura del sistema](#2-arquitectura-del-sistema)
3. [Modelo de datos](#3-modelo-de-datos)
4. [Especificación de la API](#4-especificación-de-la-api)
5. [Historias de usuario](#5-historias-de-usuario)
6. [Tickets de trabajo](#6-tickets-de-trabajo)
7. [Metodología de Desarrollo (OpenSpec)](#7-metodología-de-desarrollo-openspec)
8. [Infraestructura y Despliegue](#8-infraestructura-y-despliegue)

---

## 0. Ficha del proyecto

### **0.1. Autor:**
Lester David López Bustillo

### **0.2. Nombre del proyecto:**
**GolMetrics**

### **0.3. Descripción:**
Chatbot especializado en analítica deportiva que democratiza el acceso a datos complejos de fútbol a través de una interfaz conversacional, eliminando la necesidad de navegar por portales de estadísticas tradicionales.

---

## 1. Descripción general del producto

### **1.1. Propuesta de Valor:**
- **Interfaz Conversacional:** Consultas como "¿Quién es el pichichi de La Liga?" devuelven tablas formateadas al instante.
- **BYOK (Bring Your Own Key):** Permite a los usuarios usar su propia suscripción de API-Football, gestionando su cuota de forma independiente.
- **Caché Inteligente:** Reduce la latencia y el consumo de créditos mediante un sistema de caché en base de datos con TTL diferenciado.

---

## 2. Arquitectura del Sistema

### **2.1. Stack Tecnológico:**
- **Backend:** .NET 10 (Minimal APIs), MediatR (CQRS), FluentValidation, Entity Framework Core.
- **Frontend:** React 19, Vite, TypeScript, Tailwind CSS 4, shadcn/ui, Zustand.
- **IA:** Semantic Kernel + Google Gemini 2.0 Flash.
- **Base de Datos:** PostgreSQL 16.

### **2.2. Patrón Arquitectónico:**
Se implementó **Vertical Slice Architecture (VSA)**. Cada funcionalidad (Auth, Chat, User) es una pieza autónoma que contiene su ruteo, lógica de negocio y persistencia, minimizando el acoplamiento y facilitando la mantenibilidad por agentes de IA.

---

## 3. Modelo de Datos

El sistema utiliza un esquema relacional optimizado para identidades y caché:
- **Users:** Extiende `IdentityUser` con soporte para claves de API encriptadas (AES-256).
- **Conversations & Messages:** Almacenan el historial completo con ruteo basado en propiedad del usuario.
- **CachedQueries:** Utiliza columnas **JSONB** para almacenar las respuestas crudas de API-Football, permitiendo una recuperación de datos veloz sin procesar de nuevo la respuesta externa.

---

## 4. Especificación de la API

| Endpoint | Método | Descripción |
| :--- | :--- | :--- |
| `/api/auth/register` | POST | Registro de nuevos usuarios. |
| `/api/auth/login` | POST | Login y obtención de JWT. |
| `/api/chat/message` | POST | Envío de mensajes y orquestación de IA. |
| `/api/user/profile` | GET | Visualización de perfil y estado de API Key. |
| `/api/user/api-key` | PUT | Configuración de clave BYOK (validada y encriptada). |

---

## 7. Metodología de Desarrollo (OpenSpec)

Este proyecto no fue desarrollado mediante "vibe coding" o prompts aislados. Se utilizó **OpenSpec**, un framework de especificaciones vivas que actúa como la "Única Fuente de Verdad". 

**Beneficios obtenidos:**
- **Cero alucinaciones:** La IA implementó los servicios basándose en reglas SHALL/WHEN/THEN estrictas.
- **Trazabilidad:** Cada uno de los 20 tickets técnicos fue validado contra las specs antes de ser aplicado.
- **Calidad:** La estructura de Vertical Slices se mantuvo consistente en todo el proyecto gracias a las reglas definidas en `config.yaml`.

---

## 8. Infraestructura y Despliegue

### **8.1. CI/CD:**
Automatizado mediante **GitHub Actions**. El pipeline ejecuta:
1. Restauración y Build de la solución.
2. Suite de Tests Unitarios.
3. Tests de Integración usando **Testcontainers** (PostgreSQL real en el runner).
4. Despliegue automático a Render mediante Webhooks.

### **8.2. Despliegue:**
- **Hosting:** Render.com (Plan Free).
- **Contenedores:** Imágenes Docker multi-stage optimizadas para .NET y Nginx.
- **Infraestructura como Código:** Definida en `render.yaml` (Blueprints).
