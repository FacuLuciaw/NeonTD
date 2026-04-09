using UnityEngine;

public class GestorPantallas : MonoBehaviour
{
    [Header("Conecta tus pantallas aquí")]
    // Estas variables guardarán las pantallas enteras
    public GameObject pantallaInicioSesion;
    public GameObject pantallaRegistro;

    // Esta función se llamará cuando aprietes el botón "Registrarse"
    public void IrARegistro()
    {
        pantallaInicioSesion.SetActive(false); // Apaga el Inicio de Sesión
        pantallaRegistro.SetActive(true);      // Enciende el Registro
        Debug.Log("Cambiando a pantalla de Registro");
    }

    // Esta función se llamará cuando aprietes el botón "Volver" o "Ya tengo cuenta"
    public void IrAInicioSesion()
    {
        pantallaRegistro.SetActive(false);     // Apaga el Registro
        pantallaInicioSesion.SetActive(true);  // Enciende el Inicio de Sesión
        Debug.Log("Cambiando a pantalla de Inicio de Sesión");
    }
}