using UnityEngine;
using TMPro;
using UnityEngine.UI; // <- ¡NUEVO! Necesario para trabajar con Imágenes

public class GestorIdiomas : MonoBehaviour
{
    [Header("Icono del Botón de Idioma")]
    public Image iconoBandera; 
    public Sprite banderaEspana;
    public Sprite banderaInglaterra;

    [Header("Pantalla: Menú Principal")]
    public TMP_Text txtTituloHistorial;
    public TMP_Text txtTituloConfiguracion;
    public TMP_Text txtBtnJugar;
    public TMP_Text txtBtnHistorial;
    public TMP_Text txtBtnConfiguracion;

    [Header("Pantalla: Jugar")]
    public TMP_Text txtNuevaPartida;

    [Header("Pantalla: Login")]
    [Header("Textos Únicos (Login & Registro)")]
    public TMP_Text txtIniciarSesion;
    public TMP_Text txtCrearCuenta;
    public TMP_Text txtBtnContinuar; 

    [Header("Textos Repetidos (Etiquetas)")]
    [Tooltip("Arrastra aquí los textos 'Usuario' de Login y Registro")]
    public TMP_Text[] listaTxtUsuario;
    
    [Tooltip("Arrastra aquí los textos 'Contraseña' de Login y Registro")]
    public TMP_Text[] listaTxtContrasena;

    [Header("Textos Repetidos (Placeholders)")]
    [Tooltip("Arrastra aquí los placeholders de Usuario")]
    public TMP_Text[] listaPlaceholderUsuario;
    
    [Tooltip("Arrastra aquí los placeholders de Contraseña")]
    public TMP_Text[] listaPlaceholderContrasena;

    [Header("Botones Repetidos")]
    [Tooltip("Arrastra aquí todos los textos de botones que digan 'Salir'")]
    public TMP_Text[] listaBtnSalir; 
    
    [Tooltip("Arrastra aquí todos los textos de botones que digan 'Crear'")]
    public TMP_Text[] listaBtnCrear;

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
    }

    private void ActualizarTextos()
    {
        int idioma = PlayerPrefs.GetInt("IdiomaActual", 0);

        if (idioma == 0) // --- ESPAÑOL ---
        {
            // --- NUEVO: Cambiar la bandera ---
            if (iconoBandera != null && banderaEspana != null) 
            {
                iconoBandera.sprite = banderaEspana;
            }

            // Menú Principal
            if (txtTituloHistorial != null) txtTituloHistorial.text = "Historial";
            if (txtTituloConfiguracion != null) txtTituloConfiguracion.text = "Configuración";
            if (txtBtnJugar != null) txtBtnJugar.text = "Jugar";
            if (txtBtnHistorial != null) txtBtnHistorial.text = "Historial";
            if (txtBtnConfiguracion != null) txtBtnConfiguracion.text = "Configuracion";

            // Pantalla: Jugar
            if (txtNuevaPartida != null) txtNuevaPartida.text = "Nueva Partida";

            // Textos Únicos
            if (txtIniciarSesion != null) txtIniciarSesion.text = "Iniciar sesión";
            if (txtCrearCuenta != null) txtCrearCuenta.text = "Crear Cuenta";
            if (txtBtnContinuar != null) txtBtnContinuar.text = "Continuar"; 

            // Listas Dinámicas (Etiquetas y Placeholders)
            TraducirLista(listaTxtUsuario, "Usuario");
            TraducirLista(listaTxtContrasena, "Contraseña");
            TraducirLista(listaPlaceholderUsuario, "Ingrese su usuario...");
            TraducirLista(listaPlaceholderContrasena, "Ingrese su contraseña...");

            // Listas Dinámicas (Botones)
            TraducirLista(listaBtnSalir, "Salir"); 
            TraducirLista(listaBtnCrear, "Crear");
        }
        else // --- INGLÉS ---
        {
            // --- NUEVO: Cambiar la bandera ---
            if (iconoBandera != null && banderaInglaterra != null) 
            {
                iconoBandera.sprite = banderaInglaterra;
            }

            // Menú Principal
            if (txtTituloHistorial != null) txtTituloHistorial.text = "History";
            if (txtTituloConfiguracion != null) txtTituloConfiguracion.text = "Settings";
            if (txtBtnJugar != null) txtBtnJugar.text = "Play";
            if (txtBtnHistorial != null) txtBtnHistorial.text = "History";
            if (txtBtnConfiguracion != null) txtBtnConfiguracion.text = "Settings";

            // Pantalla: Jugar
            if (txtNuevaPartida != null) txtNuevaPartida.text = "New Game";

            // Textos Únicos
            if (txtIniciarSesion != null) txtIniciarSesion.text = "Login";
            if (txtCrearCuenta != null) txtCrearCuenta.text = "Create Account";
            if (txtBtnContinuar != null) txtBtnContinuar.text = "Continue"; 

            // Listas Dinámicas (Etiquetas y Placeholders)
            TraducirLista(listaTxtUsuario, "Username");
            TraducirLista(listaTxtContrasena, "Password");
            TraducirLista(listaPlaceholderUsuario, "Enter username...");
            TraducirLista(listaPlaceholderContrasena, "Enter password...");

            // Listas Dinámicas (Botones)
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

        if (idioma == 0) // ESPAÑOL
        {
            if (dif == "easy" || dif == "facil") return "facil";
            if (dif == "normal") return "normal";
            if (dif == "hard" || dif == "dificil") return "dificil";
        }
        else // INGLÉS
        {
            if (dif == "facil" || dif == "easy") return "easy";
            if (dif == "normal") return "normal";
            if (dif == "dificil" || dif == "hard") return "hard";
        }
        return dificultadOriginal;
    }
}