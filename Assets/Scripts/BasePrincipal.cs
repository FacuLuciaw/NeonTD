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
    public AdministradorNivel adminNivel; 

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

        if (GestorDatosPartida.instancia != null)
        {
            GestorDatosPartida.instancia.RegistrarDanoRecibido(cantidad);
        }

        if (vidaActual <= 0)
        {
            if (adminNivel != null && !adminNivel.juegoFinalizado)
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