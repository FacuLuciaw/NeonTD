using UnityEngine;

public class RotacionVisual : MonoBehaviour
{
    [Header("Configuración de Giro")]
    [Tooltip("Grados que gira por segundo. Usa valores negativos para girar al revés.")]
    public float velocidadGiro = 360f; //1 giro completo por segundo
    // Hace girar los sprites
    void Update()
    {
        transform.Rotate(0f, 0f, velocidadGiro * Time.deltaTime);
    }
}