using UnityEngine;

public class GestorDatosPartida : MonoBehaviour
{
    public static GestorDatosPartida instancia;

    [Header("Datos de la Partida")]
    public PartidaJSON datosPartida = new PartidaJSON();

    void Awake()
    {
        // Configuramos el Singleton para acceder desde cualquier lado
        if (instancia == null)
        {
            instancia = this;
            // Hacemos que persista entre cambios de escena
            DontDestroyOnLoad(gameObject);
            Debug.Log("✓ GestorDatosPartida inicializado");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Verificamos que esté inicializado
        if (instancia == null)
        {
            Debug.LogError("✗ ERROR: GestorDatosPartida no está inicializado");
        }
        else
        {
            Debug.Log("✓ GestorDatosPartida disponible en escena: " + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }
    }

    // Métodos de acceso rápido
    public void RegistrarTorre(string nombreTorre) => datosPartida.RegistrarTorre(nombreTorre);
    public void DesvincularTorre(string nombreTorre) => datosPartida.DesvincularTorre(nombreTorre);
    public void RegistrarEnemigo(string nombreEnemigo) => datosPartida.RegistrarEnemigo(nombreEnemigo);
    public void RegistrarOroGanado(int cantidad) => datosPartida.RegistrarOroGanado(cantidad);
    public void RegistrarOroGastado(int cantidad) => datosPartida.RegistrarOroGastado(cantidad);
    public void RestarOroGastado(int cantidad) => datosPartida.RestarOroGastado(cantidad);
    public void RegistrarDanoRecibido(float cantidad) => datosPartida.RegistrarDanoRecibido(cantidad);
    public void EstablecerNivel(string nombreNivel) => datosPartida.EstablecerNivel(nombreNivel);
}
