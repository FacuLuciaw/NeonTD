using UnityEngine;

public class GestorHistorial : MonoBehaviour
{
    [Header("Navegación de Pantallas")]
    public GameObject pantallaMenuPrincipal;
    public GameObject pantallaHistorial;

    [Header("Configuración de Tarjetas")]
    public GameObject prefabTarjeta; // El molde
    public Transform contenedorContent; // La caja donde se apilan

    // 1. FUNCIÓN PARA ABRIR LA PANTALLA Y CARGAR DATOS
    public void AbrirHistorial()
    {
        // Navegación
        pantallaMenuPrincipal.SetActive(false);
        pantallaHistorial.SetActive(true);

        // Generar datos
       // GenerarDatosDePrueba();
    }

    // 2. FUNCIÓN PARA VOLVER AL MENÚ
    public void CerrarHistorial()
    {
        pantallaHistorial.SetActive(false);
        pantallaMenuPrincipal.SetActive(true);
    }

    // 3. LA MAGIA: SIMULAR LA BASE DE DATOS
   /* private void GenerarDatosDePrueba()
    {
        // Primero, borramos las tarjetas viejas (por si entramos y salimos varias veces)
        foreach (Transform hijo in contenedorContent)
        {
            Destroy(hijo.gameObject);
        }

        // Creamos 5 partidas de prueba usando un bucle "for"
        for (int i = 0; i < 5; i++)
        {
            // Instanciar (Clonar) el Prefab dentro del contenedor
            GameObject nuevaTarjeta = Instantiate(prefabTarjeta, contenedorContent);

            // Obtener el script de la tarjeta que acabamos de crear
            TarjetaPartida scriptTarjeta = nuevaTarjeta.GetComponent<TarjetaPartida>();

            // Inventar datos aleatorios para la prueba
            string resultadoFalso = (Random.value > 0.5f) ? "Victoria" : "Derrota";
            string fechaFalsa = "13/04/2026"; // Fecha actual de simulación
            string duracionFalsa = Random.Range(5, 25).ToString() + " min";

            // ¡Mandamos los datos a la tarjeta!
            scriptTarjeta.ConfigurarTarjeta(duracionFalsa, fechaFalsa, resultadoFalso,"dificil", "10","4");
        }
    }*/
}