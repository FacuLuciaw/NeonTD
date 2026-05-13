using UnityEngine;
using UnityEngine.UI; 

public class LogicaEnemigo : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    public Transform[] puntos;    
    public float velocidad = 30f;
    private int indicePunto = 0;

    private float velocidadOriginal = -1f; 

    private int zonasTeslaPisadas = 0; 
    private float danoTeslaActual = 0f; 
    private float temporizadorChoqueTesla = 0f; 

    [Header("Configuración de Vida")]
    public float vidaMax = 3f;
    private float vidaActual;
    public Slider barraDeVida;    

    [Header("Ataque a Base")]
    public float danoABase = 20f; 

    [Header("Recompensa")]
    public int oroAlMorir = 2; 

    [Header("Identificación")]
    public string nombreEnemigo = "Enemigo"; 

    private Coroutine rutinaVenenoUnico; 

    // Configura la salud inicial del enemigo y ajusta su barra de vida visual
    void Start()
    {
        vidaActual = vidaMax;
        if (barraDeVida != null)
        {
            barraDeVida.maxValue = vidaMax;
            barraDeVida.value = vidaMax;
        }
    }

    // Gestiona el movimiento constante y aplica el daño de las torres Tesla si está sobre una
    void Update()
    {
        MoverEnemigo();

        if (zonasTeslaPisadas > 0)
        {
            temporizadorChoqueTesla += Time.deltaTime;
            if (temporizadorChoqueTesla >= 1f)
            {
                RecibirDano(danoTeslaActual);
                temporizadorChoqueTesla -= 1f; 
            }
        }
        else
        {
            temporizadorChoqueTesla = 0f;
        }
    }

    // Desplaza al enemigo hacia el siguiente WP. Si llega al final desaparece
    void MoverEnemigo()
    {
        if (indicePunto >= puntos.Length)
        {
            if (GestorDatosPartida.instancia != null)
            {
                GestorDatosPartida.instancia.RemoverEnemigoActivo(nombreEnemigo);
            }
            Destroy(gameObject);
            return;
        }

        transform.position = Vector2.MoveTowards(transform.position, puntos[indicePunto].position, velocidad * Time.deltaTime);

        if (Vector2.Distance(transform.position, puntos[indicePunto].position) < 0.1f)
        {
            indicePunto++;
        }
    }

    // Reduce la salud del enemigo, actualiza su barra visual y comprueba si debe morir
    public void RecibirDano(float cantidad)
    {
        vidaActual -= cantidad;
        if (barraDeVida != null) barraDeVida.value = vidaActual;
        if (vidaActual <= 0) Morir();
    }

    // Ejecuta la lógica de eliminación: registra estadísticas, otorga dinero y destruye el objeto
    void Morir()
    {
        if (GestorDatosPartida.instancia != null)
        {
            GestorDatosPartida.instancia.RegistrarEnemigo(nombreEnemigo);
            GestorDatosPartida.instancia.RemoverEnemigoActivo(nombreEnemigo);
            GestorDatosPartida.instancia.RegistrarDanoInfligido(vidaMax);
        }

        if(GestorEconomia.instancia != null) GestorEconomia.instancia.SumarOro(oroAlMorir);
        Destroy(gameObject);
    }

    // Detecta la colisión con la base principal para infligir daño 
    private void OnTriggerEnter2D(Collider2D colision)
    {
        if (colision.CompareTag("Base"))
        {
            BasePrincipal baseScript = colision.GetComponent<BasePrincipal>();
            if (baseScript != null)
            {
                baseScript.RecibirDano(danoABase);

                if (GestorDatosPartida.instancia != null)
                {
                    GestorDatosPartida.instancia.RemoverEnemigoActivo(nombreEnemigo);
                    float danoInfligido = vidaMax - vidaActual;
                    GestorDatosPartida.instancia.RegistrarDanoInfligido(danoInfligido);
                }

                Destroy(gameObject);
            }
        }
    }

    // Reduce la velocidad del enemigo e inicia el ciclo de daño al entrar en el rango de una torre Tesla
    public void EntrarEnTesla(float multiplicador, float danoQueHaceLaTorre)
    {
        zonasTeslaPisadas++;
        
        if (zonasTeslaPisadas == 1) 
        {
            if (velocidadOriginal == -1f) velocidadOriginal = velocidad;
            velocidad = velocidadOriginal * multiplicador;
            danoTeslaActual = danoQueHaceLaTorre;
            temporizadorChoqueTesla = 0.9f; 
        }
    }

    // Restaura la velocidad original del enemigo una vez que sale de todas las zonas Tesla activas
    public void SalirDeTesla()
    {
        zonasTeslaPisadas--;
        
        if (zonasTeslaPisadas <= 0)
        {
            zonasTeslaPisadas = 0; 
            if (velocidadOriginal != -1f) velocidad = velocidadOriginal;
        }
    }

    // Gestiona si el efecto de D.O.T. se acumula en múltiples instancias o si reinicia el actual
    public void AplicarDanoContinuo(float dps, float duracion, bool seAcumula)
    {
        if (seAcumula)
        {
            StartCoroutine(RutinaDanoContinuo(dps, duracion));
        }
        else
        {
            if (rutinaVenenoUnico != null)
            {
                StopCoroutine(rutinaVenenoUnico);
            }
            rutinaVenenoUnico = StartCoroutine(RutinaDanoContinuo(dps, duracion));
        }
    }

    // Corrutina que aplica D.O.T.
    private System.Collections.IEnumerator RutinaDanoContinuo(float dps, float duracion)
    {
        float tiempoTotalPasado = 0f;
        float cronometroInterno = 0f;

        while (tiempoTotalPasado < duracion)
        {
            float delta = Time.deltaTime;
            tiempoTotalPasado += delta;
            cronometroInterno += delta;

            if (cronometroInterno >= 1f)
            {
                RecibirDano(dps);
                cronometroInterno -= 1f; 
            }

            yield return null; 
        }
    }
}