using UnityEngine;
using TMPro;

public class GestorIdiomas : MonoBehaviour
{
    [Header("Pantalla: Menú Principal")]
    public TMP_Text txtTituloHistorial;
    public TMP_Text txtTituloConfiguracion;
    public TMP_Text txtBtnJugar;
    public TMP_Text txtBtnHistorial;
    public TMP_Text txtBtnConfiguracion;

    [Header("Pantalla: Jugar")]
    public TMP_Text txtNuevaPartida;

    [Header("Pantalla: Login")]
    public TMP_Text txtIniciarSesion;
    public TMP_Text txtCrearCuenta;
    public TMP_Text txtBtnContinuar;

    [Header("Textos de Niveles (Partidas)")]
    public TMP_Text txtBtnEmpezar;
    public TMP_Text txtBtnVender;

    [Header("Pantalla: Fin de Partida")]
    public TMP_Text txtBtnModoInfinito;
    public TMP_Text txtBtnNuevaPartidaFin;
    public TMP_Text txtBtnEstadisticas;
    public TMP_Text txtBtnMenuPrincipal;
    public TMP_Text txtTituloEstadisticas;

    [Header("Etiquetas repetidas")]
    public TMP_Text[] listaTxtUsuario;
    public TMP_Text[] listaTxtContrasena;

    [Header("Placeholders repetidos")]
    public TMP_Text[] listaPlaceholderUsuario;
    public TMP_Text[] listaPlaceholderContrasena;

    [Header("Botones Repetidos")]
    public TMP_Text[] listaBtnSalir;
    public TMP_Text[] listaBtnCrear;

    [Header("Pantalla: Configuración")]
    public TMP_Text txtMusica;
    public TMP_Text txtEfectos;
    public TMP_Text txtResolucion;
    public TMP_Dropdown dropdownPantalla;

    [Header("Catálogo de Torres: Nombres")]
    public TMP_Text txtNomPulso;
    public TMP_Text txtNomTesla;
    public TMP_Text txtNomTrituradora;
    public TMP_Text txtNomLaser;
    public TMP_Text txtNomVeneno;
    public TMP_Text txtNomPerforante;

    [Header("Catálogo de Torres: Descripciones")]
    public TMP_Text txtDescPulso;
    public TMP_Text txtDescTesla;
    public TMP_Text txtDescTrituradora;
    public TMP_Text txtDescLaser;
    public TMP_Text txtDescVeneno;
    public TMP_Text txtDescPerforante;

    [Header("Catálogo de Torres: Tipos de daño")]
    public TMP_Text txtDanoPulso;
    public TMP_Text txtDanoTesla;
    public TMP_Text txtDanoTrituradora;
    public TMP_Text txtDanoLaser;
    public TMP_Text txtDanoVeneno;
    public TMP_Text txtDanoPerforante;

    [Header("Bestiario: Descripciones")]
    public TMP_Text txtDescVelth;
    public TMP_Text txtDescVelarth;
    public TMP_Text txtDescVelariath;
    public TMP_Text txtDescPyxer;
    public TMP_Text txtDescPyxerion;
    public TMP_Text txtDescPyxerarch;
    public TMP_Text txtDescZhax;
    public TMP_Text txtDescZhaxeris;
    public TMP_Text txtDescZhaxarian;

    [Header("Bestiario: Estadísticas")]
    public TMP_Text txtStatsVelth;
    public TMP_Text txtStatsVelarth;
    public TMP_Text txtStatsVelariath;
    public TMP_Text txtStatsPyxer;
    public TMP_Text txtStatsPyxerion;
    public TMP_Text txtStatsPyxerarch;
    public TMP_Text txtStatsZhax;
    public TMP_Text txtStatsZhaxeris;
    public TMP_Text txtStatsZhaxarian;

    void Start()
    {
        ActualizarTextos();
    }

    public void AlternarIdioma()
    {
        int idiomaActual = PlayerPrefs.GetInt("IdiomaActual", 0);
        CambiarIdioma(idiomaActual == 0 ? 1 : 0);
    }

    public void CambiarIdioma(int id)
    {
        PlayerPrefs.SetInt("IdiomaActual", id);
        PlayerPrefs.Save();

        ActualizarTextos();

        BanderaBtn[] todosLosBotones = Resources.FindObjectsOfTypeAll<BanderaBtn>();
        foreach (BanderaBtn btn in todosLosBotones)
        {
            btn.ActualizarBandera();
        }

        CarruselDificultad[] carruseles = Resources.FindObjectsOfTypeAll<CarruselDificultad>();
        foreach (CarruselDificultad c in carruseles)
        {
            c.ActualizarPantalla();
        }
    }

