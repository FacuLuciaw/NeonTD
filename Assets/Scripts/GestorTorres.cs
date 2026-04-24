using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

// Creamos un "molde" para guardar los datos de cualquier torre
[System.Serializable] 
public class DatosTorre
{
    public string nombre = "Nueva Torre"; // Para el texto del botón
    public GameObject prefabReal;
    public GameObject prefabFantasma;
    public int costeActual = 200;
    public float multiplicadorCoste = 1.05f;
    public TextMeshProUGUI textoBoton;
}

public class GestorTorres : MonoBehaviour
{
    [Header("Catálogo de Torres")]
    [Tooltip("Añade aquí todas las torres que quieras que tenga el juego")]
    public DatosTorre[] catalogoTorres; 

    [Header("Configuración de Construcción")]
    public LayerMask capasBloqueadas;
    public float tamañoCasilla = 100f; 
    public float offsetX = 50f;      
    public float offsetY = 50f;      

    [Header("Conexión al Registro de Partida")]
    public PartidaJSON datosPartida; // <- Para registrar las torres construidas

    private DatosTorre torreAConstruir; // La torre que hemos elegido ahora mismo
    private GameObject previewActual; 
    private bool enModoConstruccion = false;

    void Start()
    {
        ActualizarTodosLosBotones();
    }

    // ¡ESTA ES LA MAGIA NUEVA! 
    // Recibe un número: 0 es la primera torre, 1 es la segunda, etc.
    public void SeleccionarTorre(int indiceTorre)
    {
        if (indiceTorre < 0 || indiceTorre >= catalogoTorres.Length) return;

        // Si ya estábamos construyendo otra cosa, cancelamos
        if (enModoConstruccion) CancelarConstruccion();

        // Cargamos los datos de la torre que queremos
        torreAConstruir = catalogoTorres[indiceTorre];
        enModoConstruccion = true;
        
        // Creamos su fantasma específico
        if (torreAConstruir.prefabFantasma != null)
        {
            previewActual = Instantiate(torreAConstruir.prefabFantasma);
        }
    }

    void Update()
    {
        if (!enModoConstruccion || torreAConstruir == null) return;

        Vector2 posSnapped = ObtenerPosicionCuadricula();
        
        if (previewActual != null)
        {
            previewActual.transform.position = posSnapped;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;
            IntentarConstruir(posSnapped);
        }

        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))
        {
            CancelarConstruccion();
        }
    }

    Vector2 ObtenerPosicionCuadricula()
    {
        Vector2 posicionRaton = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float xGrid = Mathf.Round((posicionRaton.x - offsetX) / tamañoCasilla) * tamañoCasilla + offsetX;
        float yGrid = Mathf.Round((posicionRaton.y - offsetY) / tamañoCasilla) * tamañoCasilla + offsetY;
        return new Vector2(xGrid, yGrid);
    }

    void IntentarConstruir(Vector2 posicion)
    {
        // Comprobamos el precio de la torre seleccionada
        if (!GestorEconomia.instancia.PuedoComprar(torreAConstruir.costeActual))
        {
            Debug.Log("No hay oro suficiente.");
            CancelarConstruccion();
            return;
        }

        Collider2D[] obstaculos = Physics2D.OverlapPointAll(posicion, capasBloqueadas);
        bool ocupado = false;

        foreach (Collider2D col in obstaculos)
        {
            if (previewActual != null && col.gameObject == previewActual) continue;
            
            if (!col.isTrigger) 
            {
                ocupado = true;
                Debug.LogWarning("Bloqueado por: " + col.name);
                break;
            }
        }

        if (!ocupado)
        {
            // Cobramos y construimos
            GestorEconomia.instancia.RestarOro(torreAConstruir.costeActual);
            Instantiate(torreAConstruir.prefabReal, posicion, Quaternion.identity);
            
            // Registramos la torre construida en los datos de la partida
            if (datosPartida != null)
            {
                datosPartida.RegistrarTorre(torreAConstruir.nombre);
                Debug.Log("✓ Torre registrada: " + torreAConstruir.nombre + " | Total torres: " + datosPartida.torres.Count);
                
                // Mostrar el estado actual de las torres
                foreach (DetalleTorre t in datosPartida.torres)
                {
                    Debug.Log("  └─ " + t.nombre + " x" + t.cantidad);
                }
            }
            else
            {
                Debug.LogWarning("⚠ datosPartida no está asignado en GestorTorres");
            }
            
            // Subimos el precio SOLO de la torre que acabamos de poner
            torreAConstruir.costeActual = Mathf.RoundToInt(torreAConstruir.costeActual * torreAConstruir.multiplicadorCoste);
            ActualizarTodosLosBotones();
            
            CancelarConstruccion(); 
        }
    }

    void CancelarConstruccion()
    {
        enModoConstruccion = false;
        torreAConstruir = null; // Vaciamos la selección
        if (previewActual != null) 
        {
            Destroy(previewActual);
        }
    }

    public void ActualizarTodosLosBotones()
    {
        for (int i = 0; i < catalogoTorres.Length; i++)
        {
            if (catalogoTorres[i].textoBoton != null)
            {
                catalogoTorres[i].textoBoton.text = catalogoTorres[i].nombre + "\n(" + catalogoTorres[i].costeActual + " Oro)";
            }
        }
    }
}