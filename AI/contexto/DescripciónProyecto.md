# Descripción

Propósito

Desarrollar un producto de software end-to-end (E2E) que cubra todo el ciclo de vida —de la idea al despliegue— apoyándose en IA en todas las fases y con criterio humano para revisar, corregir y elevar la calidad.

## Alcance del MVP

-   Dominio libre (ideal: cercano a tu trabajo o uno nuevo para aprender). Ejemplos: e-commerce tipo Zalando, neobanco tipo Revolut, transporte tipo Uber, marketplace tipo Amazon o alojamientos tipo Airbnb.
-   Define un flujo E2E prioritario que tenga principio y fin claros y que aporte valor completo (por ejemplo: registro → login → búsqueda → compra → pago → confirmación).
-   Planifica para ese flujo:
    -   3–5 historias Must-Have (imprescindibles).
    -   1–2 historias Should-Have (opcionales, pero deseables).

## Artefactos a producir

A lo largo de las tres entregas irás completando estos artefactos:

1. Documentación de producto
    - Objetivo, características y funcionalidades principales.
2. Historias de usuario y tickets de trabajo
    - Historias con criterios de aceptación claros.
    - Tickets con buena trazabilidad (qué historia, qué módulo, qué impacto).
3. Arquitectura y modelo de datos.
    - Diagrama de arquitectura del sistema.
    - Modelo de datos con entidades, relaciones y restricciones.
4. Backend
    - API o servicios con acceso a base de datos.
    - Operaciones necesarias para soportar el flujo E2E.
5. Frontend
    - Implementación usable del flujo E2E (no hace falta diseño ultra sofisticado, pero sí navegable y coherente).
6. Suite de tests
    - Tests unitarios y de integración.
    - Al menos un test E2E del flujo principal.
7. Infra y despliegue
    - Pipeline básico de CI/CD (aunque sea sencillo).
    - Gestión de secretos mínimamente cuidada.
    - URL pública accesible (o entorno accesible para el TA).
8. Registro del uso de IA
    - Prompts clave utilizados.
    - Herramientas de IA usadas (IDE, copilots, LLMs externos, etc.).
    - Ejemplos de “antes/después” y explicación de qué ajustes humanos hiciste sobre el resultado generado por IA.

## Libertad tecnológica

Puedes usar el lenguaje y stack que domines mejor:

-   Ejemplos: JavaScript/TypeScript, Java, PHP, Python, Ruby, etc.
-   Frameworks y librerías quedan a tu elección, siempre que el resultado sea:
    -   Ejecutable.
    -   Comprensible.
    -   Razonablemente documentado.

# Formato de trabajo y entrega:

Sigue los siguientes pasos en orden para garantizar una entrega completa y revisable:

-   Completa la plantilla de trabajo

    -   El archivo [text](Entregables/README.md)

        Con la ficha del proyecto, descripción general del producto, arquitectura, modelo de datos, API, historias de usuario, tickets de trabajo y pull requests, siguiendo la estructura que ya viene en la plantilla.

    -   El archivo [text](Entregables/prompts.md)

        Aquí debes documentar los prompts más relevantes que utilizaste durante la creación del proyecto.
        Para cada sección (producto, arquitectura, modelo de datos, API, etc.), incluye:

        -   Hasta 3 prompts clave.
        -   Una breve nota de cómo guiaste al asistente de código o LLM.
        -   Opcional: enlace o referencia a la conversación completa si lo consideras útil.

-   Repositorio de código

    -   El código debe estar alojado en un repositorio accesible:
        -   Puede ser público o privado.
        -   Si es privado, debes dar acceso a tu TA (por GitHub handle o correo).
    -   El proyecto debe estar desplegado en un entorno ejecutable, de forma que se pueda:
        -   Probar el flujo principal.
        -   Ver el sistema “en vivo” (aunque sea un entorno de pruebas).

-   Trabajo mediante Pull Requests

    Durante el desarrollo:

    -   Realiza los cambios mediante pull requests.
    -   Asegúrate de que cada PR:
        -   Tiene un título claro.
        -   Incluye una descripción detallada (qué cambia, por qué, impacto).
        -   Hace referencia a la historia de usuario o ticket correspondiente cuando aplique.

