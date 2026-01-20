# UI Design - GolMetrics

## 1. Paleta de Colores

```css
:root {
	--primary: #3b82f6; /* Blue 500 */
	--secondary: #64748b; /* Slate 500 */
	--success: #10b981; /* Green 500 */
	--danger: #ef4444; /* Red 500 */
	--background: #ffffff;
	--surface: #f8fafc; /* Slate 50 */
	--text-primary: #0f172a; /* Slate 900 */
	--text-secondary: #64748b;
}
```

---

## 2. Wireframes

### Login Page

```
┌─────────────────────────────────────┐
│                                     │
│         ⚽ GolMetrics                │
│                                     │
│   ┌─────────────────────────────┐   │
│   │  Email                      │   │
│   └─────────────────────────────┘   │
│                                     │
│   ┌─────────────────────────────┐   │
│   │  Password                   │   │
│   └─────────────────────────────┘   │
│                                     │
│         [  Iniciar Sesión  ]        │
│                                     │
│     ¿No tienes cuenta? Regístrate   │
│                                     │
└─────────────────────────────────────┘
```

### Chat Page

```
┌─────────────────────────────────────────────────────────┐
│  ⚽ GolMetrics              [Configuración] [Cerrar]     │
├──────────────┬──────────────────────────────────────────┤
│              │                                          │
│ Conversaciones│  Chat: Goleadores Premier 2024         │
│              │  ┌────────────────────────────────────┐ │
│ • Goleadores │  │ User: ¿Goleadores Premier 2024?   │ │
│   Premier    │  └────────────────────────────────────┘ │
│              │                                          │
│ • Clasificación│ ┌────────────────────────────────────┐│
│   La Liga    │  │ AI: **Máximos goleadores...**      ││
│              │  │ | # | Jugador | Equipo | Goles |    ││
│ [+ Nueva]    │  │ |---|---------|--------|-------|    ││
│              │  │ | 1 | Haaland | City   | 27    |    ││
│              │  └────────────────────────────────────┘ │
│              │                                          │
│              │  ┌────────────────────────────────────┐ │
│              │  │ Escribe tu pregunta...         [>] │ │
│              │  └────────────────────────────────────┘ │
└──────────────┴──────────────────────────────────────────┘
```

---

## 3. Componentes shadcn/ui

```bash
# Instalación de componentes
npx shadcn-ui@latest init
npx shadcn-ui@latest add button
npx shadcn-ui@latest add input
npx shadcn-ui@latest add textarea
npx shadcn-ui@latest add card
npx shadcn-ui@latest add avatar
npx shadcn-ui@latest add scroll-area
```

---

## 4. Estados de la UI

| Estado  | Descripción         | Indicador Visual                        |
| ------- | ------------------- | --------------------------------------- |
| Loading | Procesando mensaje  | Skeleton + "Escribiendo..."             |
| Error   | Fallo en API        | Toast rojo con mensaje                  |
| Empty   | Sin conversaciones  | Ilustración + "Inicia una conversación" |
| Typing  | Usuario escribiendo | Contador de caracteres                  |

---

**Última actualización:** 2025-10-10
