# Limitaciones de API-Football y Alternativas

## Resumen Ejecutivo

Este documento describe las limitaciones encontradas en API-Football para consultas avanzadas de estadísticas de jugadores, el análisis de alternativas, y las decisiones de diseño tomadas para el MVP.

---

## 1. Limitación Principal: Stats de Jugador por Home/Away

### El Problema

API-Football no ofrece un endpoint que devuelva estadísticas de jugadores individuales desglosadas por partidos home vs away. El endpoint `/players` devuelve estadísticas **agregadas por temporada**:

```json
{
	"player": { "name": "Erling Haaland" },
	"statistics": [
		{
			"games": { "appearences": 15 },
			"goals": { "total": 13 } // Sin breakdown home/away
		}
	]
}
```

### Consulta Afectada

> "Compara el rendimiento de Haaland y Mbappe en sus respectivas ligas esta temporada, pero solo contando partidos jugados fuera de casa."

### Workaround Requerido

Para responder esta consulta se necesitan múltiples llamadas:

```
Por jugador:
1. GET /players?search=Haaland           → Obtener player_id
2. GET /fixtures?team=50&season=2024     → Obtener todos los partidos (~38)
3. Filtrar fixtures donde teams.away.id == team_id  (~19 partidos)
4. GET /fixtures/players?fixture={id}    → Para cada partido away (x19)
5. Sumar goals del jugador en cada fixture

Total: ~21 llamadas por jugador
Comparación 2 jugadores: ~42 llamadas
```

---

## 2. Impacto en Planes de API-Football

### Tabla de Capacidad

| Plan  | Precio/mes | Requests/dia | Consultas complejas/dia\* | Usuarios activos\*\* |
| ----- | ---------- | ------------ | ------------------------- | -------------------- |
| Free  | $0         | 100          | 2                         | 1-2                  |
| Pro   | $20        | 7,200        | 171                       | 50-100               |
| Ultra | $29        | 75,000       | 1,785                     | 500-1,000            |

\*Consultas que requieren ~42 llamadas (comparacion jugadores away)
\*\*Asumiendo mix 70% consultas simples + 30% complejas = ~15 calls/usuario promedio

### Conclusión

Con el plan Ultra ($29/mes), el sistema podría servir ~500-1,000 usuarios activos diarios con un mix de consultas simples y complejas.

---

## 3. Alternativas Evaluadas

### Sportmonks Football API

-   **Precio:** ~$39-79/mes (plans similares)
-   **Stats home/away:** Si, pero **solo para equipos**, no jugadores individuales
-   **Ejemplo disponible:**
    ```json
    {
    	"team": "Real Madrid",
    	"goals_for": { "total": 70, "home": 40, "away": 30 },
    	"win": { "total": 26, "home": 15, "away": 11 }
    }
    ```
-   **Limitación:** Estadísticas de jugadores siguen siendo agregados por temporada

### FootyStats API

-   **Precio:** $49/mes (Pro)
-   **Stats home/away:** Si, pero **solo para equipos**
-   **Filtros disponibles:** `_overall`, `_home`, `_away` muy detallados
-   **Limitación:** No tiene endpoints de stats de jugadores individuales con breakdown home/away

### Conclusión de Alternativas

**Ninguna API del mercado ofrece estadísticas de jugadores individuales desglosadas por home/away de forma directa.** Esta es una limitación de la industria, no específica de API-Football.

---

## 4. Decisiones de Diseño para MVP

### Enfoque Adoptado

1. **Limitar consultas MVP a 5 tipos simples** (1-2 API calls cada una):

    - Máximos goleadores (`/players/topscorers`)
    - Clasificación (`/standings`)
    - Resultados recientes (`/fixtures?last=5`)
    - Próximos partidos (`/fixtures?next=1`)
    - Estadísticas de equipo (`/teams/statistics`)

2. **Modelo BYOK (Bring Your Own Key):**

    - Cada usuario aporta su API Key (plan Free: 100 req/dia)
    - Sin costos de infraestructura por llamadas a API
    - Usuarios que necesiten mas pueden pagar su propio plan

3. **Caché agresivo:**
    - Datos históricos: TTL 30 días
    - Clasificaciones actuales: TTL 1 hora
    - Reduce llamadas repetidas

### Consultas Complejas Post-MVP

Si se implementan consultas como comparaciones home/away:

1. **Sistema de créditos:** Consultas complejas consumen más créditos del usuario
2. **Cola de procesamiento:** Ejecutar consultas pesadas en background
3. **Advertencia al usuario:** Indicar que la consulta consumirá ~40 requests de su cuota
4. **Límite diario:** Máximo 2-3 consultas complejas por usuario/día en plan Free

---

## 5. Referencia Rápida

### Consultas Simples (1-2 calls) - MVP

| Consulta          | Endpoint              | Calls |
| ----------------- | --------------------- | ----- |
| Top goleadores    | `/players/topscorers` | 1     |
| Clasificación     | `/standings`          | 1     |
| Últimos partidos  | `/fixtures?last=N`    | 1     |
| Próximos partidos | `/fixtures?next=N`    | 1     |
| Stats equipo      | `/teams/statistics`   | 1     |

### Consultas Complejas (20-50 calls) - Post-MVP

| Consulta                       | Proceso                        | Calls |
| ------------------------------ | ------------------------------ | ----- |
| Stats jugador away             | fixtures + players por fixture | ~21   |
| Comparar 2 jugadores away      | x2 jugadores                   | ~42   |
| Histórico jugador 5 temporadas | x5 temporadas                  | ~105  |

---

## 6. Enlaces de Referencia

-   [API-Football Pricing](https://www.api-football.com/pricing)
-   [API-Football Documentation v3](https://www.api-football.com/documentation-v3)
-   [Sportmonks Football API](https://www.sportmonks.com/football-api/)
-   [FootyStats API](https://footystats.org/api/)

---

**Última actualización:** 2025-12-08
**Autor:** Documentado durante fase de análisis de arquitectura
