using UnityEngine;
using TMPro; // Necesario para modificar los textos de la UI modernas

public class GestorEconomia : MonoBehaviour
{
    // Esto crea un "Singleton", una forma fácil de acceder a este script desde cualquier lado
    public static GestorEconomia instancia;

    [Header("Dinero")]
    public int oroActual = 15; // Empezamos con 15 para que puedas poner un par de torres al inicio
    
    [Tooltip("Multiplica el oro ganado. Ej: 1 = Normal, 2 = El Doble, 1.5 = 50% más")]
    public float multiplicadorOro = 1f; 
    
    public TextMeshProUGUI textoOro; // El texto de la pantalla

    void Awake()
    {
        // Configuramos el Singleton
        if (instancia == null) {
            instancia = this;
        } else {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        ActualizarTextoUI();
    }

    // Los enemigos llamarán a esta función al morir
    public void SumarOro(int cantidad)
    {
        // Aplicamos el multiplicador y redondeamos al entero más cercano
        int cantidadFinal = Mathf.RoundToInt(cantidad * multiplicadorOro);

        oroActual += cantidadFinal;
        
        // Registramos el dinero ganado en la partida usando la cantidad ya multiplicada
        if (GestorDatosPartida.instancia != null)
        {
            GestorDatosPartida.instancia.RegistrarOroGanado(cantidadFinal);
        }
        
        ActualizarTextoUI();
    }

    // El gestor de torres usará esto para saber si somos ricos o no
    public bool PuedoComprar(int coste)
    {
        return oroActual >= coste;
    }

    // El gestor de torres usará esto al colocar la torre
    public void RestarOro(int coste)
    {
        oroActual -= coste;
        
        // Registramos el dinero gastado en la partida
        if (GestorDatosPartida.instancia != null)
        {
            GestorDatosPartida.instancia.RegistrarOroGastado(coste);
        }
        
        ActualizarTextoUI();
    }

    // Se usa cuando se vende una torre (reembolso, no ganancia)
    public void ReembolsoVenta(int cantidad)
    {
        oroActual += cantidad;
        
        // NO registramos como oro_ganado, solo restamos del oro_gastado
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