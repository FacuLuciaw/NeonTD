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

    // Inicializa el carrusel mostrando el primer mapa 
    void Start()
    {
        if (mapasDisponibles.Length > 0)
        {
            ActualizarPantalla();
        }
    }

    // Avanza al siguiente mapa de la lista y vuelve al principio si sobrepasa el límite
    public void SiguienteMapa()
    {
        indiceActual++; 

        if (indiceActual >= mapasDisponibles.Length)
        {
            indiceActual = 0;
        }

        ActualizarPantalla();
    }

    // Retrocede al mapa anterior y pasa al último de la lista si se encuentra en el primero
    public void MapaAnterior()
    {
        indiceActual--; 

        if (indiceActual < 0)
        {
            indiceActual = mapasDisponibles.Length - 1;
        }

        ActualizarPantalla();
    }

    // Actualiza los elementos visuales del menú principal, solicitando la traducción del texto
    public void ActualizarPantalla()
    {
        txtDificultad.text = GestorIdiomas.ObtenerDificultadTraducida(mapasDisponibles[indiceActual].nombreDificultad);
        imgMapaFondo.sprite = mapasDisponibles[indiceActual].imagenDelMapa;
    }

    // Carga el nivel seleccionado
    public void CargarMapaSeleccionado()
    {
        string escenaACargar = mapasDisponibles[indiceActual].nombreEscenaUnity;
        dificultad = mapasDisponibles[indiceActual].nombreDificultad;

        if (GestorDatosPartida.instancia != null)
        {
            GestorDatosPartida.instancia.ResetDatosPartida();
        }

        SceneManager.sceneLoaded += AlCargarEscena;
        
        Time.timeScale = 1f; 
        SceneManager.LoadScene(escenaACargar);
    }

    // Registra la dificultad en la base de datos
    private void AlCargarEscena(Scene escena, LoadSceneMode modo) 
    {
        if (GestorDatosPartida.instancia != null)
        {
            GestorDatosPartida.instancia.EstablecerNivel(dificultad);
        }

        SceneManager.sceneLoaded -= AlCargarEscena;
    }
   
    public int ObtenerNivelSeleccionado()
    {
        return indiceActual;
    }
}