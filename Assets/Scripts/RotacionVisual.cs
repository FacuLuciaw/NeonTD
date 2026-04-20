using UnityEngine;

public class RotacionVisual : MonoBehaviour
{
    [Header("Configuración de Giro")]
    [Tooltip("Grados que gira por segundo. Usa valores negativos para girar al revés.")]
    public float velocidadGiro = 360f; // 360 = 1 vuelta completa por segundo

    void Update()
    {
        // En 2D siempre rotamos sobre el eje Z (el eje de profundidad)
        // transform.Rotate suma grados a la rotación actual en cada frame
        transform.Rotate(0f, 0f, velocidadGiro * Time.deltaTime);
    }
}