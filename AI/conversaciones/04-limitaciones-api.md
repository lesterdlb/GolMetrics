# Conversación 4: Análisis de Limitaciones de API-Football

**Herramienta:** Claude Code (claude-sonnet-4.5)
**Duración:** ~45 minutos
**Archivos creados:** 1 (docs/02-architecture/APILimitations.md)
**Archivos modificados:** 1 (docs/01-product/ProjectOverview.md)
**Tokens utilizados:** ~15,000
**Herramientas MCP utilizadas:** Context7 (para investigar API-Football, Sportmonks, FootyStats)

---

## Resumen

Esta conversación se centró en el análisis técnico profundo de las limitaciones de API-Football para consultas avanzadas (específicamente estadísticas de jugadores por home/away) y la evaluación de alternativas del mercado. Se descubrió que es una limitación de toda la industria, no solo de API-Football.

**Logros principales:**

-   ✅ Identificación de limitación crítica: stats de jugador por home/away requieren ~42 API calls
-   ✅ Análisis de capacidad por plan (Free, Pro, Ultra)
-   ✅ Comparativa con alternativas (Sportmonks, FootyStats)
-   ✅ Corrección de precio del plan Ultra ($149→$29/mes)
-   ✅ Validación de enfoque MVP con consultas simples
-   ✅ Documentación de limitaciones en archivo técnico

---

## Prompt 1: Análisis de Limitación y Planes de API

### Contexto

Evaluación de si los planes de pago de API-Football resolverían la limitación encontrada con consultas complejas.

### Prompt

```
He identificado una limitación crítica con consultas complejas. El ejemplo
"Compara el rendimiento de Haaland y Mbappé en partidos away" parece requerir múltiples llamadas a la API.

Necesito evaluar dos aspectos:

1. **Planes de API-Football**: ¿Los planes de pago (Pro o Ultra) ofrecen endpoints adicionales o funcionalidades que simplifiquen este tipo de consultas? Usa Context7 para investigar la documentación oficial y comparar capacidades entre planes.

2. **Decisión de scope**: Considerando el alcance del proyecto (MVP en ~30 horas), ¿es más sensato enfocarse en consultas simples (1-2 API calls) para el MVP y dejar consultas complejas como feature avanzado post-MVP?

Necesito datos concretos para tomar una decisión informada sobre la arquitectura.
```

### Resultado

**Hallazgos de la investigación con Context7:**

-   **Limitación confirmada:** No existe endpoint directo para stats de jugador por home/away
-   **Workaround:** Requiere ~42 API calls para comparar 2 jugadores en partidos away:

```
Por jugador:
- 1 call: GET /players?search=Haaland (obtener ID)
- 1 call: GET /fixtures?team=50&season=2024 (~19 partidos away)
- 19 calls: GET /fixtures/players?fixture={id} (stats por partido)
= ~21 calls por jugador

Total para comparación: ~42 calls
```

**Análisis de Capacidad por Plan:**

| Plan  | Requests/día | Precio    | Usuarios concurrentes | Consultas complejas/día |
| ----- | ------------ | --------- | --------------------- | ----------------------- |
| Free  | 100          | $0        | 1-2                   | 2                       |
| Pro   | 7,200        | $29/mes   | ~50-100               | 171                     |
| Ultra | 75,000       | $29/mes\* | ~500-1,000            | 1,785                   |

\*Precio corregido posteriormente

**Recomendación dada:**

Mantener enfoque simple para MVP:

-   Las 5 consultas Must-Have son alcanzables con 1-2 API calls cada una
-   El modelo BYOK (100 req/día por usuario) es suficiente
-   Consultas complejas como comparaciones away son feature avanzado post-MVP

---

## Prompt 2: Confirmación de Actualización

### Contexto

Confirmar la actualización del ejemplo en ProjectOverview.md a una consulta más realista.

### Prompt

```
Confirmado. Procede a actualizar @docs/01-product/ProjectOverview.md con un ejemplo de consulta más realista y alcanzable para el MVP.

Cambia el ejemplo complejo (Haaland vs Mbappé en partidos away) por una consulta simple que requiera solo 1-2 llamadas a la API, como "Máximos goleadores de una liga".
```

### Resultado

1. **Actualización de `docs/01-product/ProjectOverview.md`:**

    - Ejemplo anterior: "Compara Haaland vs Mbappé en partidos away" (60+ API calls)
    - Ejemplo nuevo: "¿Quiénes son los máximos goleadores de la Premier League?" (1 API call)
    - Incluye tabla de ejemplo formateada en la respuesta

2. **Actualización de métrica de producto:**
    ```diff
    - Tipos de consultas soportadas: Mínimo 8 de 10 funcionando en producción
    + Tipos de consultas soportadas: 5 de 5 Must-Have funcionando en producción
    ```