-   Ramas, pull requests y formulario de entrega

    -   Entrega 1 – Documentación técnica

        -   Trabaja en una rama de feature, por ejemplo: feature-entrega1-[iniciales]
        -   Entrega oficial: https://lidr.typeform.com/proyectoai4devs
            Incluye la URL del pull request de la Entrega 1.

    -   Entrega 2 – Código funcional (primer MVP ejecutable)

        -   Continúa sobre la base de tu repo y crea otra rama de feature, por ejemplo: feature-entrega2-[iniciales]
        -   Entrega oficial: https://lidr.typeform.com/proyectoai4devs
            Incluye la URL del pull request de la Entrega 2.

    -   Para la entrega definitiva:
        -   Crea una rama final con el siguiente formato: finalproject-[iniciales]
        -   En esa rama deben estar:
            -   Plantilla completa: readme.md y prompts.md
            -   Código funcional.
            -   Evidencia de despliegue:
                -   Link al entorno público, y/o
                -   Instrucciones claras o capturas del sistema funcionando.
            -   (Opcional, pero recomendado) Etiqueta de release:
                -   v1.0-final-[iniciales]

-   Envío del proyecto

    -   Sube la URL de la rama final en el formulario:
        👉 https://lidr.typeform.com/proyectoai4devs
    -   La fecha de las entregas parciales será:
        -   Documentación técnica: Entrega de la idea, estructura y diseño del proyecto, con la mayor parte de la plantilla avanzada (producto, arquitectura, modelo de datos, historias).
            -   Miércoles 17 de diciembre
        -   Código funcional: Backend, frontend y base de datos ya conectados, con el flujo principal “casi” completo.
            -   Miércoles 21 de enero
        -   Entrega final: Versión completa y desplegada del proyecto, con el flujo principal funcionando de principio a fin, tests y documentación cerrada.
            -   Martes 3 de febrero

-   ⚠️ Recordatorios importantes

    -   Si tu repositorio es privado, da acceso a tu TA.
    -   El nombre de la rama debe contener tus iniciales. De lo contrario, tu entrega no podrá ser identificada correctamente.
    -   En caso de que el proyecto sea privado, puedes incluir en la plantilla capturas del funcionamiento. Sin embargo, se recomienda anexar un video breve (2–3 minutos) explicando y mostrando el flujo principal del sistema.

## Dedicación estimada:

-   Se espera una dedicación aproximada de 30 horas en total.
-   Puedes organizar tu tiempo como prefieras, pero las tres entregas están pensadas para repartir el esfuerzo y evitar dejar todo para el final.

## Tutoría y soporte:

-   Por email con cualquier duda a jorge@lidr.co o tu TA.
-   Habrá 3 sesiones de tutoría en vivo centradas en el Proyecto Final:
-   Una al inicio (para elegir bien la idea y planificar).
-   Una a mitad (para desbloquear problemas de diseño/implementación).
-   Una cerca del cierre (para pulir detalles antes de la entrega final).
    -   3 sesiones de 1,5h en distintos horarios y días para garantizar la asistencia mínima
        -   Lunes 27 de octubre | 13:30 - 15:00 CET
        -   Miércoles 3 de diciembre | 10:30 - 12:00 CET
        -   Lunes 26 de enero | 15:30 - 17:00 CET

## Fecha de entrega final

-   Martes 3 de febrero, al final del día (hora del programa).
-   Toda la información debe estar:
    -   En la rama finalproject-[iniciales].
    -   Con el formulario de Typeform enviado.

## Extensión y retroalimentación

-   Si no llegas a la fecha de entrega final, puedes solicitar una prórroga de hasta dos semanas a partir de esa fecha.
-   La prórroga debe tramitarse directamente con el TA, quien evaluará cada caso y confirmará su aprobación.
-   No se entregará feedback individual de las dos primeras entregas (documentación técnica y código funcional), ya que su propósito es guiar la construcción progresiva del proyecto.
-   La retroalimentación completa y formal se realizará únicamente sobre la entrega final, una vez evaluado el proyecto en su conjunto.
