using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class DatosNivel
{
    public string nombre; // "Mapa 1", "Mapa 2", etc.
    public string dificultad; // "facil", "normal", "dificil"
    public string nombreEscena; // Nombre de la escena de Unity
}

public class GestorMapas : MonoBehaviour
{
    [Header("Configuración de Niveles")]
    [SerializeField] private DatosNivel[] niveles = new DatosNivel[3];

    void Start()
    {
        // Inicializamos los niveles si no están configurados
        if (niveles[0] == null || string.IsNullOrEmpty(niveles[0].nombre))
        {
            niveles[0] = new DatosNivel { nombre = "Mapa 1", dificultad = "facil", nombreEscena = "Nivel1" };
            niveles[1] = new DatosNivel { nombre = "Mapa 2", dificultad = "normal", nombreEscena = "Nivel2" };
            niveles[2] = new DatosNivel { nombre = "Mapa 3", dificultad = "dificil", nombreEscena = "Nivel3" };
        }
    }

    // Se llama cuando el jugador selecciona un mapa (botón del menú)
    public void SeleccionarNivel(int indiceNivel)
    {
        if (indiceNivel < 0 || indiceNivel >= niveles.Length)
        {
            Debug.LogError("Índice de nivel inválido: " + indiceNivel);
            return;
        }

        DatosNivel nivelSeleccionado = niveles[indiceNivel];

        // Registramos el nivel en los datos de la partida
        if (GestorDatosPartida.instancia != null)
        {
            GestorDatosPartida.instancia.EstablecerNivel(nivelSeleccionado.dificultad);
            Debug.Log("✓ Nivel seleccionado: " + nivelSeleccionado.nombre + " (" + nivelSeleccionado.dificultad + ")");
        }
        else
        {
            Debug.LogWarning("⚠ GestorDatosPartida no está inicializado");
        }

        // Cargamos la escena del nivel
        if (!string.IsNullOrEmpty(nivelSeleccionado.nombreEscena))
        {
            SceneManager.LoadScene(nivelSeleccionado.nombreEscena);
        }
        else
        {
            Debug.LogError("⚠ nombreEscena no está configurado para " + nivelSeleccionado.nombre);
        }
    }

    // Método para obtener los datos de un nivel específico
    public DatosNivel ObtenerNivel(int indiceNivel)
    {
        if (indiceNivel >= 0 && indiceNivel < niveles.Length)
        {
            return niveles[indiceNivel];
        }
        return null;
    }

    // Obtener todos los niveles
    public DatosNivel[] ObtenerTodosLosNiveles()
    {
        return niveles;
    }
}
