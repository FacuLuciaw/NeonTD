using UnityEngine;
using TMPro; // Necesario para los textos

public class TarjetaPartida : MonoBehaviour
{
    [Header("Textos de la Interfaz")]
    public TMP_Text txtDuracion;
    public TMP_Text txtFecha;
    public TMP_Text txtResultado;

    // Esta función la llamaremos desde el Gestor para rellenar los datos
    public void ConfigurarTarjeta(string duracion, string fecha, string resultado)
    {
        txtDuracion.text = "Duración: " + duracion;
        txtFecha.text = "Fecha: " + fecha;
        txtResultado.text = resultado;

        // Pequeño detalle visual: Cambiar color si es victoria o derrota
        if (resultado == "Victoria")
        {
            txtResultado.color = Color.green; // O tu color neón verde
        }
        else
        {
            txtResultado.color = Color.red; // O tu color neón rojo
        }
    }
}