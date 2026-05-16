using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; 

public class AdministradorNivel : MonoBehaviour
{
    [Header("Pantallas")]
    public GameObject hudPrincipal; 
    public GameObject pantallaFinJuego; 
    public GameObject pantallaEstadisticas; 
    public GameObject pantallaConfiguracion; 

    // Ya no usamos esta variable, pero la dejamos por si acaso para no romper conexiones en el Inspector
    [Header("Conexiones (Ya no es necesaria, busca automáticamente)")]
    public GeneradorEnemigos generadorEnemigos; 

    [Header("Botones Especiales")]
    public GameObject botonModoInfinito; 

    [Header("Textos")]
    public TMP_Text txtResultado;
    public TMP_Text txtTituloEstadisticas;

    [Header("Estado del juego")]
    public bool juegoFinalizado = false;
    public bool enModoInfinito = false; 
    
    private bool juegoPausado = false;

    // NUEVO: Creamos una referencia para poder comunicarnos con el script del tiempo
    [Header("Referencias de Control")]
    public ControladorTiempo controladorTiempo;

    void Start()
    {
        if (hudPrincipal != null) hudPrincipal.SetActive(true);
        if (pantallaFinJuego != null) pantallaFinJuego.SetActive(false);
        if (botonModoInfinito != null) botonModoInfinito.SetActive(false);
        if (pantallaConfiguracion != null) pantallaConfiguracion.SetActive(false);

        if (txtTituloEstadisticas != null)
        {
            int idioma = PlayerPrefs.GetInt("IdiomaActual", 0);
            txtTituloEstadisticas.text = (idioma == 0) ? "Estadísticas" : "Statistics";
        }
    }

    void Update()
    {
        if (juegoFinalizado) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            AlternarPausa();
        }
    }

    public void AlternarPausa()
    {
        juegoPausado = !juegoPausado; 

        if (juegoPausado)
        {
            Time.timeScale = 0f; 
            if (pantallaConfiguracion != null) pantallaConfiguracion.SetActive(true); 
        }
        else
        {
            if (controladorTiempo != null)
            {
                controladorTiempo.ReaplicarVelocidadActual();
            }
            else
            {
                Time.timeScale = 1f;
            }

            if (pantallaConfiguracion != null) pantallaConfiguracion.SetActive(false); 
        }
    }

    public void MostrarDerrota()
    {
        if (juegoFinalizado) return;
        juegoFinalizado = true;

        if (GestorDatosPartida.instancia != null)
        {
            GestorDatosPartida.instancia.EstablecerEstado("derrota");
            GestorDatosPartida.instancia.GuardarPartidaAWS();
        }

        if (hudPrincipal != null) hudPrincipal.SetActive(false); 
        if (botonModoInfinito != null) botonModoInfinito.SetActive(false); 

        pantallaFinJuego.SetActive(true);
        pantallaEstadisticas.SetActive(false); 
        if (pantallaConfiguracion != null) pantallaConfiguracion.SetActive(false); 

        int idioma = PlayerPrefs.GetInt("IdiomaActual", 0);
        txtResultado.text = (idioma == 0) ? "¡BASE DESTRUIDA!" : "BASE DESTROYED!";
        txtResultado.color = Color.red; 
        Time.timeScale = 0f; 
    }

    public void MostrarVictoria()
    {
        if (juegoFinalizado || enModoInfinito) return; 
        
        juegoFinalizado = true;

        if (GestorDatosPartida.instancia != null)
        {
            GestorDatosPartida.instancia.EstablecerEstado("victoria");
            GestorDatosPartida.instancia.GuardarPartidaAWS();
        }

        if (hudPrincipal != null) hudPrincipal.SetActive(false); 

        if (botonModoInfinito != null) botonModoInfinito.SetActive(true);

        pantallaFinJuego.SetActive(true);
        pantallaEstadisticas.SetActive(false); 
        if (pantallaConfiguracion != null) pantallaConfiguracion.SetActive(false); 

        int idioma = PlayerPrefs.GetInt("IdiomaActual", 0);
        txtResultado.text = (idioma == 0) ? "¡VICTORIA!" : "VICTORY!";
        txtResultado.color = Color.green; 
        Time.timeScale = 0f; 
    }

    public void Rendirse()
    {
        if (juegoFinalizado) return;
        juegoFinalizado = true;

        if (GestorDatosPartida.instancia != null)
        {
            GestorDatosPartida.instancia.EstablecerEstado("rendicion");
            GestorDatosPartida.instancia.GuardarPartidaAWS();
        }

        if (hudPrincipal != null) hudPrincipal.SetActive(false); 
        if (botonModoInfinito != null) botonModoInfinito.SetActive(false); 

        pantallaFinJuego.SetActive(true);
        pantallaEstadisticas.SetActive(false); 
        if (pantallaConfiguracion != null) pantallaConfiguracion.SetActive(false); 

        int idioma = PlayerPrefs.GetInt("IdiomaActual", 0);
        txtResultado.text = (idioma == 0) ? "¡TE HAS RENDIDO!" : "YOU SURRENDERED!"; 
        txtResultado.color = new Color(1f, 0.5f, 0f); 
        Time.timeScale = 0f; 
    }

    public void IniciarModoInfinito()
    {
        juegoFinalizado = false; 
        enModoInfinito = true;

        GeneradorEnemigos[] todosLosSpawners = FindObjectsByType<GeneradorEnemigos>(FindObjectsSortMode.None);
        foreach (GeneradorEnemigos spawner in todosLosSpawners)
        {
            // Activamos el modo infinito en todos los caminos a la vez
            spawner.ActivarModoInfinitoEnTodosLosScripts();
        }

        if (GestorDatosPartida.instancia != null)
        {
            GestorDatosPartida.instancia.ResetearParaModoInfinito();
        }

        pantallaFinJuego.SetActive(false); 

        if (hudPrincipal != null) hudPrincipal.SetActive(true); 

        Time.timeScale = 1f; 
    }

    public void AbrirEstadisticas()
    {
        pantallaFinJuego.SetActive(false);     
        pantallaEstadisticas.SetActive(true);  
    }

    public void VolverAlMenuPrincipal()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene("InterfazUsuario"); 
    }

    public void VolverASeleccionMapa()
    {
        Time.timeScale = 1f; 
        PlayerPrefs.SetInt("AbrirCarrusel", 1); 
        PlayerPrefs.Save(); 
        SceneManager.LoadScene("InterfazUsuario"); 
    }

    public void VolverAFinDePartida()
    {
        pantallaEstadisticas.SetActive(false); 
        pantallaFinJuego.SetActive(true);      
    }
}