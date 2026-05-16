using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class DatosTorre
{
    public string nombre = "Nueva Torre";
    public GameObject prefabReal;
    public GameObject prefabFantasma;
    public int costeActual = 200;
    public float multiplicadorCoste = 1.05f;
    public TextMeshProUGUI textoBoton;
    public Image imagenFondoBoton;
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

    [Header("Economía de Venta")]
    [Range(0f, 100f)]
    public float porcentajeReembolso = 100f;

    private DatosTorre torreAConstruir;
    private int indiceDeTorreAConstruir;
    private GameObject previewActual;
    private bool enModoConstruccion = false;

    void Start()
    {
        ActualizarTodosLosBotones();
    }

    // Activa el modo construcción y genera el preview de la torre
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

    // Comprueba el estado de la economía, actualiza la UI de venta 
    // y gestiona el movimiento del preview durante la construcción
    void Update()
    {
        if (botonVenderUI != null)
        {
            for (int i = 0; i < catalogoTorres.Length; i++)
            {
                if (catalogoTorres[i].imagenFondoBoton != null)
                {
                    if (GestorEconomia.instancia.PuedoComprar(catalogoTorres[i].costeActual))
                    {
                        catalogoTorres[i].imagenFondoBoton.color = Color.white; 
                    }
                    else
                    {
                        catalogoTorres[i].imagenFondoBoton.color = new Color(0.4f, 0.4f, 0.4f, 1f); 
                    }
                }
            }

            if (SeleccionTorre.torreSeleccionadaActual != null)
            {
                botonVenderUI.SetActive(true);

                if (textoPrecioVenta != null)
                {
                    DatosTorre datosTorreSeleccionada = catalogoTorres[SeleccionTorre.torreSeleccionadaActual.indiceCatalogo];

                    int valorUltimaTorre = Mathf.RoundToInt(datosTorreSeleccionada.costeActual / datosTorreSeleccionada.multiplicadorCoste);
                    int valorReembolso = Mathf.RoundToInt(valorUltimaTorre * (porcentajeReembolso / 100f));

                    textoPrecioVenta.text = GestorIdiomas.ObtenerTextoVenderConPrecio(valorReembolso);
                }
            }
            else
            {
                botonVenderUI.SetActive(false);
            }
        }

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

    // Calcula la posición exacta en la cuadrícula basándose en la posición del ratón
    Vector2 ObtenerPosicionCuadricula()
    {
        Vector2 posicionRaton = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float xGrid = Mathf.Round((posicionRaton.x - offsetX) / tamañoCasilla) * tamañoCasilla + offsetX;
        float yGrid = Mathf.Round((posicionRaton.y - offsetY) / tamañoCasilla) * tamañoCasilla + offsetY;
        return new Vector2(xGrid, yGrid);
    }

    // Verifica si la casilla está libre y si hay suficiente oro antes de colocar la torre
    void IntentarConstruir(Vector2 posicion)
    {
        if (!GestorEconomia.instancia.PuedoComprar(torreAConstruir.costeActual))
        {
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
                break;
            }
        }

        if (!ocupado)
        {
            GestorEconomia.instancia.RestarOro(torreAConstruir.costeActual);
            GameObject torreCreada = Instantiate(torreAConstruir.prefabReal, posicion, Quaternion.identity);

            SeleccionTorre scriptSeleccion = torreCreada.GetComponentInChildren<SeleccionTorre>();
            if (scriptSeleccion != null)
            {
                scriptSeleccion.ConfigurarDatosDeCompra(torreAConstruir.costeActual, indiceDeTorreAConstruir);
            }

            if (GestorDatosPartida.instancia != null)
            {
                GestorDatosPartida.instancia.RegistrarTorre(torreAConstruir.nombre);
            }

            torreAConstruir.costeActual = Mathf.RoundToInt(torreAConstruir.costeActual * torreAConstruir.multiplicadorCoste);
            ActualizarTodosLosBotones();

            CancelarConstruccion();
        }
    }

    // Apaga el modo de construcción y elimina el preview
    void CancelarConstruccion()
    {
        enModoConstruccion = false;
        torreAConstruir = null;
        if (previewActual != null)
        {
            Destroy(previewActual);
        }
    }

    // Actualiza visualmente todos los botones de la tienda 
    public void ActualizarTodosLosBotones()
    {
        for (int i = 0; i < catalogoTorres.Length; i++)
        {
            if (catalogoTorres[i].textoBoton != null)
            {
                catalogoTorres[i].textoBoton.text = catalogoTorres[i].costeActual.ToString();
            }

            if (catalogoTorres[i].imagenFondoBoton != null)
            {
                if (GestorEconomia.instancia.PuedoComprar(catalogoTorres[i].costeActual))
                {
                    catalogoTorres[i].imagenFondoBoton.color = Color.white;
                }
                else
                {
                    catalogoTorres[i].imagenFondoBoton.color = new Color(0.4f, 0.4f, 0.4f, 1f);
                }
            }
        }
    }

    // Gestiona la venta de una torre, la devolución de dinero y actualización de datos
    public void VenderTorreSeleccionada()
    {
        SeleccionTorre torre = SeleccionTorre.torreSeleccionadaActual;
        if (torre == null) return;

        DatosTorre datos = catalogoTorres[torre.indiceCatalogo];

        int valorUltimaTorre = Mathf.RoundToInt(datos.costeActual / datos.multiplicadorCoste);
        int valorReembolso = Mathf.RoundToInt(valorUltimaTorre * (porcentajeReembolso / 100f));

        GestorEconomia.instancia.ReembolsoVenta(valorReembolso);

        if (GestorDatosPartida.instancia != null)
        {
            GestorDatosPartida.instancia.DesvincularTorre(datos.nombre);
        }

        datos.costeActual = valorUltimaTorre;

        ActualizarTodosLosBotones();

        torre.Deseleccionar();
        SeleccionTorre.torreSeleccionadaActual = null;
        Destroy(torre.gameObject);
    }
}