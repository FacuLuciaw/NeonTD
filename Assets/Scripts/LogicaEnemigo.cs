using UnityEngine;
using UnityEngine.UI; // Necesario para que el script reconozca el componente Slider

public class LogicaEnemigo : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    public Transform[] puntos;    // Arrastra aquí tus Waypoints (Punto1, Punto2...)
    public float velocidad = 30f;
    private int indicePunto = 0;

    [Header("Configuración de Vida")]
    public float vidaMax = 3f;
    private float vidaActual;
    public Slider barraDeVida;    // Arrastra aquí el Slider que creaste sobre el enemigo

    [Header("Recompensa")]
    public int oroAlMorir = 2; // Por defecto 2 (para el normal)
    void Start()
    {
        // Al empezar, la vida está al máximo
        vidaActual = vidaMax;

        // Configuramos la barrita visual
        if (barraDeVida != null)
        {
            barraDeVida.maxValue = vidaMax;
            barraDeVida.value = vidaMax;
        }
    }

    void Update()
    {
        MoverEnemigo();
    }

    void MoverEnemigo()
    {
        // 1. Si ya llegamos al último punto, el enemigo desaparece (llegó a la base)
        if (indicePunto >= puntos.Length)
        {
            // Aquí podrías restar vida a la BASE del jugador antes de destruir al enemigo
            Destroy(gameObject);
            return;
        }

        // 2. Moverse hacia el punto actual
        transform.position = Vector2.MoveTowards(transform.position, puntos[indicePunto].position, velocidad * Time.deltaTime);

        // 3. Si estamos muy cerca del punto, pasamos al siguiente
        if (Vector2.Distance(transform.position, puntos[indicePunto].position) < 0.1f)
        {
            indicePunto++;
        }
    }

    // Esta función la llama la BALA cuando impacta
    public void RecibirDaño(float cantidad)
    {
        vidaActual -= cantidad;

        // Actualizamos la barra visualmente
        if (barraDeVida != null)
        {
            barraDeVida.value = vidaActual;
        }

        // Si la vida llega a cero, el enemigo muere
        if (vidaActual <= 0)
        {
            Morir();
        }
    }

    void Morir()
    {
        Debug.Log("Enemigo destruido");
        
        // Avisamos al banco de que sume el dinero
        GestorEconomia.instancia.SumarOro(oroAlMorir);
        
        Destroy(gameObject);
    }
}