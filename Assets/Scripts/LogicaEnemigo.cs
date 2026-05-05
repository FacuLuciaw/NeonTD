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
    public string nombreEnemigo = "Enemigo"; // Nombre específico del enemigo

    // --- NUEVA VARIABLE PARA EL VENENO ---
    // Aquí el enemigo guardará el veneno único para poder borrarlo si le disparan otra vez
    private Coroutine rutinaVenenoUnico; 

    void Start()
    {
        vidaActual = vidaMax;
        if (barraDeVida != null)
        {
            barraDeVida.maxValue = vidaMax;
            barraDeVida.value = vidaMax;
        }
    }

    void Update()
    {
        MoverEnemigo();

        if (zonasTeslaPisadas > 0)
        {
            temporizadorChoqueTesla += Time.deltaTime;
            if (temporizadorChoqueTesla >= 1f)
            {
                RecibirDaño(danoTeslaActual);
                // Restamos 1 segundo exacto para mantener los milisegundos que nos hayamos pasado
                temporizadorChoqueTesla -= 1f; 
            }
        }
        else
        {
            temporizadorChoqueTesla = 0f;
        }
    }

    void MoverEnemigo()
    {
        if (indicePunto >= puntos.Length)
        {
            // Remover el enemigo de activos cuando escapa
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

    public void RecibirDaño(float cantidad)
    {
        vidaActual -= cantidad;
        if (barraDeVida != null) barraDeVida.value = vidaActual;
        if (vidaActual <= 0) Morir();
    }

    void Morir()
    {
        // Registrar la muerte del enemigo
        if (GestorDatosPartida.instancia != null)
        {
            GestorDatosPartida.instancia.RegistrarEnemigo(nombreEnemigo);
            GestorDatosPartida.instancia.RemoverEnemigoActivo(nombreEnemigo);
            // Registrar el daño infligido (vida máxima porque lo eliminamos completamente)
            GestorDatosPartida.instancia.RegistrarDanoInfligido(vidaMax);
        }

        if(GestorEconomia.instancia != null) GestorEconomia.instancia.SumarOro(oroAlMorir);
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

                // Remover el enemigo de activos ya que llegó a la base
                if (GestorDatosPartida.instancia != null)
                {
                    GestorDatosPartida.instancia.RemoverEnemigoActivo(nombreEnemigo);
                    // Registrar el daño infligido (lo que se le quitó de vida, no la vida restante)
                    float danoInfligido = vidaMax - vidaActual;
                    GestorDatosPartida.instancia.RegistrarDanoInfligido(danoInfligido);
                }

                Destroy(gameObject);
            }
        }
    }

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

    public void SalirDeTesla()
    {
        zonasTeslaPisadas--;
        
        if (zonasTeslaPisadas <= 0)
        {
            zonasTeslaPisadas = 0; 
            if (velocidadOriginal != -1f) velocidad = velocidadOriginal;
        }
    }

    // --- SISTEMA DE DAÑO CONTINUO (AHORA CON CHECKBOX) ---
    public void AplicarDañoContinuo(float dps, float duracion, bool seAcumula)
    {
        if (seAcumula)
        {
            // Si SÍ se acumula, simplemente lanzamos uno nuevo sin importar los demás (Como estaba antes)
            StartCoroutine(RutinaDañoContinuo(dps, duracion));
        }
        else
        {
            // Si NO se acumula, miramos si ya estaba envenenado...
            if (rutinaVenenoUnico != null)
            {
                // Si lo estaba, paramos ese veneno viejo...
                StopCoroutine(rutinaVenenoUnico);
            }
            // ...y lanzamos el nuevo, guardándolo en la memoria
            rutinaVenenoUnico = StartCoroutine(RutinaDañoContinuo(dps, duracion));
        }
    }

    private System.Collections.IEnumerator RutinaDañoContinuo(float dps, float duracion)
{
    float tiempoTotalPasado = 0f;
    float cronometroInterno = 0f;

    while (tiempoTotalPasado < duracion)
    {
        // Acumulamos el tiempo del frame actual
        float delta = Time.deltaTime;
        tiempoTotalPasado += delta;
        cronometroInterno += delta;

        // Cada vez que acumulamos 1 segundo...
        if (cronometroInterno >= 1f)
        {
            RecibirDaño(dps);
            cronometroInterno -= 1f; // Restamos el segundo y mantenemos la precisión
        }

        yield return null; // Esperamos al siguiente frame
    }
}
}