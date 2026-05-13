using UnityEngine;

public class GestorHistorial : MonoBehaviour
{
    [Header("Navegación de Pantallas")]
    public GameObject pantallaMenuPrincipal;
    public GameObject pantallaHistorial;

    [Header("Configuración de Tarjetas")]
    public GameObject prefabTarjeta; // El molde visual para mostrar los datos de cada partida
    public Transform contenedorContent; // La caja de la interfaz donde se apilarán todas las tarjetas

    // Desactiva el menú principal y activa el panel del historial para que el jugador lo vea
    public void AbrirHistorial()
    {
        pantallaMenuPrincipal.SetActive(false);
        pantallaHistorial.SetActive(true);
    }

    // Oculta el panel del historial y devuelve al jugador al menú principal
    public void CerrarHistorial()
    {
        pantallaHistorial.SetActive(false);
        pantallaMenuPrincipal.SetActive(true);
    }
}