    private void ActualizarTextos()
    {
        int idioma = PlayerPrefs.GetInt("IdiomaActual", 0);

        if (idioma == 0)
        {
            if (txtTituloHistorial != null) txtTituloHistorial.text = "Historial";
            if (txtTituloConfiguracion != null) txtTituloConfiguracion.text = "Configuración";
            if (txtBtnJugar != null) txtBtnJugar.text = "Jugar";
            if (txtBtnHistorial != null) txtBtnHistorial.text = "Historial";
            if (txtBtnConfiguracion != null) txtBtnConfiguracion.text = "Configuración";

            if (txtNuevaPartida != null) txtNuevaPartida.text = "Nueva Partida";
            if (txtIniciarSesion != null) txtIniciarSesion.text = "Iniciar sesión";
            if (txtCrearCuenta != null) txtCrearCuenta.text = "Crear Cuenta";
            if (txtBtnContinuar != null) txtBtnContinuar.text = "Continuar";

            if (txtMusica != null) txtMusica.text = "Música";
            if (txtEfectos != null) txtEfectos.text = "Efectos";
            if (txtResolucion != null) txtResolucion.text = "Resolución";

            if (dropdownPantalla != null && dropdownPantalla.options.Count >= 2)
            {
                dropdownPantalla.options[0].text = "Pantalla Completa";
                dropdownPantalla.options[1].text = "Ventana con Bordes";
                dropdownPantalla.captionText.text = dropdownPantalla.options[dropdownPantalla.value].text;
            }

            if (txtBtnEmpezar != null) txtBtnEmpezar.text = "Empezar";
            if (txtBtnVender != null) txtBtnVender.text = "Vender";

            if (txtBtnModoInfinito != null) txtBtnModoInfinito.text = "Modo Infinito";
            if (txtBtnNuevaPartidaFin != null) txtBtnNuevaPartidaFin.text = "Nueva Partida";
            if (txtBtnEstadisticas != null) txtBtnEstadisticas.text = "Estadísticas";
            if (txtBtnMenuPrincipal != null) txtBtnMenuPrincipal.text = "Menú Principal";
            if (txtTituloEstadisticas != null) txtTituloEstadisticas.text = "Estadísticas";

            TraducirLista(listaTxtUsuario, "Usuario");
            TraducirLista(listaTxtContrasena, "Contraseña");
            TraducirLista(listaPlaceholderUsuario, "Ingrese su usuario...");
            TraducirLista(listaPlaceholderContrasena, "Ingrese su contraseña...");
            TraducirLista(listaBtnSalir, "Salir");
            TraducirLista(listaBtnCrear, "Crear");

            if (txtNomPulso != null) txtNomPulso.text = "Torre de Pulso";
            if (txtNomTesla != null) txtNomTesla.text = "Torre Tesla";
            if (txtNomTrituradora != null) txtNomTrituradora.text = "Torre Trituradora";
            if (txtNomLaser != null) txtNomLaser.text = "Torre Láser";
            if (txtNomVeneno != null) txtNomVeneno.text = "Torre de Veneno";
            if (txtNomPerforante != null) txtNomPerforante.text = "Torre Perforante";

            if (txtDescPulso != null) txtDescPulso.text = "Emite un pulso de energía concentrada que impacta con precisión en un único objetivo.";
            if (txtDescTesla != null) txtDescTesla.text = "Electrifica el suelo a su alrededor para ralentizar el avance enemigo mientras inflige un daño leve.";
            if (txtDescTrituradora != null) txtDescTrituradora.text = "Unidad de asalto pesado con mayor alcance y potencia que la torre de pulso básica.";
            if (txtDescLaser != null) txtDescLaser.text = "Fija un haz térmico en un enemigo; el daño aumenta drásticamente cuanto más tiempo mantiene el objetivo.";
            if (txtDescVeneno != null) txtDescVeneno.text = "Infecta a los objetivos con toxinas corrosivas que restan vida de forma constante a través del tiempo.";
            if (txtDescPerforante != null) txtDescPerforante.text = "Lanza proyectiles de alta velocidad capaces de atravesar múltiples enemigos en una línea recta.";

            if (txtDanoPulso != null) txtDanoPulso.text = "Tipo de daño: Impacto";
            if (txtDanoTesla != null) txtDanoTesla.text = "Tipo de daño: Daño cada 1s (Tick)";
            if (txtDanoTrituradora != null) txtDanoTrituradora.text = "Tipo de daño: Impacto";
            if (txtDanoLaser != null) txtDanoLaser.text = "Tipo de daño: Daño incremental";
            if (txtDanoVeneno != null) txtDanoVeneno.text = "Tipo de daño: Daño en el tiempo (DoT)";
            if (txtDanoPerforante != null) txtDanoPerforante.text = "Tipo de daño: Impacto";

            if (txtDescVelth != null) txtDescVelth.text = "Unidad básica y equilibrada, ideal para formar la primera línea de asedio.";
            if (txtDescVelarth != null) txtDescVelarth.text = "Versión mejorada del Velth con el doble de resistencia y mayor poder destructivo.";
            if (txtDescVelariath != null) txtDescVelariath.text = "Líder de asalto formidable con una reserva masiva de salud.";
            if (txtDescPyxer != null) txtDescPyxer.text = "Unidad extremadamente veloz pero frágil, diseñada para colarse entre tus defensas.";
            if (txtDescPyxerion != null) txtDescPyxerion.text = "Corredor que sacrifica un poco de velocidad a cambio de mayor supervivencia.";
            if (txtDescPyxerarch != null) txtDescPyxerarch.text = "Líder escurridizo que mantiene un paso rápido mientras soporta daño considerable.";
            if (txtDescZhax != null) txtDescZhax.text = "Gigante acorazado de movimiento lento con una gran cantidad de vida inicial.";
            if (txtDescZhaxeris != null) txtDescZhaxeris.text = "Coloso de as siege con blindaje pesado que requiere fuego concentrado para caer.";
            if (txtDescZhaxarian != null) txtDescZhaxarian.text = "Fortaleza móvil extremadamente lenta, pero con una resistencia titánica.";

            if (txtStatsVelth != null) txtStatsVelth.text = "Velocidad: 175 - Vida: 100 - Daño: 75 - Oro: 20";
            if (txtStatsVelarth != null) txtStatsVelarth.text = "Velocidad: 175 - Vida: 200 - Daño: 125 - Oro: 45";
            if (txtStatsVelariath != null) txtStatsVelariath.text = "Velocidad: 175 - Vida: 1500 - Daño: 200 - Oro: 300";
            if (txtStatsPyxer != null) txtStatsPyxer.text = "Velocidad: 280 - Vida: 65 - Daño: 50 - Oro: 5";
            if (txtStatsPyxerion != null) txtStatsPyxerion.text = "Velocidad: 240 - Vida: 130 - Daño: 75 - Oro: 15";
            if (txtStatsPyxerarch != null) txtStatsPyxerarch.text = "Velocidad: 200 - Vida: 300 - Daño: 100 - Oro: 150";
            if (txtStatsZhax != null) txtStatsZhax.text = "Velocidad: 150 - Vida: 555 - Daño: 250 - Oro: 100";
            if (txtStatsZhaxeris != null) txtStatsZhaxeris.text = "Velocidad: 125 - Vida: 888 - Daño: 375 - Oro: 150";
            if (txtStatsZhaxarian != null) txtStatsZhaxarian.text = "Velocidad: 100 - Vida: 3000 - Daño: 500 - Oro: 150";
        }
        else
        {
            if (txtTituloHistorial != null) txtTituloHistorial.text = "History";
            if (txtTituloConfiguracion != null) txtTituloConfiguracion.text = "Settings";
            if (txtBtnJugar != null) txtBtnJugar.text = "Play";
            if (txtBtnHistorial != null) txtBtnHistorial.text = "History";
            if (txtBtnConfiguracion != null) txtBtnConfiguracion.text = "Settings";

            if (txtNuevaPartida != null) txtNuevaPartida.text = "New Game";
            if (txtIniciarSesion != null) txtIniciarSesion.text = "Login";
            if (txtCrearCuenta != null) txtCrearCuenta.text = "Create Account";
            if (txtBtnContinuar != null) txtBtnContinuar.text = "Continue";

            if (txtMusica != null) txtMusica.text = "Music";
            if (txtEfectos != null) txtEfectos.text = "Effects";
            if (txtResolucion != null) txtResolucion.text = "Resolution";

            if (dropdownPantalla != null && dropdownPantalla.options.Count >= 2)
            {
                dropdownPantalla.options[0].text = "Full Screen";
                dropdownPantalla.options[1].text = "Windowed";
                dropdownPantalla.captionText.text = dropdownPantalla.options[dropdownPantalla.value].text;
            }

            if (txtBtnEmpezar != null) txtBtnEmpezar.text = "Start";
            if (txtBtnVender != null) txtBtnVender.text = "Sell";

            if (txtBtnModoInfinito != null) txtBtnModoInfinito.text = "Endless Mode";
            if (txtBtnNuevaPartidaFin != null) txtBtnNuevaPartidaFin.text = "New Game";
            if (txtBtnEstadisticas != null) txtBtnEstadisticas.text = "Statistics";
            if (txtBtnMenuPrincipal != null) txtBtnMenuPrincipal.text = "Main Menu";
            if (txtTituloEstadisticas != null) txtTituloEstadisticas.text = "Statistics";

            TraducirLista(listaTxtUsuario, "Username");
            TraducirLista(listaTxtContrasena, "Password");
            TraducirLista(listaPlaceholderUsuario, "Enter username...");
            TraducirLista(listaPlaceholderContrasena, "Enter password...");
            TraducirLista(listaBtnSalir, "Exit");
            TraducirLista(listaBtnCrear, "Create");

            if (txtNomPulso != null) txtNomPulso.text = "Pulse Tower";
            if (txtNomTesla != null) txtNomTesla.text = "Tesla Tower";
            if (txtNomTrituradora != null) txtNomTrituradora.text = "Shredder Tower";
            if (txtNomLaser != null) txtNomLaser.text = "Laser Tower";
            if (txtNomVeneno != null) txtNomVeneno.text = "Poison Tower";
            if (txtNomPerforante != null) txtNomPerforante.text = "Piercing Tower";

            if (txtDescPulso != null) txtDescPulso.text = "Emits a concentrated energy pulse that strikes a single target with precision.";
            if (txtDescTesla != null) txtDescTesla.text = "Electrifies the surrounding ground to slow enemy advance while dealing light damage.";
            if (txtDescTrituradora != null) txtDescTrituradora.text = "Heavy assault unit with greater range and power than the basic pulse tower.";
            if (txtDescLaser != null) txtDescLaser.text = "Locks a thermal beam on an enemy; damage increases drastically the longer it holds the target.";
            if (txtDescVeneno != null) txtDescVeneno.text = "Infects targets with corrosive toxins that constantly drain health over time.";
            if (txtDescPerforante != null) txtDescPerforante.text = "Fires high-speed projectiles capable of piercing multiple enemies in a straight line.";

            if (txtDanoPulso != null) txtDanoPulso.text = "Damage Type: Impact";
            if (txtDanoTesla != null) txtDanoTesla.text = "Damage Type: Damage every 1s (Tick)";
            if (txtDanoTrituradora != null) txtDanoTrituradora.text = "Damage Type: Impact";
            if (txtDanoLaser != null) txtDanoLaser.text = "Damage Type: Incremental Damage";
            if (txtDanoVeneno != null) txtDanoVeneno.text = "Damage Type: Damage over Time (DoT)";
            if (txtDanoPerforante != null) txtDanoPerforante.text = "Damage Type: Impact";

            if (txtDescVelth != null) txtDescVelth.text = "Basic and balanced unit, ideal for forming the first line of siege.";
            if (txtDescVelarth != null) txtDescVelarth.text = "Upgraded version of the Velth with double resistance and greater destructive power.";
            if (txtDescVelariath != null) txtDescVelariath.text = "Formidable assault leader with a massive health pool.";
            if (txtDescPyxer != null) txtDescPyxer.text = "Extremely fast but fragile unit, designed to slip through your defenses.";
            if (txtDescPyxerion != null) txtDescPyxerion.text = "Runner that sacrifices some speed for greater survivability.";
            if (txtDescPyxerarch != null) txtDescPyxerarch.text = "Elusive leader that maintains a fast pace while enduring considerable damage.";
            if (txtDescZhax != null) txtDescZhax.text = "Slow-moving armored giant with a large initial health pool.";
            if (txtDescZhaxeris != null) txtDescZhaxeris.text = "Siege colossus with heavy armor that requires concentrated fire to fall.";
            if (txtDescZhaxarian != null) txtDescZhaxarian.text = "Extremely slow mobile fortress with titanic resistance.";

            if (txtStatsVelth != null) txtStatsVelth.text = "Speed: 175 - Health: 100 - Damage: 75 - Gold: 20";
            if (txtStatsVelarth != null) txtStatsVelarth.text = "Speed: 175 - Health: 200 - Damage: 125 - Gold: 45";
            if (txtStatsVelariath != null) txtStatsVelariath.text = "Speed: 175 - Health: 1500 - Damage: 200 - Gold: 300";
            if (txtStatsPyxer != null) txtStatsPyxer.text = "Speed: 280 - Health: 65 - Damage: 50 - Gold: 5";
            if (txtStatsPyxerion != null) txtStatsPyxerion.text = "Speed: 240 - Health: 130 - Damage: 75 - Gold: 15";
            if (txtStatsPyxerarch != null) txtStatsPyxerarch.text = "Speed: 200 - Health: 300 - Damage: 100 - Gold: 150";
            if (txtStatsZhax != null) txtStatsZhax.text = "Speed: 150 - Health: 555 - Damage: 250 - Gold: 100";
            if (txtStatsZhaxeris != null) txtStatsZhaxeris.text = "Speed: 125 - Health: 888 - Damage: 375 - Gold: 150";
            if (txtStatsZhaxarian != null) txtStatsZhaxarian.text = "Speed: 100 - Health: 3000 - Damage: 500 - Gold: 150";

        }
    }

