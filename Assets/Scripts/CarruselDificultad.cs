using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement; 

[System.Serializable]
public class NivelMapa
{
    public string nombreDificultad; 
    public Sprite imagenDelMapa;    
    public string nombreEscenaUnity; 
}

public class CarruselDificultad : MonoBehaviour
{
    [Header("Conexiones de la UI")]
    public Image imgMapaFondo;
    public TMP_Text txtDificultad;

    [Header("Configuración de Mapas")]
    public NivelMapa[] mapasDisponibles; 

    private int indiceActual = 0;
    private string dificultad;

    void Start()
    {
        if (mapasDisponibles.Length > 0)
        {
            ActualizarPantalla();
        }
    }

    public void SiguienteMapa()
    {
        indiceActual++; 

        if (indiceActual >= mapasDisponibles.Length)
        {
            indiceActual = 0;
        }

        ActualizarPantalla();
    }

    public void MapaAnterior()
    {
        indiceActual--; 

        if (indiceActual < 0)
        {
            indiceActual = mapasDisponibles.Length - 1;
        }

        ActualizarPantalla();
    }

    // --- FUNCIÓN QUE CAMBIA LOS GRÁFICOS ---
    private void ActualizarPantalla()
    {
        // --- ¡MAGIA DEL IDIOMA APLICADA AQUÍ! ---
        // Traducimos visualmente la palabra ANTES de ponerla en la pantalla
        txtDificultad.text = GestorIdiomas.ObtenerDificultadTraducida(mapasDisponibles[indiceActual].nombreDificultad);
        // ----------------------------------------
        
        imgMapaFondo.sprite = mapasDisponibles[indiceActual].imagenDelMapa;
    }

    public void CargarMapaSeleccionado()
    {
        string escenaACargar = mapasDisponibles[indiceActual].nombreEscenaUnity;

        // OJO: Guardamos el nombre ORIGINAL (sin traducir) para mandarlo limpio a AWS
        dificultad = mapasDisponibles[indiceActual].nombreDificultad;

        if (GestorDatosPartida.instancia != null)
        {
            GestorDatosPartida.instancia.ResetDatosPartida();
            Debug.Log("✓ Datos de partida reiniciados antes de cargar nueva partida");
        }

        SceneManager.sceneLoaded += AlCargarEscena;
        
        Debug.Log("Cargando nivel: " + escenaACargar);
        
        Time.timeScale = 1f; 
        SceneManager.LoadScene(escenaACargar);

    }

    private void AlCargarEscena(Scene escena, LoadSceneMode modo) 
    {
        if (GestorDatosPartida.instancia != null){
            GestorDatosPartida.instancia.EstablecerNivel(dificultad);
            Debug.Log("✓ Dificultad registrada en base de datos tras carga: " + dificultad);
        } else{
            Debug.LogError("✗ El Gestor sigue sin aparecer incluso tras cargar escena");
        }

        SceneManager.sceneLoaded -= AlCargarEscena;
    }
   
    public int ObtenerNivelSeleccionado()
    {
        return indiceActual;
    }
}