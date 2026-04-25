using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

[System.Serializable] 
public class DatosTorre
{
    public string nombre = "Nueva Torre"; 
    public GameObject prefabReal;
    public GameObject prefabFantasma;
    public int costeActual = 200;
    public float multiplicadorCoste = 1.05f;
    public TextMeshProUGUI textoBoton;
}

public class GestorTorres : MonoBehaviour
{
    [Header("Catálogo de Torres")]
    public DatosTorre[] catalogoTorres; 

    [Header("Configuración de Construcción")]
    public LayerMask capasBloqueadas;
    public float tamañoCasilla = 100f; 
    public float offsetX = 50f;      
    public float offsetY = 50f;      

    [Header("Interfaz de Venta")]
    public GameObject botonVenderUI; 
    public TextMeshProUGUI textoPrecioVenta; 
    public Vector3 offsetBotonVenta = new Vector3(0f, -1.5f, 0f); 

    // --- MAGIA NUEVA: Porcentaje de Venta ---
    [Header("Economía de Venta")]
    [Tooltip("100 = Devuelve el valor total. 50 = Devuelve la mitad.")]
    [Range(0f, 100f)] 
    public float porcentajeReembolso = 100f;
    // ----------------------------------------

    private DatosTorre torreAConstruir; 
    private int indiceDeTorreAConstruir; 
    private GameObject previewActual; 
    private bool enModoConstruccion = false;

    void Start()
    {
        ActualizarTodosLosBotones();
    }

    public void SeleccionarTorre(int indiceTorre)
    {
        if (indiceTorre < 0 || indiceTorre >= catalogoTorres.Length) return;

        if (enModoConstruccion) CancelarConstruccion();

        torreAConstruir = catalogoTorres[indiceTorre];
        indiceDeTorreAConstruir = indiceTorre; 
        enModoConstruccion = true;
        
        if (torreAConstruir.prefabFantasma != null)
        {
            previewActual = Instantiate(torreAConstruir.prefabFantasma);
        }
    }

    void Update()
    {
        // --- MOSTRAR Y POSICIONAR EL BOTÓN DE VENDER ---
        if (botonVenderUI != null)
        {
            if (SeleccionTorre.torreSeleccionadaActual != null)
            {
                botonVenderUI.SetActive(true);
                
                // Calculamos el precio en tiempo real basado en la tienda, no en la torre
                if (textoPrecioVenta != null) 
                {
                    DatosTorre datosTorreSeleccionada = catalogoTorres[SeleccionTorre.torreSeleccionadaActual.indiceCatalogo];
                    
                    // 1. Averiguamos cuánto costó la última que se puso
                    int valorUltimaTorre = Mathf.RoundToInt(datosTorreSeleccionada.costeActual / datosTorreSeleccionada.multiplicadorCoste);
                    
                    // 2. Le aplicamos tu porcentaje (ej: el 75% de 1000)
                    int valorReembolso = Mathf.RoundToInt(valorUltimaTorre * (porcentajeReembolso / 100f));
                    
                    textoPrecioVenta.text = "Vender\n(+" + valorReembolso + " Oro)";
                }

                Vector3 posicionEnMundo = SeleccionTorre.torreSeleccionadaActual.transform.position + offsetBotonVenta;
                Vector3 posicionEnPantalla = Camera.main.WorldToScreenPoint(posicionEnMundo);
                botonVenderUI.transform.position = posicionEnPantalla;
            }
            else
            {
                botonVenderUI.SetActive(false); 
            }
        }
        // --------------------------------------------------

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
            GestorEconomia.instancia.RestarOro(torreAConstruir.costeActual);
            GameObject torreCreada = Instantiate(torreAConstruir.prefabReal, posicion, Quaternion.identity);
            
            // Seguimos pasándole el índice para saber de qué tipo es (Tesla, Láser, etc)
            SeleccionTorre scriptSeleccion = torreCreada.GetComponentInChildren<SeleccionTorre>();
            if (scriptSeleccion != null)
            {
                scriptSeleccion.ConfigurarDatosDeCompra(torreAConstruir.costeActual, indiceDeTorreAConstruir);
            }

            if (GestorDatosPartida.instancia != null)
            {
                GestorDatosPartida.instancia.RegistrarTorre(torreAConstruir.nombre);
                Debug.Log("✓ Torre registrada: " + torreAConstruir.nombre + " | Total torres: " + GestorDatosPartida.instancia.datosPartida.torres.Count);
                
                // Mostrar el estado actual de las torres
                foreach (DetalleTorre t in GestorDatosPartida.instancia.datosPartida.torres)
                {
                    Debug.Log("  └─ " + t.nombre + " x" + t.cantidad);
                }
            }
            
            torreAConstruir.costeActual = Mathf.RoundToInt(torreAConstruir.costeActual * torreAConstruir.multiplicadorCoste);
            ActualizarTodosLosBotones();
            
            CancelarConstruccion(); 
        }
    }

    void CancelarConstruccion()
    {
        enModoConstruccion = false;
        torreAConstruir = null; 
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

    public void VenderTorreSeleccionada()
    {
        SeleccionTorre torre = SeleccionTorre.torreSeleccionadaActual;
        if (torre == null) return;

        DatosTorre datos = catalogoTorres[torre.indiceCatalogo];

        // 1. Calculamos cuánto costó la ÚLTIMA torre de este tipo que pusimos
        int valorUltimaTorre = Mathf.RoundToInt(datos.costeActual / datos.multiplicadorCoste);
        
        // 2. Le aplicamos el porcentaje de penalización por vender
        int valorReembolso = Mathf.RoundToInt(valorUltimaTorre * (porcentajeReembolso / 100f));

        // 3. Devolvemos el dinero al jugador (sin contar como ganancia)
        GestorEconomia.instancia.ReembolsoVenta(valorReembolso);

        // Desvinculamos la torre de los datos de la partida
        if (GestorDatosPartida.instancia != null)
        {
            GestorDatosPartida.instancia.DesvincularTorre(datos.nombre);
            Debug.Log("✗ Torre vendida: " + datos.nombre + " | Reembolso: +" + valorReembolso + " Oro");
            Debug.Log("💰 Oro Gastado actualizado de forma automática");
            
            // Mostrar el estado actual de las torres
            foreach (DetalleTorre t in GestorDatosPartida.instancia.datosPartida.torres)
            {
                Debug.Log("  └─ " + t.nombre + " x" + t.cantidad);
            }
        }

        // 4. Bajamos el precio de la tienda al escalón anterior (sin importar el porcentaje de reembolso)
        datos.costeActual = valorUltimaTorre;
        
        ActualizarTodosLosBotones();

        torre.Deseleccionar(); 
        SeleccionTorre.torreSeleccionadaActual = null;
        Destroy(torre.gameObject); 
    }
}