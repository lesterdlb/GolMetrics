> Detalla en esta sección los prompts principales utilizados durante la creación del proyecto, que justifiquen el uso de asistentes de código en todas las fases del ciclo de vida del desarrollo. Esperamos un máximo de 3 por sección, principalmente los de creación inicial o los de corrección o adición de funcionalidades que consideres más relevantes.
> Puedes añadir adicionalmente la conversación completa como link o archivo adjunto si así lo consideras

## Índice

1. [Descripción general del producto](#1-descripción-general-del-producto)
2. [Arquitectura del sistema](#2-arquitectura-del-sistema)
3. [Modelo de datos](#3-modelo-de-datos)
4. [Especificación de la API](#4-especificación-de-la-api)
5. [Historias de usuario](#5-historias-de-usuario)
6. [Tickets de trabajo](#6-tickets-de-trabajo)
7. [Desarrollo (Entrega 2)](#desarrollo-entrega-2)
8. [Pull requests](#8-pull-requests)

---

## 1. Descripción general del producto

**Prompt 1:**
"Actúa como un Product Manager experto en deportes. Analiza la idea de un chatbot para estadísticas de fútbol y genera una descripción general del producto que incluya: visión, propuesta de valor para aficionados y periodistas, y las características principales del MVP."

---

## 2. Arquitectura del Sistema

### **2.4. Infraestructura y despliegue**

**Prompt 1 (Generación del esqueleto):**
"Dada la proximidad de la entrega, necesito implementar un 'Walking Skeleton' que conecte todas las capas. Genera los comandos necesarios para crear una solución .NET 10 Web API y un proyecto React con Vite. Incluye la configuración de Docker (Dockerfiles y docker-compose.yml) para orquestar el frontend, el backend y una base de datos PostgreSQL."

---

## 3. Modelo de Datos

**Prompt 1:**
"Diseña un modelo de datos relacional para GolMetrics en PostgreSQL. Necesito entidades para Usuarios (con soporte para API Keys encriptadas), Conversaciones, Mensajes y una tabla de caché para resultados de la API externa."

---

## 5. Historias de Usuario

**Prompt 1:**
"Genera 5 historias de usuario clave para un MVP de estadísticas de fútbol. Incluye criterios de aceptación detallados, enfocándote en la consulta de goleadores, tablas de posiciones y la configuración de claves de API personales (BYOK)."

---

## 6. Tickets de Trabajo

**Prompt 1:**
"Basado en las historias de usuario anteriores, desglosa el trabajo en tickets técnicos. Utiliza una estimación tipo T-shirt sizing y asegura la trazabilidad entre el ticket y la funcionalidad del sistema."

---

## Desarrollo (Entrega 2)

**Prompt 1 (Conexión Frontend-Backend):**
"Analiza el código del frontend y del backend. Crea un endpoint '/api/chat' en .NET que responda con un mensaje estático de 'Proof of Life'. Luego, actualiza la lógica de React para que, al enviar un mensaje, realice una petición real a este endpoint y muestre la respuesta en la interfaz."

**Prompt 2 (Configuración de entornos Local/Docker):**
"La aplicación debe funcionar tanto en Docker como en desarrollo local. Configura un archivo `.env.development` para el frontend y habilita el middleware de CORS en el backend de .NET para permitir peticiones desde el origen del servidor de desarrollo de Vite."

---

## 8. Pull Requests

**Prompt 1:**
"Genera una descripción profesional para un Pull Request de la 'Entrega 2'. El resumen debe explicar la implementación del Walking Skeleton, la arquitectura de contenedores y cómo verificar la conexión exitosa entre el cliente React y la API."