    private void TraducirLista(TMP_Text[] lista, string texto)
    {
        if (lista == null) return;
        foreach (TMP_Text t in lista)
        {
            if (t != null) t.text = texto;
        }
    }

    public static string ObtenerDificultadTraducida(string dificultadOriginal)
    {
        int idioma = PlayerPrefs.GetInt("IdiomaActual", 0);
        string dif = dificultadOriginal.ToLower();

        if (idioma == 0) return (dif == "easy" || dif == "facil") ? "facil" : (dif == "hard" || dif == "dificil") ? "dificil" : "normal";
        else return (dif == "facil" || dif == "easy") ? "easy" : (dif == "dificil" || dif == "hard") ? "hard" : "normal";
    }

    public static string ObtenerResultadoTraducido(string resultadoOriginal)
    {
        if (string.IsNullOrEmpty(resultadoOriginal)) return "";
        int idioma = PlayerPrefs.GetInt("IdiomaActual", 0);
        string res = resultadoOriginal.ToLower();

        if (idioma == 0)
        {
            if (res.Contains("victoria") || res.Contains("victory")) return "VICTORIA";
            if (res.Contains("derrota") || res.Contains("defeat")) return "DERROTA";
            if (res.Contains("rendicion") || res.Contains("surrender")) return "RENDICIÓN";
            if (res.Contains("infinito") || res.Contains("endless")) return "INFINITO";
        }
        else
        {
            if (res.Contains("victoria") || res.Contains("victory")) return "VICTORY";
            if (res.Contains("derrota") || res.Contains("defeat")) return "DEFEAT";
            if (res.Contains("rendicion") || res.Contains("surrender")) return "SURRENDER";
            if (res.Contains("infinito") || res.Contains("endless")) return "ENDLESS";
        }
        return resultadoOriginal.ToUpper();
    }

