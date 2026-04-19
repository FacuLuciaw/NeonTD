using UnityEngine;
using UnityEngine.UI; // Necesario para que el script reconozca el componente Slider

public class LogicaEnemigo : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    public Transform[] puntos;    // Arrastra aquí tus Waypoints (Punto1, Punto2...)
    public float velocidad = 30f;
    private int indicePunto = 0;

    // --- NUEVA VARIABLE PROTEGIDA PARA LA TESLA ---
    private float velocidadOriginal = -1f; 

    [Header("Configuración de Vida")]
    public float vidaMax = 3f;
    private float vidaActual;
    public Slider barraDeVida;    // Arrastra aquí el Slider que creaste sobre el enemigo

    [Header("Ataque a Base")]
    public float danoABase = 20f; // Cuánto quita a la base

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
        // 1. Si ya llegamos al último punto, el enemigo desaparece
        if (indicePunto >= puntos.Length)
        {
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

    // Esta función la llama la BALA o la TORRE TESLA cuando impacta
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

        if(GestorEconomia.instancia != null)
        {
            GestorEconomia.instancia.SumarOro(oroAlMorir);
        }
            
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D colision)
    {
        if (colision.CompareTag("Base"))
        {
            BasePrincipal baseScript = colision.GetComponent<BasePrincipal>();
            
            if (baseScript != null)
            {
                baseScript.RecibirDano(danoABase);
                Destroy(gameObject);
            }
        }
    }

    // --- SISTEMA DE RALENTIZACIÓN (BLINDADO) ---
    public void AlterarVelocidad(float multiplicador)
    {
        // Si es la primera vez que entramos, guardamos la velocidad real que tenga ahora mismo
        if (velocidadOriginal == -1f) 
        {
            velocidadOriginal = velocidad;
        }
        
        velocidad = velocidadOriginal * multiplicador;
    }

    public void RestaurarVelocidad()
    {
        // Solo restauramos si alguna vez se guardó una velocidad original
        if (velocidadOriginal != -1f)
        {
            velocidad = velocidadOriginal;
        }
    }
}
