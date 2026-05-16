# Neon Defender - Videojuego en Unity

![Versión](https://img.shields.io/badge/Versión-1.0.0-blueviolet)
![Unity](https://img.shields.io/badge/Unity-6000.3.10f1-blue)
![URP](https://img.shields.io/badge/Render_Pipeline-URP_17.3.0-lightgrey)
![AWS](https://img.shields.io/badge/Backend-AWS_Lambda-ff9900)

## Descripción

NeonTD es un proyecto de juego tipo Tower Defense creado en Unity. Está ambientado en un universo futurista con identidad visual de `Neon Defender`, fundamentada en una estética cyberpunk y de ciencia ficción, caracterizada por una paleta de tonos oscuros en contraste con efectos de iluminación neón.

Incluye:

- Niveles con caminos múltiples y lógica de rondas.
- Sistema de guardado y login mediante AWS Lambda.
- Gestión de estadísticas de partida (daño, oro, rondas, torres y enemigos).
- Interfaz de usuario para menús, historial y progreso.
- 6 torres jugables y 9 enemigos distintos.

## Tecnologías

- Unity Editor: `6000.3.10f1`
- Pipeline de renderizado: Universal Render Pipeline (URP) `17.3.0`
- Paquetes principales:
  - `com.unity.inputsystem` `1.18.0`
  - `com.unity.visualscripting` `1.9.9`
  - `com.unity.2d.animation` `13.0.4`
  - `com.unity.2d.tilemap.extras` `6.0.1`
- Backend: AWS Lambda (para login, historial y guardado de partidas)

## Estructura del proyecto

- `Assets/Scenes/`
  - `InterfazUsuario.unity`
  - `Nivel1.unity`
  - `Nivel2.unity`
  - `Nivel3.unity`
- `Assets/Scripts/`
  - `Menu/GestorPantallas.cs` - controla login, registro y llamadas a AWS
  - `Menu/GestorConfiguracion.cs` - controla audio, fullscreen y preferencias
  - `GestorDatosPartida.cs` - singleton que guarda y procesa datos de partida
  - `DatosPartida.cs` - modelo JSON con estadísticas, torres y enemigos
  - `GeneradorEnemigos.cs` - controla oleadas, modo infinito y fin de ronda
  - `Niveles/administradorNivel.cs` - controla victoria, derrota, pausa y guardado
  - `Niveles/cargarEstadisticas.cs` - carga estadísticas finales en la UI
  - `GestorEconomia.cs` - administra el oro del jugador
  - `GestorTorres.cs` - construcción, previsualización y venta de torres
  - `SeleccionTorre.cs` - selección de torre y visualización de rango
  - `LogicaTorre.cs` - disparo estándar de torreta a enemigos en rango
  - `LogicaTorre5.cs` - variante de torre que dispara balas perforantes
  - `TorreRayo.cs` - torre con rayo continuo y daño por enfoque
  - `TorreTesla.cs` - torre Tesla que ralentiza y daña enemigos en área
  - `LogicaEnemigo.cs` - comportamiento de enemigos, movimiento y vida
  - `LogicaBala.cs` - proyectil guiado que persigue enemigos
  - `BalaPerforante.cs` - proyectil directo con daño continuo opcional
  - `BasePrincipal.cs` - gestiona la vida de la base y detecta derrota
  - `TarjetaPartida.cs` - muestra cada partida en el historial
  - `GestorTarjetas.cs` - abre/cierra la pantalla de historial
  - `CarruselDificultad.cs` - selección de mapa/dificultad en el menú
  - `ControladorTiempo.cs` - controla la velocidad de juego (x1, x1.5, x2)
  - `DibujarRango.cs` - dibuja el rango de ataque de las torres con LineRenderer
  - `CodigoSecreto.cs` - secuencia de teclas para obtener bono de oro
  - `RotacionVisual.cs` - rota elementos decorativos en la escena
- `Assets/Scripts/Universales/`
  - `GestorIdiomas.cs` - traducciones y cambio de idioma Español/Inglés
  - `BanderaBtn.cs` - actualiza la bandera del botón según idioma
  - `Audio/GestorMusica.cs` - música persistente y reproducción de efectos

## Descripción de scripts

### Menú, configuración y UI
- `GestorPantallas.cs`: navega entre pantallas del menú, registra/login con AWS y solicita historial.
- `GestorConfiguracion.cs`: ajusta volumen de música/FX y cambia el modo de pantalla.
- `MenuPrincipal.cs`: acción simple para salir del juego.
- `CarruselDificultad.cs`: muestra mapas, cambia selección y carga la escena correspondiente.
- `GestorTarjetas.cs`: abre/cierra el historial de partidas.
- `TarjetaPartida.cs`: rellena cada tarjeta de historial con datos de una partida.

### Gestión de datos y guardado
- `GestorDatosPartida.cs`: singleton persistente que mantiene el estado de la partida y envía datos a AWS.
- `DatosPartida.cs`: contiene las estadísticas de juego y los métodos para registrar eventos (oro, daño, rondas, torres, enemigos).
- `CargarEstadisticas.cs`: muestra en pantalla las estadísticas actuales guardadas por `GestorDatosPartida`.

### Gameplay y mecánicas del juego
- `GeneradorEnemigos.cs`: controla el spawn de oleadas, el modo infinito, y notifica cuando termina una ronda.
- `BasePrincipal.cs`: gestiona la salud de la base y desencadena derrota si la base llega a 0.
- `AdministradorNivel.cs`: maneja estado de victoria, derrota, pausa, y solicita guardado de partida.
- `GestorEconomia.cs`: suma, resta y verifica el oro disponible; actualiza la UI.
- `GestorTorres.cs`: selecciona torres, muestra previews y realiza las compras/ventas.
- `SeleccionTorre.cs`: gestiona la selección de torres en el mapa y muestra el rango activo.

### Torres y disparos
- `LogicaTorre.cs`: torre estándar que busca y dispara al primer enemigo dentro de su rango.
- `LogicaTorre5.cs`: torre alternativa que dispara balas perforantes a objetivos cercanos.
- `TorreRayo.cs`: emite un rayo continuo que daña al enemigo con daño creciente mientras impacta.
- `TorreTesla.cs`: aplica ralentización y daño por zona en el camino del enemigo.
- `LogicaBala.cs`: proyectil que sigue a un enemigo hasta impactar y aplicar daño.
- `BalaPerforante.cs`: bala directa que atraviesa enemigos y puede aplicar daño continuo.

### Enemigos y efectos especiales
- `LogicaEnemigo.cs`: controla el movimiento por waypoints, la salud del enemigo, recompensas y daño a la base.
- `DibujarRango.cs`: dibuja visualmente el rango de ataque de una torre como círculo o cuadrado.
- `ControladorTiempo.cs`: cambia la velocidad de tiempo y actualiza el texto del botón.
- `CodigoSecreto.cs`: detecta una secuencia de teclas secreta para dar oro de bonificación.
- `RotacionVisual.cs`: rota objetos para efectos visuales en la escena.

### Internacionalización y audio
- `GestorIdiomas.cs`: cambia los textos y opciones de idioma entre español e inglés.
- `BanderaBtn.cs`: actualiza la imagen de la bandera según el idioma actual.
- `GestorMusica.cs`: mantiene música entre escenas, reproduce efectos y aplica volumen guardado.

## Cómo replicar el proyecto

### Requisitos

1. Instalar Unity Editor `6000.3.10f1`.
2. Instalar Unity Hub y agregar la versión indicada si está disponible.
3. Abrir el proyecto desde la carpeta raíz `c:\GitHub\NeonTD`.

### Pasos para abrir

1. Clona o descarga el repositorio:
   ```bash
   git clone <url-del-repositorio>
   cd NeonTD
   ```
2. Abre el proyecto con Unity Hub seleccionando la carpeta raíz.
3. Deja que Unity importe los paquetes y regenere la carpeta `Library` si es necesario.
4. Abre la escena principal desde `Assets/Scenes/InterfazUsuario.unity`.

### Escenas principales

- `InterfazUsuario.unity` - menú principal, login y navegación.
- `Nivel1.unity`, `Nivel2.unity`, `Nivel3.unity` - niveles de juego.

## Configuración adicional

- `Packages/manifest.json` define los paquetes de Unity usados en el proyecto.
- `ProjectSettings/ProjectVersion.txt` confirma la versión de Unity.
- El proyecto usa AWS Lambda para:
  - login/registro de usuario
  - historial de partidas
  - guardado de datos de partida

> Nota: se recomienda mantener la misma versión de Unity para evitar problemas de compatibilidad.

## Ejecución

1. Selecciona la escena `InterfazUsuario.unity`.
2. Ejecuta el proyecto desde Unity con `Play`.
3. Navega por el menú para iniciar niveles y probar las funciones de juego.

## Notas

- Si se produce un error de paquetes, abre el `Package Manager` y actualiza/restaura los paquetes según el archivo `Packages/manifest.json`.
- Si el backend AWS no está disponible, algunas funcionalidades de login y guardado pueden fallar.
