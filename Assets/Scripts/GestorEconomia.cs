using UnityEngine;
using TMPro; // Necesario para modificar los textos de la UI modernas

public class GestorEconomia : MonoBehaviour
{
    // Esto crea un "Singleton", una forma fácil de acceder a este script desde cualquier lado
    public static GestorEconomia instancia;

    [Header("Dinero")]
    public int oroActual = 15; // Empezamos con 15 para que puedas poner un par de torres al inicio
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
        oroActual += cantidad;
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
        ActualizarTextoUI();
    }

    void ActualizarTextoUI()
    {
        if (textoOro != null)
        {
            textoOro.text = "Oro: " + oroActual.ToString();
        }
    }
}