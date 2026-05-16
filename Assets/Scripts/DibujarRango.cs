using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class DibujarRango : MonoBehaviour
{
    [Header("Forma del Rango")]
    public bool esCuadrado = false; 

    [Header("Ajustes Visuales")]
    public float radio = 12f;
    public float grosorLinea = 0.2f;
    public int suavidad = 60;
    public Color colorNeon = Color.cyan;

    private LineRenderer lineRenderer;

    // Obtenemos la linea del linerenderer, para dibujar despues el rango
    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        Dibujar();
    }

    // Se muestra el cambio sin necesidad de arrancar el juego
    void OnValidate()
    {
        if (lineRenderer == null) lineRenderer = GetComponent<LineRenderer>();
        Dibujar();
    }

    // Metemos los colores y grosores a la linea y decidimos si toca hacer la forma circular o cuadrada
    public void Dibujar()
    {
        if (lineRenderer == null) return;

        lineRenderer.startWidth = grosorLinea;
        lineRenderer.endWidth = grosorLinea;
        lineRenderer.startColor = colorNeon;
        lineRenderer.endColor = colorNeon;
        
        lineRenderer.loop = true; 
        
        lineRenderer.useWorldSpace = false; 

        if (esCuadrado)
        {
            GenerarPuntosCuadrado();
        }
        else
        {
            GenerarPuntosCirculo();
        }
    }

    // Para hacer un circulo redondo suave
    void GenerarPuntosCirculo()
    {
        lineRenderer.positionCount = suavidad;
        float angulo = 0f;

        for (int i = 0; i < suavidad; i++)
        {
            float x = Mathf.Sin(Mathf.Deg2Rad * angulo) * radio;
            float y = Mathf.Cos(Mathf.Deg2Rad * angulo) * radio;

            lineRenderer.SetPosition(i, new Vector3(x, y, 0f));
            angulo += (360f / suavidad);
        }
    }

    // Pone 4 puntos en las 4 esquinas para crear la caja
    void GenerarPuntosCuadrado()
    {
        lineRenderer.positionCount = 4;

        lineRenderer.SetPosition(0, new Vector3(-radio, -radio, 0f)); 
        lineRenderer.SetPosition(1, new Vector3(-radio, radio, 0f));  
        lineRenderer.SetPosition(2, new Vector3(radio, radio, 0f));   
        lineRenderer.SetPosition(3, new Vector3(radio, -radio, 0f));  
    }
}