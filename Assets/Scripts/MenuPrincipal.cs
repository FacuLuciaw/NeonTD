using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para cambiar de pantallas/niveles

public class MenuPrincipal : MonoBehaviour
{
    // Función para el botón Nueva Partida
    public void JugarNuevaPartida()
    {
        Debug.Log("Cargando el nivel 1 de Neon TD...");
        // Cuando tengas tu nivel de juego creado, le quitaremos las barras "//" a la siguiente línea:
        // SceneManager.LoadScene(1); 
    }

    // Función para el botón Continuar
    public void ContinuarPartida()
    {
        Debug.Log("Cargando la partida guardada...");
    }

    // Función para el botón Opciones
    public void AbrirOpciones()
    {
        Debug.Log("Abriendo el menú de opciones...");
    }


    // NO BORRAR
    // Función para el botón Salir (El rojo)
    public void SalirDelJuego()
    {
        Debug.Log("Cerrando Neon TD...");
        
        // Esto cierra el juego cuando ya está exportado en PC/Móvil
        Application.Quit(); 
        
        // Este bloque extra hace que el botón también funcione mientras probamos dentro de Unity
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}