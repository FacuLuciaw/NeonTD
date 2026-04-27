using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio; // Necesario para el AudioMixer

public class GestorConfiguracion : MonoBehaviour
{
    [Header("Conexiones Audio")]
    public AudioMixer mezcladorAudio;

    // Estas funciones las conectaremos a tus Sliders
    public void CambiarVolumenMusica(float valorSlider)
    {
        // Usamos Log10 porque el oído humano y Unity escuchan el volumen en curvas (decibelios), no en líneas rectas.
        mezcladorAudio.SetFloat("VolumenMusica", Mathf.Log10(valorSlider) * 20);
    }

    public void CambiarVolumenFX(float valorSlider)
    {
        mezcladorAudio.SetFloat("VolumenFX", Mathf.Log10(valorSlider) * 20);
    }

    // Esta función la conectaremos a tu Dropdown
    public void CambiarModoPantalla(int indiceDropdown)
    {
        // El índice 0 es la primera opción, el 1 la segunda, etc.
        switch (indiceDropdown)
        {
            case 0: // Pantalla Completa
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                break;
            case 1: // Ventana sin bordes
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;
            case 2: // Ventana con bordes
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;
        }
    }
}