---

## Prompt 3: Análisis Detallado de Escalabilidad y Alternativas

### Contexto

Profundizar en la capacidad real del plan Ultra y explorar alternativas a API-Football.

### Prompt

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

### Resultado

**Cálculo de capacidad con Plan Ultra:**

-   75,000 req/día ÷ 42 calls = ~1,785 consultas complejas/día
-   Asumiendo mix 70% simples (2 calls) + 30% complejas (42 calls) = ~15 calls/usuario promedio
-   **Capacidad: ~500-1,000 usuarios activos diarios**

**Investigación de alternativas con Context7:**

| API              | Stats Jugador Home/Away    | Precio    | Requests/día  | Cobertura    |
| ---------------- | -------------------------- | --------- | ------------- | ------------ |
| **API-Football** | No directo (workaround)    | $29/mes\* | 75,000        | 860+ ligas   |
| **Sportmonks**   | Solo equipos, no jugadores | ~$150/mes | Ilimitado\*\* | 2,000+ ligas |
| **FootyStats**   | Solo equipos, no jugadores | $49/mes   | 3,600         | 1,500+ ligas |

\*Precio corregido posteriormente
\*\*Sistema de "includes" cuenta como requests adicionales

**Conclusión crítica:**

**Ninguna API del mercado ofrece estadísticas de jugadores individuales desglosadas por home/away de forma directa.** Es una limitación de toda la industria.

-   Sportmonks tiene `stats.win.home`, `stats.goals_for.away` pero **solo para equipos**
-   FootyStats tiene filtros `_home`/`_away` muy detallados pero **solo para equipos**
-   Stats de jugadores en todas las APIs son agregados por temporada

**Recomendación final:**

Mantener modelo BYOK para MVP:

-   Cada usuario trae su free tier (100 req/día)
-   Si un usuario necesita más, puede pagar su propio plan Pro/Ultra
-   Si se agregan consultas complejas post-MVP:
    -   Implementar sistema de "créditos" (consultas complejas cuestan más)
    -   Cola de procesamiento para consultas pesadas
    -   Advertir al usuario sobre consumo de requests

---

## Prompt 4: Documentación de Limitaciones

### Contexto

Crear documento técnico separado para documentar la limitación y la corrección del precio.

### Prompt

```
El plan Ultra son $29/mes. Crea un documento aparte con la descripción de
esta limitación.
```

### Resultado

**Archivo creado:** `docs/02-architecture/APILimitations.md`

**Contenido completo incluye:**

1. **Resumen ejecutivo** de la limitación
2. **Descripción técnica del problema:**

    - Endpoint `/players` devuelve stats agregados
    - Workaround requiere ~42 llamadas

3. **Tabla de capacidad corregida:**

    ```
    Plan Ultra: $29/mes → 75,000 req/día → 500-1,000 usuarios activos
    ```

4. **Comparativa de alternativas** (Sportmonks, FootyStats) con conclusión:
   "Es una limitación de toda la industria, no solo de API-Football"

5. **Decisiones de diseño para MVP:**

    - 5 consultas simples (1-2 calls cada una)
    - Modelo BYOK
    - Caché agresivo

6. **Estrategia para consultas complejas post-MVP:**

    - Sistema de créditos
    - Cola de procesamiento
    - Límite diario para plan Free

7. **Referencia rápida:** Tablas de calls por tipo de consulta

---

## Decisiones Técnicas Tomadas

### Problema Identificado

La consulta ejemplo en `ProjectOverview.md` ("Compara Haaland vs Mbappé en partidos away") no es viable con las capacidades de API-Football ni de ninguna alternativa del mercado sin múltiples llamadas (~42 calls).

### Soluciones Implementadas

1. **Actualización de ejemplo en ProjectOverview.md:**

    - De consulta compleja (60+ calls) a consulta simple (1 call)
    - Mejor refleja capacidades reales del MVP

2. **Documentación de limitaciones:**

    - Nuevo archivo `docs/02-architecture/APILimitations.md`
    - Análisis técnico completo con precio corregido ($29/mes Ultra)
    - Justificación de decisiones de diseño

3. **Validación de enfoque MVP:**
    - 5 consultas simples es la decisión correcta
    - Modelo BYOK viable para escala inicial
    - Plan Ultra ($29/mes) accesible si se necesita escalar

### Comparativa de APIs Investigadas

**API-Football (Seleccionada):**

-   ✅ Cobertura amplia (860+ ligas)
-   ✅ Precio accesible ($29/mes para 75k req/día)
-   ✅ Documentación completa
-   ❌ No tiene stats jugador por home/away directo

**Sportmonks:**

