# Visual Design Prompts for "Visión de Juego" Football Stats Chatbot

This document contains detailed design prompts for three different visual concepts for the chatbot interface. Each prompt is available in both Spanish (original) and English.

---

## Table of Contents

### Spanish Version (Original)

1. [Propuesta A: Pizarra Táctica del Entrenador](#propuesta-a-pizarra-táctica-del-entrenador-español)
2. [Propuesta B: Estadio de Noche](#propuesta-b-estadio-de-noche-español)
3. [Propuesta C: Cromos/Trading Cards](#propuesta-c-cromostrading-cards-español)

### English Version

1. [Proposal A: Coach's Tactical Board](#proposal-a-coachs-tactical-board-english)
2. [Proposal B: Night Stadium](#proposal-b-night-stadium-english)
3. [Proposal C: Trading Cards/Collectible Cards](#proposal-c-trading-cardscollectible-cards-english)

---

# SPANISH VERSION / VERSIÓN EN ESPAÑOL

---

## Propuesta A: Pizarra Táctica del Entrenador (Español)

### Contexto del Proyecto

Estoy diseñando la interfaz de usuario para un chatbot de estadísticas de fútbol llamado "Visión de Juego". Es una aplicación web conversacional donde usuarios hacen preguntas sobre estadísticas de fútbol (goleadores, clasificaciones, historial de partidos) y reciben respuestas inteligentes.

### Objetivo del Diseño

Necesito un sistema de diseño visual completo que transforme la interfaz de chat en una experiencia que simule la **pizarra táctica de un entrenador de fútbol**. El objetivo es crear una estética diferencial, nostálgica pero moderna, que haga que consultar estadísticas se sienta como estar en el vestuario recibiendo análisis táctico del entrenador.

### Concepto Visual Principal

La interfaz debe evocar una pizarra táctica física donde los entrenadores dibujan jugadas con tiza. Imagina las clásicas pizarras verdes/negras con diagramas de fútbol, flechas curvas indicando movimientos, círculos representando jugadores, y escritura manual con tiza blanca.

### Elementos Visuales a Diseñar

#### 1. Fondo Principal

-   Textura de pizarra verde oscuro o negro mate (no brillante)
-   Debe tener imperfecciones sutiles: marcas de borrado previo, pequeñas rayas, textura granulada
-   Ligero efecto de polvo de tiza en los bordes
-   No debe ser plano: necesita profundidad táctil como una pizarra real

#### 2. Mensajes del Usuario

-   Contenedor: Rectángulo con esquinas ligeramente redondeadas
-   Borde: Blanco tiza, 2-3px de grosor, con ligera irregularidad (simulando trazos de tiza)
-   Fondo: Semi-transparente oscuro para contraste con el texto
-   Texto: Color blanco tiza con ligera opacidad (95%)
-   Tipografía: Sans-serif limpia pero con carácter manuscrito sutil (ej: Caveat, Patrick Hand, o similar)

#### 3. Mensajes del Chatbot

-   Contenedor: Óvalos o círculos irregulares (como jugadores en un diagrama táctico)
-   Para estadísticas complejas: Rectángulos con esquinas curvas conteniendo "sub-jugadores" (círculos más pequeños)
-   Borde: Tiza blanca con trazos que parecen hechos a mano
-   Elementos decorativos:
    -   Flechas curvas conectando datos relacionados
    -   Líneas punteadas indicando "movimientos"
    -   Pequeños números dentro de círculos para listas ordenadas
-   Efecto: Sombra sutil tipo tiza difuminada

#### 4. Iconografía y Elementos Decorativos

-   Iconos dibujados en estilo "tiza sobre pizarra":
    -   Balón de fútbol (círculo con pentágonos hexagonales simplificados)
    -   Silbato del árbitro para notificaciones
    -   Cronómetro para información temporal
    -   Tarjetas amarillas/rojas para destacar datos importantes
-   Separadores: Líneas del campo de fútbol (línea de medio campo, áreas, etc.)
-   Header: Diagrama táctico simplificado (4-4-2, 4-3-3) como decoración sutil

#### 5. Animaciones

-   Aparición de mensajes: Efecto de "dibujo progresivo" como si se escribiera con tiza
-   Transiciones: Ligero efecto de "borrado y redibujo"
-   Hover: Los elementos se "iluminan" sutilmente como si alguien pasara la mano sobre la tiza
-   Carga: Animación de balón rebotando dibujado con tiza

### Paleta de Colores Exacta

-   **Pizarra (fondo)**: #1a3d2e (verde pizarra oscuro) o #2c2c2c (negro pizarra)
-   **Tiza principal (texto/bordes)**: #f5f5f5 (blanco tiza ligeramente apagado)
-   **Tiza amarilla (highlights)**: #ffd700 con 85% opacidad
-   **Tiza naranja (warnings/importante)**: #ff8c42
-   **Tiza roja (errores/tarjetas)**: #e63946
-   **Sombra de tiza**: #ffffff con 15% opacidad y blur de 8-12px

### Referencias Visuales (Describe o busca)

-   Pizarras tácticas de entrenadores de fútbol profesionales
-   Diagramas tácticos de revistas deportivas como FourFourTwo
-   Análisis tácticos de Michael Cox o Jonathan Wilson
-   Videojuegos: Screens de tácticas en FIFA/Football Manager
-   Apps como TacticalPad o Touchtight

### Output Esperado

Por favor, genera:

1. **Mockup de la interfaz completa** mostrando:

    - Vista de conversación con 3-4 mensajes intercalados
    - Un mensaje del usuario preguntando "¿Quiénes son los máximos goleadores de La Liga 2024?"
    - Respuesta del chatbot mostrando una lista de 3 jugadores con estadísticas (nombre, equipo, goles)

2. **Paleta de colores visual** con los HEX codes y muestras

3. **Ejemplos de iconografía** (balón, silbato, cronómetro, tarjetas) en estilo tiza

4. **Variaciones de estados**:

    - Mensaje en estado normal
    - Mensaje en hover
    - Mensaje con información destacada (ej: récord histórico)

5. **Especificaciones tipográficas**:
    - Fuente recomendada con tamaños (heading, body, caption)
    - Line-height y letter-spacing

### Restricciones y Consideraciones

-   La interfaz debe ser legible en monitores normales (no requiere zoom)
-   El contraste debe cumplir WCAG AA para accesibilidad
-   No debe parecer infantil o caricaturesca (público objetivo: 18-50 años)
-   Debe funcionar tanto en modo desktop como mobile (responsive)
-   Evitar saturación visual: la pizarra no debe estar "sobrecargada"
-   El efecto tiza debe ser sutil, no exagerado (evitar que parezca un tablero escolar)

### Tono y Personalidad

-   Nostálgico pero moderno
-   Profesional pero accesible
-   Táctico e inteligente
-   Pasión contenida (no fanático excesivo)

---

## Propuesta B: Estadio de Noche (Español)

### Contexto del Proyecto

Estoy diseñando la interfaz de usuario para un chatbot de estadísticas de fútbol llamado "Visión de Juego". Es una aplicación web donde usuarios consultan datos sobre partidos, jugadores, clasificaciones y estadísticas históricas mediante conversación natural.

### Objetivo del Diseño

Necesito un sistema de diseño visual completo que sumerja al usuario en la experiencia de estar en un **estadio de fútbol durante un partido nocturno**. La interfaz debe capturar la atmósfera cinematográfica de las noches de Champions League: luces de reflectores, césped perfectamente iluminado, pantallas LED mostrando estadísticas, y el cielo nocturno sobre el estadio.

### Concepto Visual Principal

Imagina estar sentado en un estadio moderno de noche. Ves el campo verde brillante bajo los reflectores, el cielo oscuro arriba, las pantallas electrónicas mostrando datos en tiempo real, y destellos ocasionales de cámaras fotográficas. La interfaz debe replicar esta atmósfera: oscura, elegante, con toques de neón/LED, y una sensación de "evento importante en vivo".

### Elementos Visuales a Diseñar

#### 1. Fondo Principal

-   **Capa superior**: Degradado vertical de cielo nocturno
    -   Arriba: Azul noche muy oscuro casi negro (#0a1628)
    -   Medio: Transición suave a azul medio (#1a2f4a)
    -   Abajo (20% inferior): Verde césped (#2d5016) con textura sutil de pasto
-   **Efectos atmosféricos**:
    -   Partículas sutiles flotando (polvo/humedad del estadio)
    -   Brillo difuso en la parte superior simulando luces de reflectores
    -   Opcional: Siluetas muy sutiles de gradas en los extremos laterales (opacidad 5-10%)

#### 2. Contenedor Principal del Chat

-   Panel semi-transparente flotante sobre el fondo
-   Fondo: Negro (#000000) con 70-80% opacidad y efecto de vidrio esmerilado (backdrop-filter blur)
-   Borde: Sutil brillo tipo neón blanco/azul (#4a9eff) de 1px con glow de 4-6px
-   Esquinas: Redondeadas (border-radius: 16-20px)
-   Sombra: Profunda y suave simulando que el panel "flota" sobre el estadio

#### 3. Mensajes del Usuario

-   Contenedor: Burbujas rectangulares con esquinas redondeadas
-   Fondo: Degradado sutil de gris oscuro (#2c3e50 a #34495e)
-   Borde: Línea fina blanca (#ffffff) con 30% opacidad
-   Texto: Blanco LED brillante (#ffffff)
-   Posición: Alineados a la derecha
-   Efecto: Sombra interna muy sutil

#### 4. Mensajes del Chatbot

-   Contenedor: Panel tipo "pantalla de estadísticas del estadio"
-   Fondo: Negro profundo (#0d0d0d) con borde tipo LED:
    -   Borde superior: Degradado amarillo-dorado (#ffdd57 a #ffa500)
    -   Brillo: Glow effect de 8-10px simulando LEDs encendidos
-   Para estadísticas numéricas grandes:
    -   Números en fuente tipo "marcador digital" (monoespaciada, bold)
    -   Tamaño grande (36-48px para números principales)
    -   Color: Blanco LED brillante con ligero glow
-   Separadores: Líneas horizontales finas simulando filas de LEDs

#### 5. Elementos Decorativos y Detalles

##### Líneas del Campo

-   En el fondo o como separadores entre secciones
-   Blanco brillante (#ffffff) con opacidad 40%
-   Simulando líneas pintadas del campo de fútbol
-   Usar como divisores sutiles en la interfaz

##### Efectos de Reflectores

-   Conos de luz sutiles desde las esquinas superiores
-   Degradado radial desde puntos específicos
-   Color: Blanco cálido (#fff8e7) con opacidad muy baja (5-10%)
-   Debe ser apenas perceptible, no obvio

##### Efecto "Flash de Cámaras"

-   Cuando aparece una estadística importante o récord
-   Breve destello blanco (200-300ms) en el borde del mensaje
-   Partículas de luz pequeñas dispersándose
-   Usar con moderación (solo para highlights)

##### Iconografía

-   Iconos estilo LED/neón con glow effect:
    -   Balón: Círculo con brillo dorado
    -   Estadio: Silueta simple con reflectores
    -   Cronómetro: Reloj digital LED
    -   Trofeo: Silueta brillante
-   Color: Blanco/dorado con glow

#### 6. Animaciones

##### Aparición de Mensajes

-   Fade in desde abajo (translateY: 20px → 0)
-   Duración: 400-500ms
-   Easing: ease-out suave
-   Para mensajes con estadísticas: los números "cuentan" desde 0 hasta el valor final

##### Indicador de Escritura

-   Tres puntos parpadeando en estilo LED
-   Color que pulsa entre amarillo brillante y amarillo apagado
-   Ritmo: 800ms por ciclo

##### Scroll

-   Smooth scroll con momentum
-   Al llegar al final: ligero efecto de rebote

##### Hover/Interacción

-   Los elementos interactivos aumentan ligeramente su glow
-   Transición suave (200ms)
-   Cursor cambia a pointer con trail de luz sutil

### Paleta de Colores Exacta

**Fondo:**

-   Cielo nocturno superior: #0a1628
-   Cielo nocturno medio: #1a2f4a
-   Césped inferior: #2d5016

**Contenedores:**

-   Panel principal: #000000 (80% opacidad)
-   Mensajes usuario: #2c3e50 a #34495e
-   Mensajes chatbot: #0d0d0d

**Acentos:**

-   LED blanco: #ffffff
-   LED amarillo/dorado: #ffdd57 a #ffa500
-   Brillo reflector: #fff8e7 (muy baja opacidad)
-   Neón azul: #4a9eff

**Efectos:**

-   Glow principal: #ffffff con blur 8-12px
-   Glow dorado: #ffd700 con blur 10-15px
-   Sombras: #000000 con 40-60% opacidad

### Referencias Visuales

**Buscar inspiración en:**

-   Partidos nocturnos de Champions League (UEFA)
-   Estadios modernos: Allianz Arena, Wanda Metropolitano, Tottenham Hotspur Stadium
-   Cinematografía de películas de fútbol (Goal!, United)
-   Pantallas LED de estadios (scoreboards, advertising boards)
-   Apps deportivas: OneFootball (modo oscuro), FotMob, Sofascore
-   Diseño UI: Behance/Dribbble buscar "dark sports app" o "stadium UI"

### Output Esperado

Por favor, genera:

1. **Mockup completo de la interfaz** mostrando:

    - Vista de conversación con 4-5 mensajes
    - Pregunta del usuario: "¿Cuántos goles lleva Lewandowski esta temporada?"
    - Respuesta del chatbot con estadística destacada: "32 goles" en grande, más desglose (Liga: 25, Champions: 7)
    - Fondo con el degradado cielo nocturno → césped

2. **Detalles de componentes individuales**:

    - Burbuja de mensaje de usuario (normal y hover)
    - Panel de estadística del chatbot con números grandes
    - Indicador de escritura ("escribiendo...")
    - Header/navbar con el nombre "Visión de Juego"

3. **Paleta de colores visual** con muestras y códigos HEX

4. **Especificaciones de efectos luminosos**:

    - Valores exactos de glow (blur, spread, color)
    - Opacidades recomendadas
    - Degradados con puntos de parada

5. **Guía tipográfica**:

    - Fuente para texto general (legibilidad en oscuro)
    - Fuente para números grandes (estilo marcador digital)
    - Tamaños, weights, line-heights

6. **Estados especiales**:
    - Mensaje con "récord histórico" (efecto flash de cámaras)
    - Error/mensaje vacío
    - Cargando datos

### Restricciones y Consideraciones

-   **Legibilidad**: Alto contraste entre texto y fondo (WCAG AAA si es posible)
-   **Performance**: Los efectos de blur y glow no deben afectar rendimiento
-   **Responsive**: El diseño debe adaptarse a mobile sin perder la atmósfera
-   **No saturar**: Los efectos luminosos deben ser sutiles; evitar aspecto "discoteca"
-   **Profesional**: Debe verse premium, no como un juego casual
-   **Edad objetivo**: Público adulto (18-55 años), evitar estética muy juvenil

### Tono y Personalidad

-   Cinematográfico y épico (como una final de Champions)
-   Moderno y premium
-   Elegante sin ser ostentoso
-   Emocionante pero contenido
-   Tecnológico y futurista sin ser sci-fi

---

## Propuesta C: Cromos/Trading Cards (Español)

### Contexto del Proyecto

Estoy diseñando la interfaz de usuario para un chatbot de estadísticas de fútbol llamado "Visión de Juego". Los usuarios hacen preguntas sobre jugadores, equipos y competiciones, y el chatbot responde con datos estadísticos. Quiero que la experiencia sea coleccionable, nostálgica y visualmente rica.

### Objetivo del Diseño

Necesito un sistema de diseño visual que presente las estadísticas como **cromos de fútbol coleccionables** (estilo álbum Panini). Cada respuesta del chatbot debe sentirse como abrir un sobre de cromos y descubrir información valiosa presentada en un formato icónico y familiar para los aficionados al fútbol.

### Concepto Visual Principal

Imagina los cromos tradicionales de fútbol: rectángulos de cartón con foto del jugador, escudo del equipo, estadísticas clave, y detalles en los bordes (dorados para especiales, plateados para raros). La interfaz debe transformar datos fríos en "cartas coleccionables" visualmente atractivas, con jerarquía clara y detalles que homenajean los álbumes físicos.

### Elementos Visuales a Diseñar

#### 1. Fondo Principal de la Aplicación

-   Color base: Gris carbón oscuro (#2c2c2c) o negro suave (#1a1a1a)
-   Textura sutil: Trama de papel reciclado o cartón fino (opacidad 5-8%)
-   Efecto opcional: Patrón repetitivo muy sutil de escudos de equipos en watermark (opacidad 3%)
-   Gradiente: Muy ligero desde el centro (más claro) hacia los bordes (más oscuro)

#### 2. Mensajes del Usuario

-   Estilo: Burbujas simples y discretas para no competir con las cards
-   Contenedor: Rectángulo con esquinas redondeadas (12-16px)
-   Fondo: Gris oscuro semi-transparente (#3a3a3a con 85% opacidad)
-   Borde: Sutil, 1px sólido gris medio (#5a5a5a)
-   Texto: Blanco suave (#f0f0f0)
-   Posición: Alineado a la derecha
-   Sin decoración excesiva (el foco visual debe estar en las cards del chatbot)

#### 3. Mensajes del Chatbot (Cards/Cromos)

##### Card Estructura General

Cada respuesta estadística se presenta como una o varias cards de cromo:

**Dimensiones:**

-   Proporción: Similar a cromos reales (aprox. 2.5:3.5 ratio)
-   Width: 280-320px en desktop
-   Padding interno: 16-20px

**Elementos de la Card:**

###### Header del Cromo

-   **Franja superior** con el nombre de la liga/competición
    -   Fondo: Degradado según la competición (ej: Champions = azul oscuro a azul claro)
    -   Texto: Blanco, uppercase, fuente condensada, bold
    -   Iconos: Logo de la competición pequeño

###### Foto/Visual Principal

-   Para jugadores: Foto recortada del jugador
    -   Fondo: Degradado de color del equipo
    -   Posición: Centrada o en pose dinámica
-   Para equipos: Escudo grande y limpio
-   Para estadísticas generales: Iconografía relacionada (balón, trofeo, etc.)

###### Información Central

-   **Nombre del Jugador/Equipo**: Tipografía bold, grande (24-28px)
    -   Color: Según nivel de rareza (ver abajo)
    -   Efecto: Sombra sutil para resaltar
-   **Posición/Rol**: Texto pequeño arriba del nombre (ej: "Delantero", "Mediocampista")

###### Estadísticas Clave

Layout tipo tabla/grid:

-   **Números grandes**: 32-40px, bold, color dorado/plateado según importancia
-   **Labels**: 12-14px, uppercase, gris claro
-   Ejemplo layout:
    ```
    GOLES        ASISTENCIAS      PARTIDOS
      32             15              28
    ```
-   Separadores: Líneas verticales finas entre columnas

###### Footer del Cromo

-   Año/Temporada: Pequeño, abajo a la izquierda
-   Número de card ficticio: "045/500" abajo a la derecha (añade exclusividad)

##### Sistema de Rareza (Afecta estilo de borde y efectos)

**Common (Común)**

-   Borde: Plateado mate (#c0c0c0)
-   Grosor: 2px sólido
-   Sin efectos especiales

**Rare (Raro)**

-   Borde: Dorado (#d4af37)
-   Grosor: 3px con ligero bisel
-   Efecto: Sutil glow dorado (4-6px blur)

**Legendary (Legendario) - Para récords/históricos**

-   Borde: Gradiente arcoíris holográfico (#ff6b6b → #4ecdc4 → #ffe66d → #ff6b6b)
-   Grosor: 4px
-   Efecto: Glow iridiscente animado
-   Background: Degradado sutil con shimmer effect
-   Partículas: Pequeñas estrellas/brillos flotando

##### Card Multiple (Respuestas con varias estadísticas)

-   Layout: Grid de 2-3 cards lado a lado (desktop) o stack vertical (mobile)
-   Separación: 16-20px entre cards
-   Animación: Aparecen en cascada (stagger de 150ms entre cada una)

#### 4. Animaciones de Cards

##### Aparición (Flip Animation)

La card "se voltea" como si descubrieras un cromo:

-   Estado inicial: Card vista de dorso (genérica, logo de la app)
-   Animación: Rotación 3D en eje Y (0deg → 180deg)
-   Duración: 600-800ms
-   Easing: cubic-bezier para efecto realista
-   Mientras se voltea: Ligero efecto de "brillo holográfico" pasando por la superficie

##### Hover

-   Card se eleva ligeramente (translateY: -8px)
-   Sombra se intensifica y expande
-   Borde aumenta su glow
-   Duración: 250ms
-   Para Legendary: El efecto holográfico se acelera

##### Click/Tap (Acción opcional: "Guardar estadística")

-   Card hace un pequeño "salto" (bounce animation)
-   Sonido opcional tipo "clic" de álbum
-   Feedback visual: Checkmark aparece brevemente

#### 5. Iconografía y Elementos Decorativos

**Escudos de Equipos**

-   Renderizados en vectorial (SVG) si es posible
-   Tamaño: 48-64px
-   Posición: Esquina superior derecha de la card o junto al nombre

**Banderas de Nacionalidad**

-   Para jugadores: Bandera pequeña junto al nombre
-   Estilo: Iconos planos, no realistas

**Iconos de Estadísticas**

-   Balón para goles, zapato para tiros, mano para paradas, etc.
-   Estilo: Minimalista, line-art, monocromáticos
-   Tamaño: 20-24px junto a cada stat

**Hologramas/Efectos Especiales**

-   Para cards Legendary: Patrón de prismas/cristales en el fondo
-   Degradiente que cambia con el ángulo (simulando hologramas reales)
-   Usar CSS gradients complejos o SVG filters

#### 6. Tipografía

**Nombres y Títulos:**

-   Fuente: Bold condensada, estilo deportivo (ej: Bebas Neue, Oswald, Anton)
-   Peso: 700-900
-   Tracking: -0.02em (ligeramente comprimido)

**Números de Estadísticas:**

-   Fuente: Monoespaciada bold o display (ej: Roboto Mono Bold, DIN Bold)
-   Tamaño: Grandes y prominentes
-   Color: Dorado (#d4af37) o plateado (#c0c0c0) según importancia

**Texto Secundario (labels, footer):**

-   Fuente: Sans-serif limpia (ej: Inter, Roboto)
-   Peso: 400-500
-   Uppercase para labels

### Paleta de Colores

**Base:**

-   Fondo app: #2c2c2c (carbón oscuro)
-   Cards fondo: #f9f9f9 (crema claro, papel de cromo)
-   Texto primario en cards: #1a1a1a (casi negro)
-   Texto secundario: #666666

**Bordes/Rareza:**

-   Common: #c0c0c0 (plateado)
-   Rare: #d4af37 (dorado)
-   Legendary: Gradiente #ff6b6b → #4ecdc4 → #ffe66d

**Acentos por Competición:**

-   Champions League: #003366 a #0066cc
-   La Liga: #ff4444
-   Premier League: #3d195b
-   Mundial: #007749 (verde césped)

**Efectos:**

-   Glow dorado: #ffd700 con blur 8-12px
-   Glow plateado: #e8e8e8 con blur 6-10px
-   Sombra cards: #000000 con 20-30% opacidad, 16-24px blur

### Referencias Visuales

**Buscar inspiración en:**

-   Álbumes Panini (World Cup, Champions League, La Liga)
-   Topps Football cards
-   Sorare (NFT football cards) - diseño digital moderno
-   Apps: Panini Sticker Album, Topps KICK
-   Videojuegos: FIFA Ultimate Team cards, eFootball
-   Trading card games: Magic: The Gathering, Pokémon (por el sistema de rareza)

### Output Esperado

Por favor, genera:

1. **Mockup de interfaz completa** mostrando:

    - Vista de chat con 2 mensajes de usuario (simples)
    - 3 cards del chatbot mostrando diferentes niveles de rareza:
        - Card Common: Estadísticas de un jugador promedio
        - Card Rare: Top scorer de una liga
        - Card Legendary: Récord histórico (ej: "Messi - 91 goles en un año")

2. **Diseño detallado de una card individual** con todos sus elementos:

    - Header con competición
    - Foto/visual del jugador
    - Nombre y posición
    - Grid de estadísticas (4-5 stats)
    - Footer con temporada y número

3. **Variaciones de rareza**:

    - Mismo contenido pero con diferentes tratamientos de borde y efectos según rareza
    - Mostrar visualmente cómo cambia la presentación

4. **Secuencia de animación flip**:

    - 4-5 frames mostrando la card volteándose
    - Vista del "dorso" genérico de la card

5. **Vista mobile**:

    - Cards adaptadas a pantalla vertical
    - Stack de 2-3 cards

6. **Paleta de colores** con muestras visuales y HEX codes

7. **Especificaciones de efectos**:
    - Valores de sombras (x, y, blur, spread, color)
    - Valores de glow/blur para cada nivel de rareza
    - Degradados con puntos de parada exactos

### Restricciones y Consideraciones

-   **Nostalgia balanceada**: Debe evocar cromos tradicionales pero no verse anticuado
-   **Legibilidad**: Las stats deben ser fáciles de leer en todos los niveles de rareza
-   **Performance**: Las animaciones flip deben ser fluidas (60fps)
-   **Responsive**: Cards deben redimensionarse bien en mobile sin perder detalles
-   **Accesibilidad**: Contraste suficiente entre texto y fondo de card
-   **No infantil**: Diseño debe atraer a adultos (25-50 años), no solo niños
-   **Efecto holográfico**: Debe ser sutil en cards comunes, más evidente solo en Legendary

### Tono y Personalidad

-   Nostálgico y coleccionable
-   Premium y exclusivo
-   Divertido pero no trivial
-   Recompensa visual (cada stat parece un "premio")
-   Emoción contenida (el placer de coleccionar)

---

---

# ENGLISH VERSION / VERSIÓN EN INGLÉS

---

## Proposal A: Coach's Tactical Board (English)

### Project Context

I am designing the user interface for a football statistics chatbot called "Visión de Juego" (Game Vision). It's a conversational web application where users ask questions about football statistics (top scorers, standings, match history) and receive intelligent responses.

### Design Objective

I need a complete visual design system that transforms the chat interface into an experience that simulates a **football coach's tactical board**. The goal is to create a differential aesthetic, nostalgic yet modern, that makes consulting statistics feel like being in the locker room receiving tactical analysis from the coach.

### Main Visual Concept

The interface should evoke a physical tactical board where coaches draw plays with chalk. Imagine the classic green/black boards with football diagrams, curved arrows indicating movements, circles representing players, and handwritten notes in white chalk.

### Visual Elements to Design

#### 1. Main Background

-   Dark green or matte black chalkboard texture (not shiny)
-   Should have subtle imperfections: previous erasure marks, small scratches, grainy texture
-   Slight chalk dust effect on the edges
-   Should not be flat: needs tactile depth like a real chalkboard

#### 2. User Messages

-   Container: Rectangle with slightly rounded corners
-   Border: Chalk white, 2-3px thick, with slight irregularity (simulating chalk strokes)
-   Background: Semi-transparent dark for contrast with text
-   Text: Chalk white color with slight opacity (95%)
-   Typography: Clean sans-serif but with subtle handwritten character (e.g., Caveat, Patrick Hand, or similar)

#### 3. Chatbot Messages

-   Container: Irregular ovals or circles (like players in a tactical diagram)
-   For complex statistics: Rectangles with curved corners containing "sub-players" (smaller circles)
-   Border: White chalk with hand-drawn strokes
-   Decorative elements:
    -   Curved arrows connecting related data
    -   Dotted lines indicating "movements"
    -   Small numbers inside circles for ordered lists
-   Effect: Subtle blurred chalk-like shadow

#### 4. Iconography and Decorative Elements

-   Icons drawn in "chalk on board" style:
    -   Football (circle with simplified hexagonal pentagons)
    -   Referee's whistle for notifications
    -   Stopwatch for temporal information
    -   Yellow/red cards to highlight important data
-   Separators: Football field lines (midfield line, areas, etc.)
-   Header: Simplified tactical diagram (4-4-2, 4-3-3) as subtle decoration

#### 5. Animations

-   Message appearance: "Progressive drawing" effect as if written with chalk
-   Transitions: Slight "erase and redraw" effect
-   Hover: Elements "light up" subtly as if someone passed their hand over the chalk
-   Loading: Bouncing ball animation drawn with chalk

### Exact Color Palette

-   **Chalkboard (background)**: #1a3d2e (dark green chalkboard) or #2c2c2c (black chalkboard)
-   **Main chalk (text/borders)**: #f5f5f5 (slightly muted chalk white)
-   **Yellow chalk (highlights)**: #ffd700 with 85% opacity
-   **Orange chalk (warnings/important)**: #ff8c42
-   **Red chalk (errors/cards)**: #e63946
-   **Chalk shadow**: #ffffff with 15% opacity and 8-12px blur

### Visual References (Describe or search for)

-   Professional football coaches' tactical boards
-   Tactical diagrams from sports magazines like FourFourTwo
-   Tactical analysis by Michael Cox or Jonathan Wilson
-   Video games: Tactics screens in FIFA/Football Manager
-   Apps like TacticalPad or Touchtight

### Expected Output

Please generate:

1. **Complete interface mockup** showing:

    - Conversation view with 3-4 interleaved messages
    - A user message asking "Who are the top scorers in La Liga 2024?"
    - Chatbot response showing a list of 3 players with statistics (name, team, goals)

2. **Visual color palette** with HEX codes and samples

3. **Iconography examples** (ball, whistle, stopwatch, cards) in chalk style

4. **State variations**:

    - Message in normal state
    - Message on hover
    - Message with highlighted information (e.g., historical record)

5. **Typographic specifications**:
    - Recommended font with sizes (heading, body, caption)
    - Line-height and letter-spacing

### Restrictions and Considerations

-   Interface must be readable on normal monitors (no zoom required)
-   Contrast must meet WCAG AA for accessibility
-   Should not look childish or cartoonish (target audience: 18-50 years)
-   Must work in both desktop and mobile modes (responsive)
-   Avoid visual saturation: the board should not be "overloaded"
-   Chalk effect should be subtle, not exaggerated (avoid looking like a school blackboard)

### Tone and Personality

-   Nostalgic yet modern
-   Professional yet accessible
-   Tactical and intelligent
-   Contained passion (not excessive fanatic)

---

## Proposal B: Night Stadium (English)

### Project Context

I am designing the user interface for a football statistics chatbot called "Visión de Juego" (Game Vision). It's a web application where users query data about matches, players, standings, and historical statistics through natural conversation.

### Design Objective

I need a complete visual design system that immerses the user in the experience of being in a **football stadium during a night match**. The interface should capture the cinematic atmosphere of Champions League nights: floodlights, perfectly illuminated grass, LED screens showing statistics, and the night sky above the stadium.

### Main Visual Concept

Imagine sitting in a modern stadium at night. You see the bright green field under the floodlights, the dark sky above, electronic screens displaying real-time data, and occasional camera flashes. The interface should replicate this atmosphere: dark, elegant, with neon/LED touches, and a sense of "important live event."

### Visual Elements to Design

#### 1. Main Background

-   **Top layer**: Vertical night sky gradient
    -   Top: Very dark night blue almost black (#0a1628)
    -   Middle: Smooth transition to medium blue (#1a2f4a)
    -   Bottom (lower 20%): Grass green (#2d5016) with subtle grass texture
-   **Atmospheric effects**:
    -   Subtle floating particles (stadium dust/moisture)
    -   Diffuse glow at the top simulating floodlights
    -   Optional: Very subtle silhouettes of stands on lateral extremes (5-10% opacity)

#### 2. Main Chat Container

-   Semi-transparent floating panel over the background
-   Background: Black (#000000) with 70-80% opacity and frosted glass effect (backdrop-filter blur)
-   Border: Subtle white/blue neon-like glow (#4a9eff) 1px with 4-6px glow
-   Corners: Rounded (border-radius: 16-20px)
-   Shadow: Deep and soft simulating the panel "floating" over the stadium

#### 3. User Messages

-   Container: Rectangular bubbles with rounded corners
-   Background: Subtle dark gray gradient (#2c3e50 to #34495e)
-   Border: Thin white line (#ffffff) with 30% opacity
-   Text: Bright LED white (#ffffff)
-   Position: Right-aligned
-   Effect: Very subtle inner shadow

#### 4. Chatbot Messages

-   Container: "Stadium statistics screen" type panel
-   Background: Deep black (#0d0d0d) with LED-type border:
    -   Top border: Yellow-gold gradient (#ffdd57 to #ffa500)
    -   Glow: 8-10px glow effect simulating lit LEDs
-   For large numerical statistics:
    -   Numbers in "digital scoreboard" font (monospaced, bold)
    -   Large size (36-48px for main numbers)
    -   Color: Bright LED white with slight glow
-   Separators: Thin horizontal lines simulating LED rows

#### 5. Decorative Elements and Details

##### Field Lines

-   In the background or as separators between sections
-   Bright white (#ffffff) with 40% opacity
-   Simulating painted football field lines
-   Use as subtle dividers in the interface

##### Floodlight Effects

-   Subtle light cones from upper corners
-   Radial gradient from specific points
-   Color: Warm white (#fff8e7) with very low opacity (5-10%)
-   Should be barely perceptible, not obvious

##### "Camera Flash" Effect

-   When an important statistic or record appears
-   Brief white flash (200-300ms) on message border
-   Small dispersing light particles
-   Use sparingly (only for highlights)

##### Iconography

-   LED/neon style icons with glow effect:
    -   Ball: Circle with golden glow
    -   Stadium: Simple silhouette with floodlights
    -   Stopwatch: LED digital clock
    -   Trophy: Bright silhouette
-   Color: White/gold with glow

#### 6. Animations

##### Message Appearance

-   Fade in from below (translateY: 20px → 0)
-   Duration: 400-500ms
-   Easing: Smooth ease-out
-   For messages with statistics: numbers "count" from 0 to final value

##### Typing Indicator

-   Three dots blinking in LED style
-   Color pulsing between bright yellow and dim yellow
-   Rhythm: 800ms per cycle

##### Scroll

-   Smooth scroll with momentum
-   When reaching the end: slight bounce effect

##### Hover/Interaction

-   Interactive elements slightly increase their glow
-   Smooth transition (200ms)
-   Cursor changes to pointer with subtle light trail

### Exact Color Palette

**Background:**

-   Upper night sky: #0a1628
-   Middle night sky: #1a2f4a
-   Lower grass: #2d5016

**Containers:**

-   Main panel: #000000 (80% opacity)
-   User messages: #2c3e50 to #34495e
-   Chatbot messages: #0d0d0d

**Accents:**

-   LED white: #ffffff
-   LED yellow/gold: #ffdd57 to #ffa500
-   Floodlight glow: #fff8e7 (very low opacity)
-   Neon blue: #4a9eff

**Effects:**

-   Main glow: #ffffff with 8-12px blur
-   Gold glow: #ffd700 with 10-15px blur
-   Shadows: #000000 with 40-60% opacity

### Visual References

**Seek inspiration in:**

-   Champions League night matches (UEFA)
-   Modern stadiums: Allianz Arena, Wanda Metropolitano, Tottenham Hotspur Stadium
-   Football movie cinematography (Goal!, United)
-   Stadium LED screens (scoreboards, advertising boards)
-   Sports apps: OneFootball (dark mode), FotMob, Sofascore
-   UI design: Behance/Dribbble search "dark sports app" or "stadium UI"

### Expected Output

Please generate:

1. **Complete interface mockup** showing:

    - Conversation view with 4-5 messages
    - User question: "How many goals does Lewandowski have this season?"
    - Chatbot response with highlighted statistic: "32 goals" in large, plus breakdown (League: 25, Champions: 7)
    - Background with night sky → grass gradient

2. **Individual component details**:

    - User message bubble (normal and hover)
    - Chatbot statistics panel with large numbers
    - Typing indicator ("typing...")
    - Header/navbar with "Visión de Juego" name

3. **Visual color palette** with samples and HEX codes

4. **Light effect specifications**:

    - Exact glow values (blur, spread, color)
    - Recommended opacities
    - Gradients with stop points

5. **Typographic guide**:

    - Font for general text (readability in dark)
    - Font for large numbers (digital scoreboard style)
    - Sizes, weights, line-heights

6. **Special states**:
    - Message with "historical record" (camera flash effect)
    - Error/empty message
    - Loading data

### Restrictions and Considerations

-   **Readability**: High contrast between text and background (WCAG AAA if possible)
-   **Performance**: Blur and glow effects should not affect performance
-   **Responsive**: Design must adapt to mobile without losing atmosphere
-   **Don't saturate**: Light effects should be subtle; avoid "disco" look
-   **Professional**: Should look premium, not like a casual game
-   **Target age**: Adult audience (18-55 years), avoid very juvenile aesthetics

### Tone and Personality

-   Cinematic and epic (like a Champions final)
-   Modern and premium
-   Elegant without being ostentatious
-   Exciting but contained
-   Technological and futuristic without being sci-fi

---

## Proposal C: Trading Cards/Collectible Cards (English)

### Project Context

I am designing the user interface for a football statistics chatbot called "Visión de Juego" (Game Vision). Users ask questions about players, teams, and competitions, and the chatbot responds with statistical data. I want the experience to be collectible, nostalgic, and visually rich.

### Design Objective

I need a visual design system that presents statistics as **collectible football cards** (Panini album style). Each chatbot response should feel like opening a pack of cards and discovering valuable information presented in an iconic format familiar to football fans.

### Main Visual Concept

Imagine traditional football cards: cardboard rectangles with player photo, team crest, key statistics, and edge details (gold for specials, silver for rares). The interface should transform cold data into visually attractive "collectible cards," with clear hierarchy and details that pay homage to physical albums.

### Visual Elements to Design

#### 1. Application Main Background

-   Base color: Dark charcoal gray (#2c2c2c) or soft black (#1a1a1a)
-   Subtle texture: Recycled paper or thin cardboard weave (5-8% opacity)
-   Optional effect: Very subtle repeating pattern of team crests in watermark (3% opacity)
-   Gradient: Very light from center (lighter) to edges (darker)

#### 2. User Messages

-   Style: Simple and discreet bubbles to not compete with cards
-   Container: Rectangle with rounded corners (12-16px)
-   Background: Semi-transparent dark gray (#3a3a3a with 85% opacity)
-   Border: Subtle, 1px solid medium gray (#5a5a5a)
-   Text: Soft white (#f0f0f0)
-   Position: Right-aligned
-   No excessive decoration (visual focus should be on chatbot cards)

#### 3. Chatbot Messages (Cards/Trading Cards)

##### General Card Structure

Each statistical response is presented as one or several trading cards:

**Dimensions:**

-   Proportion: Similar to real cards (approx. 2.5:3.5 ratio)
-   Width: 280-320px on desktop
-   Internal padding: 16-20px

**Card Elements:**

###### Card Header

-   **Top stripe** with league/competition name
    -   Background: Gradient according to competition (e.g., Champions = dark blue to light blue)
    -   Text: White, uppercase, condensed font, bold
    -   Icons: Small competition logo

###### Photo/Main Visual

-   For players: Cutout photo of player
    -   Background: Team color gradient
    -   Position: Centered or in dynamic pose
-   For teams: Large, clean crest
-   For general statistics: Related iconography (ball, trophy, etc.)

###### Central Information

-   **Player/Team Name**: Bold typography, large (24-28px)
    -   Color: According to rarity level (see below)
    -   Effect: Subtle shadow to highlight
-   **Position/Role**: Small text above name (e.g., "Forward", "Midfielder")

###### Key Statistics

Table/grid type layout:

-   **Large numbers**: 32-40px, bold, gold/silver color according to importance
-   **Labels**: 12-14px, uppercase, light gray
-   Example layout:
    ```
    GOALS        ASSISTS      MATCHES
      32            15           28
    ```
-   Separators: Thin vertical lines between columns

###### Card Footer

-   Year/Season: Small, bottom left
-   Fictional card number: "045/500" bottom right (adds exclusivity)

##### Rarity System (Affects border style and effects)

**Common**

-   Border: Matte silver (#c0c0c0)
-   Thickness: 2px solid
-   No special effects

**Rare**

-   Border: Gold (#d4af37)
-   Thickness: 3px with slight bevel
-   Effect: Subtle gold glow (4-6px blur)

**Legendary - For records/historical**

-   Border: Holographic rainbow gradient (#ff6b6b → #4ecdc4 → #ffe66d → #ff6b6b)
-   Thickness: 4px
-   Effect: Animated iridescent glow
-   Background: Subtle gradient with shimmer effect
-   Particles: Small floating stars/sparkles

##### Multiple Cards (Responses with various statistics)

-   Layout: Grid of 2-3 cards side by side (desktop) or vertical stack (mobile)
-   Spacing: 16-20px between cards
-   Animation: Appear in cascade (150ms stagger between each)

#### 4. Card Animations

##### Appearance (Flip Animation)

The card "flips" as if you were discovering a card:

-   Initial state: Card back view (generic, app logo)
-   Animation: 3D rotation on Y axis (0deg → 180deg)
-   Duration: 600-800ms
-   Easing: Cubic-bezier for realistic effect
-   While flipping: Slight "holographic shine" effect passing over the surface

##### Hover

-   Card elevates slightly (translateY: -8px)
-   Shadow intensifies and expands
-   Border increases its glow
-   Duration: 250ms
-   For Legendary: Holographic effect accelerates

##### Click/Tap (Optional action: "Save statistic")

-   Card makes a small "jump" (bounce animation)
-   Optional sound like album "click"
-   Visual feedback: Checkmark appears briefly

#### 5. Iconography and Decorative Elements

**Team Crests**

-   Rendered in vector (SVG) if possible
-   Size: 48-64px
-   Position: Upper right corner of card or next to name

**Nationality Flags**

-   For players: Small flag next to name
-   Style: Flat icons, not realistic

**Statistics Icons**

-   Ball for goals, shoe for shots, hand for saves, etc.
-   Style: Minimalist, line-art, monochromatic
-   Size: 20-24px next to each stat

**Holograms/Special Effects**

-   For Legendary cards: Prism/crystal pattern in background
-   Gradient that changes with angle (simulating real holograms)
-   Use complex CSS gradients or SVG filters

#### 6. Typography

**Names and Titles:**

-   Font: Bold condensed, sports style (e.g., Bebas Neue, Oswald, Anton)
-   Weight: 700-900
-   Tracking: -0.02em (slightly compressed)

**Statistics Numbers:**

-   Font: Bold monospaced or display (e.g., Roboto Mono Bold, DIN Bold)
-   Size: Large and prominent
-   Color: Gold (#d4af37) or silver (#c0c0c0) according to importance

**Secondary Text (labels, footer):**

-   Font: Clean sans-serif (e.g., Inter, Roboto)
-   Weight: 400-500
-   Uppercase for labels

### Color Palette

**Base:**

-   App background: #2c2c2c (dark charcoal)
-   Cards background: #f9f9f9 (light cream, card paper)
-   Primary text on cards: #1a1a1a (almost black)
-   Secondary text: #666666

**Borders/Rarity:**

-   Common: #c0c0c0 (silver)
-   Rare: #d4af37 (gold)
-   Legendary: Gradient #ff6b6b → #4ecdc4 → #ffe66d

**Accents by Competition:**

-   Champions League: #003366 to #0066cc
-   La Liga: #ff4444
-   Premier League: #3d195b
-   World Cup: #007749 (grass green)

**Effects:**

-   Gold glow: #ffd700 with 8-12px blur
-   Silver glow: #e8e8e8 with 6-10px blur
-   Card shadows: #000000 with 20-30% opacity, 16-24px blur

### Visual References

**Seek inspiration in:**

-   Panini albums (World Cup, Champions League, La Liga)
-   Topps Football cards
-   Sorare (NFT football cards) - modern digital design
-   Apps: Panini Sticker Album, Topps KICK
-   Video games: FIFA Ultimate Team cards, eFootball
-   Trading card games: Magic: The Gathering, Pokémon (for rarity system)

### Expected Output

Please generate:

1. **Complete interface mockup** showing:

    - Chat view with 2 user messages (simple)
    - 3 chatbot cards showing different rarity levels:
        - Common Card: Statistics of an average player
        - Rare Card: Top scorer of a league
        - Legendary Card: Historical record (e.g., "Messi - 91 goals in a year")

2. **Detailed design of an individual card** with all its elements:

    - Header with competition
    - Player photo/visual
    - Name and position
    - Statistics grid (4-5 stats)
    - Footer with season and number

3. **Rarity variations**:

    - Same content but with different border treatments and effects according to rarity
    - Visually show how presentation changes

4. **Flip animation sequence**:

    - 4-5 frames showing the card flipping
    - View of generic card "back"

5. **Mobile view**:

    - Cards adapted to vertical screen
    - Stack of 2-3 cards

6. **Color palette** with visual samples and HEX codes

7. **Effect specifications**:
    - Shadow values (x, y, blur, spread, color)
    - Glow/blur values for each rarity level
    - Gradients with exact stop points

### Restrictions and Considerations

-   **Balanced nostalgia**: Should evoke traditional cards but not look outdated
-   **Readability**: Stats should be easy to read at all rarity levels
-   **Performance**: Flip animations should be fluid (60fps)
-   **Responsive**: Cards should resize well on mobile without losing details
-   **Accessibility**: Sufficient contrast between text and card background
-   **Not childish**: Design should appeal to adults (25-50 years), not just children
-   **Holographic effect**: Should be subtle on common cards, more evident only on Legendary

### Tone and Personality

-   Nostalgic and collectible
-   Premium and exclusive
-   Fun but not trivial
-   Visual reward (each stat feels like a "prize")
-   Contained emotion (the pleasure of collecting)

---

## Usage Notes

### For Text Models (ChatGPT, Claude):

Copy the complete prompt and ask them to generate detailed specifications, CSS code, or more specific descriptions.

### For Image Models (Midjourney, DALL-E, Stable Diffusion):

Extract the "Visual Concept", "Visual Elements", and "Color Palette" sections and condense them into a shorter prompt focused on visuals.

### For Human Designers:

These prompts serve as a complete design brief.

### Iteration:

After receiving the first output, you can make follow-ups like:

-   "Now generate the mobile version of this"
-   "Show me 3 variations of the color palette"
-   "Create a complete style guide with all components"
