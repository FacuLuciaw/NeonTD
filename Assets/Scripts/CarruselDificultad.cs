using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement; // <- ¡NUEVO! Necesario para cambiar de escena

// Esta clase agrupa la información de cada nivel
[System.Serializable]
public class NivelMapa
{
    public string nombreDificultad; // Ej: "Bosque - Fácil"
    public Sprite imagenDelMapa;    // La foto que se mostrará
    public string nombreEscenaUnity; // <- ¡NUEVO! El nombre exacto de la escena a cargar
    
}

public class CarruselDificultad : MonoBehaviour
{
    [Header("Conexiones de la UI")]
    public Image imgMapaFondo;
    public TMP_Text txtDificultad;

    [Header("Configuración de Mapas")]
    // Esta lista aparecerá en el Inspector para que añadas tus mapas
    public NivelMapa[] mapasDisponibles; 

    // Variable para saber en qué mapa estamos (0 es el primero)
    private int indiceActual = 0;
    private string dificultad;

void Start()
    {
        // Al arrancar, mostramos el primer mapa de la lista
        if (mapasDisponibles.Length > 0)
        {
            ActualizarPantalla();
        }
    }

    // --- FUNCIONES PARA LOS BOTONES ---

    public void SiguienteMapa()
    {
        indiceActual++; // Sumamos 1

        // Si nos pasamos del último mapa, volvemos al primero (Efecto bucle)
        if (indiceActual >= mapasDisponibles.Length)
        {
            indiceActual = 0;
        }

        ActualizarPantalla();
    }

    public void MapaAnterior()
    {
        indiceActual--; // Restamos 1

        // Si estamos en el primero y damos hacia atrás, vamos al último
        if (indiceActual < 0)
        {
            indiceActual = mapasDisponibles.Length - 1;
        }

        ActualizarPantalla();
    }

    // --- FUNCIÓN QUE CAMBIA LOS GRÁFICOS ---
    private void ActualizarPantalla()
    {
        // Cambiamos el texto y la imagen según el índice actual
        txtDificultad.text = mapasDisponibles[indiceActual].nombreDificultad;
        imgMapaFondo.sprite = mapasDisponibles[indiceActual].imagenDelMapa;
    }

    // --- ¡NUEVA FUNCIÓN PARA EL BOTÓN "INICIAR PARTIDA"! ---
    public void CargarMapaSeleccionado()
    {
        // Obtenemos el nombre de la escena que configuraste en el Inspector para este mapa
        string escenaACargar = mapasDisponibles[indiceActual].nombreEscenaUnity;

        dificultad = mapasDisponibles[indiceActual].nombreDificultad;
        
        Debug.Log("Cargando nivel: " + escenaACargar);
        SceneManager.sceneLoaded += AlCargarEscena;
        
        // Viajamos a la escena
        SceneManager.LoadScene(escenaACargar);

    }

    private void AlCargarEscena(Scene escena, LoadSceneMode modo) {

     if (GestorDatosPartida.instancia != null){
         GestorDatosPartida.instancia.EstablecerNivel(dificultad);
         Debug.Log("✓ Dificultad registrada tras carga: " + dificultad);
        } else{
         Debug.LogError("✗ El Gestor sigue sin aparecer incluso tras cargar escena");
        }

     // 3. ¡MUY IMPORTANTE!: Nos desuscribimos para que no se repita esto cada vez que cambies de mapa
     SceneManager.sceneLoaded -= AlCargarEscena;
    }
   

    // Función extra por si la necesitas en el futuro
    public int ObtenerNivelSeleccionado()
    {
        return indiceActual;
    }
}