using UnityEngine;
using UnityEngine.UI;

public class BasePrincipal : MonoBehaviour
{
    [Header("Estadísticas")]
    public float vidaMaxima = 100f;
    private float vidaActual;

    private float danoTotalRecibido = 0f;

    [Header("Interfaz (UI)")]
    public Image barraDeVida; 

    [Header("Conexión al Árbitro")]
    public AdministradorNivel adminNivel; // <- ¡NUEVO! Conexión al árbitro


    void Start()
    {
        vidaActual = vidaMaxima;
        ActualizarBarraVida();
    }

    public void RecibirDano(float cantidad)
    {
        danoTotalRecibido += cantidad;
        vidaActual -= cantidad;
    
        vidaActual = Mathf.Clamp(vidaActual, 0, vidaMaxima); 
        ActualizarBarraVida();

        // Registramos el daño en los datos de la partida
        if (GestorDatosPartida.instancia != null)
        {
            GestorDatosPartida.instancia.RegistrarDanoRecibido(cantidad);
        }

        if (vidaActual <= 0)
        {
            Debug.Log("¡GAME OVER!");
            
            // <- ¡NUEVO! Le avisamos al administrador para que salte la derrota
            if (adminNivel != null)
            {
                adminNivel.MostrarDerrota(); 
            }
        }
    }

    void ActualizarBarraVida()
    {
        if (barraDeVida != null)
        {
            barraDeVida.fillAmount = vidaActual / vidaMaxima;
        }
    }

    public float ObtenerDanoTotalRecibido()
    {
        return danoTotalRecibido;
    }
}