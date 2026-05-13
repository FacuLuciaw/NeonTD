using UnityEngine;
using TMPro;

public class GestorEconomia : MonoBehaviour
{
    public static GestorEconomia instancia;

    [Header("Dinero")]
    public int oroActual = 0;
    public float multiplicadorOro = 1f; 
    public TextMeshProUGUI textoOro;

    // Configura el sistema para que solo exista un gestor de economía en toda la partida
    void Awake()
    {
        if (instancia == null) 
        {
            instancia = this;
        } 
        else 
        {
            Destroy(gameObject);
        }
    }

    // Mostramos la cantidad de oro inicial en la pantalla
    void Start()
    {
        ActualizarTextoUI();
    }

    // Añade oro al total aplicando los multiplicadoress
    public void SumarOro(int cantidad)
    {
        int cantidadFinal = Mathf.RoundToInt(cantidad * multiplicadorOro);

        oroActual += cantidadFinal;
        
        if (GestorDatosPartida.instancia != null)
        {
            GestorDatosPartida.instancia.RegistrarOroGanado(cantidadFinal);
        }
        
        ActualizarTextoUI();
    }

    // Comprueba si el jugador tiene suficiente dinero para comprar
    public bool PuedoComprar(int coste)
    {
        return oroActual >= coste;
    }

    // Resta el dinero del total y lo registra
    public void RestarOro(int coste)
    {
        oroActual -= coste;
        
        if (GestorDatosPartida.instancia != null)
        {
            GestorDatosPartida.instancia.RegistrarOroGastado(coste);
        }
        
        ActualizarTextoUI();
    }

    // Devuelve el dinero al vender una torre, ajustando el contador de gasto total
    public void ReembolsoVenta(int cantidad)
    {
        oroActual += cantidad;
        
        if (GestorDatosPartida.instancia != null)
        {
            GestorDatosPartida.instancia.RestarOroGastado(cantidad);
        }
        
        ActualizarTextoUI();
    }

    void ActualizarTextoUI()
    {
        if (textoOro != null)
        {
            textoOro.text = oroActual.ToString();
        }
    }
}