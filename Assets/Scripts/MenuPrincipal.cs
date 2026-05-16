using UnityEngine;
using UnityEngine.SceneManagement; 

public class MenuPrincipal : MonoBehaviour
{

    // Finaliza la ejecución del juego 
    public void SalirDelJuego()
    {
        Application.Quit(); 
        
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}