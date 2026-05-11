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