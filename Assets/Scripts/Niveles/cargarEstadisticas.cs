using UnityEngine;
using TMPro;

public class CargarEstadisticas : MonoBehaviour
{
    [Header("Textos de la Interfaz")]
    public TextMeshProUGUI txtOroGanado;
    public TextMeshProUGUI txtOroGastado;
    public TextMeshProUGUI txtDanoRecibido;
    public TextMeshProUGUI txtDanoInfligido;
    
    public TextMeshProUGUI txtTorretas;
    public TextMeshProUGUI txtEnemigos;

    void Start()
    {
        MostrarDatos();
    }

    public void MostrarDatos()
    {
        if (GestorDatosPartida.instancia != null)
        {
            txtOroGanado.text = GestorDatosPartida.instancia.datosPartida.oro_ganado.ToString();
            txtOroGastado.text = GestorDatosPartida.instancia.datosPartida.oro_gastado.ToString();
            txtDanoRecibido.text = GestorDatosPartida.instancia.datosPartida.dano_total_recibido.ToString();
            txtDanoInfligido.text = GestorDatosPartida.instancia.datosPartida.dano_total_infligido.ToString();
            
            int totalTorretas = 0;
            foreach (DetalleTorre torre in GestorDatosPartida.instancia.datosPartida.torres)
            {
                totalTorretas += torre.cantidad;
            }

            if (txtTorretas != null) txtTorretas.text = totalTorretas.ToString();

            int totalEnemigos = 0;
            foreach (DetalleEnemigo enemigo in GestorDatosPartida.instancia.datosPartida.enemigos)
            {
                totalEnemigos += enemigo.cantidad;
            }

            if (txtEnemigos != null) txtEnemigos.text = totalEnemigos.ToString();
            
            Debug.Log("✓ Todas las estadísticas (incluyendo torretas y enemigos) se cargaron correctamente.");
        }
        else
        {
            Debug.LogWarning("✗ No se encontró GestorDatosPartida. Recuerda iniciar el juego desde la escena del Menú para que se genere.");
        }
    }
}