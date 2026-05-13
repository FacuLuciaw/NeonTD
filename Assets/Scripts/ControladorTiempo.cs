using UnityEngine;
using TMPro;

public class ControladorTiempo : MonoBehaviour
{
    private int estadoVelocidad = 0;
    
    [Header("Configuración")]
    public TextMeshProUGUI textoBoton;

    void Start()
    {
        // Ponemos el tiempo x1 al arrancar
        Time.timeScale = 1f;
        ActualizarInterfaz();
    }

    public void AlternarVelocidad()
    {
        estadoVelocidad++;

        if (estadoVelocidad > 2)
        {
            estadoVelocidad = 0;
        }

        switch (estadoVelocidad)
        {
            case 0:
                Time.timeScale = 1f; // tiempo x1
                break;
            case 1:
                Time.timeScale = 1.5f; // tiempo x 1.5
                break;
            case 2:
                Time.timeScale = 2f; // tiempo x2
                break;
        }

        ActualizarInterfaz();
    }

    void ActualizarInterfaz()
    {
        if (textoBoton != null)
        {
            // Formato del texto del boton
            textoBoton.text = "x" + Time.timeScale.ToString("0.#", System.Globalization.CultureInfo.InvariantCulture);
        }
    }

    private void OnDestroy()
    {
        // Por si acaso se rompe la escena o salimos, dejamos el tiempo x1
        Time.timeScale = 1f;
    }
}