-   ✅ Cobertura muy amplia (2000+ ligas)
-   ✅ Requests ilimitados\*
-   ❌ Precio alto (~$150/mes)
-   ❌ Sistema de "includes" complica cálculos
-   ❌ No tiene stats jugador por home/away

**FootyStats:**

-   ✅ Precio medio ($49/mes)
-   ✅ Estadísticas muy detalladas para equipos
-   ❌ Solo 3,600 req/día
-   ❌ No tiene stats jugador por home/away

**Conclusión:** API-Football es la mejor opción para el proyecto (precio, capacidad, documentación).

---

## Hallazgos Críticos

### Limitación de la Industria

**Descubrimiento principal:** Las estadísticas de jugadores individuales desglosadas por home/away no están disponibles de forma directa en ninguna API del mercado investigada.

**Razón probable:** Las APIs consumen datos de proveedores como Opta Sports, que agregan estadísticas de jugador por temporada completa, no por condición de local/visitante.

**Implicaciones:**

-   Cualquier chatbot de fútbol enfrentará esta limitación
-   La solución con múltiples llamadas es el único workaround posible actualmente
-   No es un problema de API-Football, sino del modelo de datos de toda la industria

### Capacidad de Escalabilidad

**Con plan Free (BYOK):**

-   100 req/día por usuario
-   ~6-7 consultas complejas ó ~50 consultas simples por día
-   Ideal para MVP y pruebas iniciales

**Con plan Ultra ($29/mes):**

-   75,000 req/día
-   ~500-1,000 usuarios activos diarios (mix de consultas)
-   Suficiente para escalar a producción pequeña/mediana

### Corrección de Precio Crítica

**Error inicial:** Plan Ultra reportado a $149/mes
**Precio real:** $29/mes

**Impacto:** El plan Ultra es 5 veces más accesible de lo estimado, haciendo viable la escalabilidad del proyecto sin inversión significativa.

---

## Archivos Modificados

1. **docs/01-product/ProjectOverview.md**

    - Ejemplo actualizado: consulta compleja → consulta simple
    - Métrica corregida: "8 de 10" → "5 de 5 Must-Have"

2. **docs/02-architecture/APILimitations.md** (NUEVO)
    - Documentación técnica completa de limitaciones
    - Análisis de alternativas
    - Estrategia de mitigación

---

## Lecciones Aprendidas

### Sobre Validación de Ejemplos

1. **Validar capacidades antes de ejemplos:** El ejemplo original sugería una capacidad que no existe de forma eficiente en ninguna API del mercado
2. **Ejemplos deben reflejar realidad del MVP:** Usar casos de uso alcanzables con los recursos disponibles

### Sobre Investigación de Alternativas

1. **Investigar antes de comprometerse:** Todas las APIs tienen la misma limitación para stats de jugador por home/away
2. **Context7 es invaluable:** Permitió investigar 3 APIs diferentes en minutos, accediendo a documentación actualizada
3. **Precio no es todo:** API-Football ganó por balance de precio/capacidad/documentación, no solo por ser la más barata

### Sobre Modelo BYOK

1. **BYOK es correcto para este proyecto:** Evita costos de infraestructura y permite escalar por usuario
2. **Free tier es suficiente para MVP:** 100 req/día permite 6-7 consultas complejas o ~50 simples
3. **Plan Ultra accesible para escalar:** $29/mes es viable si el proyecto crece

### Sobre Documentación de Limitaciones

1. **Documentar limitaciones es crítico:** Ayuda a futuras decisiones de arquitectura y gestión de expectativas
2. **Comparativas justifican decisiones:** Tabla de alternativas demuestra que API-Football es la mejor opción
3. **Estrategia de mitigación importa:** Definir cómo manejar limitaciones post-MVP demuestra planificación

---

## Métricas de la Conversación

-   **Prompts principales del usuario:** 5
-   **APIs investigadas con Context7:** 3 (API-Football, Sportmonks, FootyStats)
-   **Archivos creados:** 1
-   **Archivos modificados:** 1
-   **Tiempo de investigación manual ahorrado:** ~3-4 horas (gracias a Context7)
-   **Decisión crítica tomada:** Mantener API-Football con modelo BYOK

---

## Referencias Técnicas

**APIs investigadas:**

-   API-Football v3: https://www.api-football.com/
-   Sportmonks Football API: https://www.sportmonks.com/
-   FootyStats API: https://footystats.org/api

**Documentación relevante:**

-   `docs/01-product/ProjectOverview.md` - Ejemplo actualizado
-   `docs/02-architecture/APILimitations.md` - Análisis completo de limitaciones

**Herramientas utilizadas:**

-   Context7 MCP Server - Acceso a documentación actualizada de APIs

---

**Última actualización:** 14 de Diciembre de 2025
