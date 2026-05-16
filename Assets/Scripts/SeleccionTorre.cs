using UnityEngine;
using UnityEngine.EventSystems;

public class SeleccionTorre : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject rangoVisual; 

    // Referencia compartida para que los gestores sepan qué torre está seleccionada
    public static SeleccionTorre torreSeleccionadaActual;

    [HideInInspector] public int precioPagado;
    [HideInInspector] public int indiceCatalogo;

    // Almacena el coste y el identificador del catálogo justo en el momento de la construcción para gestionar su futura venta
    public void ConfigurarDatosDeCompra(int precio, int indice)
    {
        precioPagado = precio;
        indiceCatalogo = indice;
    }

    // Selecciona la torre al hacer clic sobre ella, mostrando su rango visual y deseleccionando otra previa
    void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;

        if (torreSeleccionadaActual != null && torreSeleccionadaActual != this)
        {
            torreSeleccionadaActual.Deseleccionar();
        }

        if (rangoVisual != null) rangoVisual.SetActive(true);
        torreSeleccionadaActual = this;
    }

    // Oculta el indicador visual de rango cuando la torre es deseleccionada
    public void Deseleccionar()
    {
        if (rangoVisual != null)
        {
            rangoVisual.SetActive(false);
        }
    }

    // Detecta clics en cualquier otra parte de la pantalla que no sea esta torre o la UI para anular la selección
    void Update()
    {
        if (torreSeleccionadaActual == this && Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;

            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider == null || hit.collider.gameObject != this.gameObject)
            {
                Deseleccionar();
                torreSeleccionadaActual = null;
            }
        }
    }
}