    public static string ObtenerTituloFinDePartida(string estado)
    {
        int idioma = PlayerPrefs.GetInt("IdiomaActual", 0);
        if (idioma == 0)
        {
            if (estado == "victoria") return "¡VICTORIA!";
            if (estado == "derrota") return "¡BASE DESTRUIDA!";
            if (estado == "rendicion") return "¡TE HAS RENDIDO!";
        }
        else
        {
            if (estado == "victoria") return "VICTORY!";
            if (estado == "derrota") return "BASE DESTROYED!";
            if (estado == "rendicion") return "YOU SURRENDERED!";
        }
        return estado;
    }

    public static string ObtenerTextoVenderConPrecio(int precio)
    {
        int idioma = PlayerPrefs.GetInt("IdiomaActual", 0);
        if (idioma == 0) return "Vender\n" + precio.ToString() + " Oro";
        else return "Sell\n" + precio.ToString() + " Gold";
    }

    public static string ObtenerEtiquetaInfinita()
    {
        int idioma = PlayerPrefs.GetInt("IdiomaActual", 0);
        return (idioma == 0) ? "Infinita" : "Endless";
    }

    public static string ObtenerErrorLogin()
    {
        int idioma = PlayerPrefs.GetInt("IdiomaActual", 0);
        if (idioma == 0) return "Usuario o contraseña incorrectos.";
        else return "Invalid username or password.";
    }

    public static string ObtenerErrorRegistro()
    {
        int idioma = PlayerPrefs.GetInt("IdiomaActual", 0);
        if (idioma == 0) return "El usuario ya existe.";
        else return "Username already exists.";
    }

    public static string ObtenerErrorCamposVacios()
    {
        int idioma = PlayerPrefs.GetInt("IdiomaActual", 0);
        if (idioma == 0) return "Por favor, rellena todos los campos.";
        else return "Please fill in all fields.";
    }
}