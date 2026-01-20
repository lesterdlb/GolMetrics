> Este documento fue elaborado sin el uso de inteligencia artificial generativa.

# Chatbot de estadísticas de fútbol

Un chatbot con IA capaz de responder preguntas específicas sobre estadísticas de fútbol basadas en datos reales o almacenados en una base de datos.

1. Descripción del producto

    - Nombre tentativo: GolMetrics
    - Objetivo: Permitir a los usuarios consultar datos y estadísticas de fútbol mediante lenguaje natural.
    - Usuarios principales: Aficionados, periodistas deportivos, apostadores.
    - Valor agregado: Respuestas inteligentes y rápidas a preguntas estadísticas.

2. Funcionalidades clave

    - Registro / login (básico)
    - Interfaz de chat donde el usuario pregunta en lenguaje natural.
    - Backend que interpreta la pregunta, consulta la base de datos y devuelve la respuesta.
    - Integración con una API externa de datos de fútbol (API-Football).
    - Panel administrativo (Angular) para cargar o actualizar datos manualmente. (opcional)
    - Permitir al usuario ingresar su propia API Key de API-Football para consultas personalizadas.
        - Esto permitirá a los usuarios aprovechar sus propias suscripciones y evitar limitaciones de la API gratuita.
    - Almacenamiento de las conversaciones del usuario, similar a ChatGPT.
        - Esto permitirá a los usuarios revisar sus interacciones previas y mejorar la experiencia de uso.

3. Arquitectura propuesta

    - Frontend: React (SPA)
    - Backend: .NET 10 con Web API
    - Base de datos: PostgreSQL
    - IA:
        - Fase inicial: procesamiento básico con prompts predefinidos (sin modelo propio)
        - Fase avanzada (opcional): integración con una API de lenguaje (OpenAI o similar) para reformular preguntas o interpretar texto natural.

4. Infraestructura y despliegue

    - Contenedores Docker (API + DB + frontend)
    - Despliegue en Heroku (y local con Docker Compose)
    - Documentación en Swagger + Diagrama de arquitectura

5. Documentación esperada
    - README general del sistema
    - Documentación de arquitectura
    - Modelo de datos (ERD)
    - API Spec (OpenAPI/Swagger)
    - Historias de usuario y tickets (GitHub Issues)
    - Suite de tests (xUnit)

# Punto crítico: Base de datos

Utilizar una API pública o freemium que ya contenga las estadísticas, y almacenar solo los datos relevantes en la propia base de datos (por ejemplo, últimos partidos, jugadores destacados, etc.).

## API Propuesta

-   [API-Football](https://www.api-football.com/) (freemium, buena documentación)

## Resultado Investigación

-   API-Football es la opción más completa y con mejor documentación.
-   La versión gratuita permite 100 solicitudes diarias, suficiente para pruebas iniciales.
-   Sin embargo, se debe considerar la limitación de la API y planificar el almacenamiento local de datos clave para minimizar llamadas repetitivas.
-   Se ha confirmado que la API no tiene información histórica más allá del año 2010, por lo que se deberá considerar esto en el diseño de las funcionalidades.
-   Para obtener datos en específico, se deberá implementar un sistema de caché en la base de datos para almacenar las consultas más frecuentes y reducir la dependencia de la API externa.
