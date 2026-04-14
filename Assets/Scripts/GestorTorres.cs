using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class GestorTorres : MonoBehaviour
{
    [Header("Configuración de Construcción")]
    public GameObject torrePrefab;
    public LayerMask capasBloqueadas;

    [Header("Economía Dinámica")]
    public int costeActualTorre = 200;
    public float multiplicadorCoste = 1.05f;
    public TextMeshProUGUI textoBotonTorre;

    [Header("Ajuste de Cuadrícula")]
    public float tamañoCasilla = 100f; // Ahora la matemática sabe que tus cuadrados son gigantes
    public float offsetX = 50f;      // La mitad de 100 (para ir al centro)
    public float offsetY = 50f;      // La mitad de 100 (para ir al centro)

    private bool enModoConstruccion = false;

    void Start()
    {
        ActualizarTextoBoton();
    }

    public void ActivarModoConstruccion()
    {
        enModoConstruccion = true;
    }

    void Update()
    {
        if (enModoConstruccion && Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;
            IntentarConstruir();
        }
    }

   void IntentarConstruir()
    {
        if (!GestorEconomia.instancia.PuedoComprar(costeActualTorre))
        {
            Debug.Log("No tienes suficiente oro.");
            enModoConstruccion = false;
            return;
        }

        Vector2 posicionRaton = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // --- MATEMÁTICA EXACTA PARA CASILLAS DE 100x100 ---
        float xGrid = Mathf.Round((posicionRaton.x - offsetX) / tamañoCasilla) * tamañoCasilla + offsetX;
        float yGrid = Mathf.Round((posicionRaton.y - offsetY) / tamañoCasilla) * tamañoCasilla + offsetY;
        Vector2 posicionCuadricula = new Vector2(xGrid, yGrid);

        // --- EL FILTRO PROFESIONAL (Ignorar Triggers) ---
        // Cogemos TODO lo que hay en esa casilla
        Collider2D[] obstaculos = Physics2D.OverlapCircleAll(posicionCuadricula, 10f, capasBloqueadas);
        bool sitioOcupado = false;

        foreach (Collider2D col in obstaculos)
        {
            // Si el collider NO es un trigger (es decir, es una base física sólida)
            if (!col.isTrigger) 
            {
                sitioOcupado = true;
                Debug.LogWarning("Bloqueado por la base sólida de: " + col.name);
                break; // Paramos de buscar porque ya sabemos que está ocupado
            }
        }

        if (!sitioOcupado)
        {
            GestorEconomia.instancia.RestarOro(costeActualTorre);
            Instantiate(torrePrefab, posicionCuadricula, Quaternion.identity);
            enModoConstruccion = false;
            SubirPrecioTorre();
        }
    }

    void SubirPrecioTorre()
    {
        costeActualTorre = Mathf.RoundToInt(costeActualTorre * multiplicadorCoste);
        ActualizarTextoBoton();
    }

    public void ActualizarTextoBoton()
    {
        if (textoBotonTorre != null)
        {
            textoBotonTorre.text = "Crear Torre\n(" + costeActualTorre + " Oro)";
        }
    }
}