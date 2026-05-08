using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; 

public class AdministradorNivel : MonoBehaviour
{
    [Header("Pantallas")]
    public GameObject hudPrincipal; 
    public GameObject pantallaFinJuego; 
    public GameObject pantallaEstadisticas; 
    
    // --- NUEVO: PANTALLA DE CONFIGURACIÓN / PAUSA ---
    public GameObject pantallaConfiguracion; 
    // ------------------------------------------------

    [Header("Conexiones")]
    public GeneradorEnemigos generadorEnemigos; 

    [Header("Botones Especiales")]
    public GameObject botonModoInfinito; 

    [Header("Textos")]
    public TMP_Text txtResultado;

    [Header("Estado del juego")]
    public bool juegoFinalizado = false;
    public bool enModoInfinito = false; 
    
    // --- NUEVO: Control de pausa ---
    private bool juegoPausado = false;
    // -------------------------------

    void Start()
    {
        if (hudPrincipal != null) hudPrincipal.SetActive(true);
        if (pantallaFinJuego != null) pantallaFinJuego.SetActive(false);
        if (botonModoInfinito != null) botonModoInfinito.SetActive(false);
        
        // Nos aseguramos de que el menú de configuración esté apagado al empezar
        if (pantallaConfiguracion != null) pantallaConfiguracion.SetActive(false);
    }

    // --- NUEVO: VIGILAMOS EL TECLADO EN TODO MOMENTO ---
    void Update()
    {
        // Si el juego ya terminó (ganaste o perdiste), no hacemos nada con el ESC
        if (juegoFinalizado) return;

        // Si el jugador pulsa la tecla Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            AlternarPausa();
        }
    }

    // --- NUEVO: INTERRUPTOR DE PAUSA ---
    public void AlternarPausa()
    {
        juegoPausado = !juegoPausado; // Invertimos el estado (de falso a verdadero y viceversa)

        if (juegoPausado)
        {
            // PAUSAR EL JUEGO
            Time.timeScale = 0f; // Congelamos el tiempo
            if (pantallaConfiguracion != null) pantallaConfiguracion.SetActive(true); // Mostramos el menú
        }
        else
        {
            // REANUDAR EL JUEGO
            Time.timeScale = 1f; // Descongelamos el tiempo
            if (pantallaConfiguracion != null) pantallaConfiguracion.SetActive(false); // Ocultamos el menú
        }
    }
    // ------------------------------------

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
        if (pantallaConfiguracion != null) pantallaConfiguracion.SetActive(false); // Por seguridad, ocultamos config

        txtResultado.text = "¡BASE DESTRUIDA!";
        txtResultado.color = Color.red; 
        Time.timeScale = 0f; 
    }

    public void MostrarVictoria()
    {
        if (juegoFinalizado) return;
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
        if (pantallaConfiguracion != null) pantallaConfiguracion.SetActive(false); // Por seguridad, ocultamos config

        txtResultado.text = "¡VICTORIA!";
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
        if (pantallaConfiguracion != null) pantallaConfiguracion.SetActive(false); // Por seguridad, ocultamos config

        txtResultado.text = "¡TE HAS RENDIDO!"; 
        txtResultado.color = new Color(1f, 0.5f, 0f); 
        Time.timeScale = 0f; 
    }

    public void IniciarModoInfinito()
    {
        juegoFinalizado = false; 
        enModoInfinito = true;   

        if (generadorEnemigos != null)
        {
            generadorEnemigos.ActivarModoInfinito();
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