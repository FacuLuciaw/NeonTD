using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class GestorDatosPartida : MonoBehaviour
{
    public static GestorDatosPartida instancia;

    [Header("Datos de la Partida")]
    public PartidaJSON datosPartida = new PartidaJSON();

    [Header("AWS Guardado de Partida")]
    public string savePartidaAPI = "https://q2v7qbfux6.execute-api.us-east-1.amazonaws.com/default/L_Unity_Save_TD";

    // Configura el Singleton asegurando que este gestor sobreviva a las cargas de nuevas escenas
    void Awake()
    {
        if (instancia == null)
        {
            instancia = this;
            DontDestroyOnLoad(gameObject);
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

    // --- MÉTODOS DE ACCESO RÁPIDO A LA ESTRUCTURA JSON ---
    public void RegistrarTorre(string nombreTorre) => datosPartida.RegistrarTorre(nombreTorre);
    public void DesvincularTorre(string nombreTorre) => datosPartida.DesvincularTorre(nombreTorre);
    public void RegistrarEnemigo(string nombreEnemigo) => datosPartida.RegistrarEnemigo(nombreEnemigo);
    public void AgregarEnemigoActivo(string nombreEnemigo) => datosPartida.AgregarEnemigoActivo(nombreEnemigo);
    public void RemoverEnemigoActivo(string nombreEnemigo) => datosPartida.RemoverEnemigoActivo(nombreEnemigo);
    public void RegistrarOroGanado(int cantidad) => datosPartida.RegistrarOroGanado(cantidad);
    public void RegistrarOroGastado(int cantidad) => datosPartida.RegistrarOroGastado(cantidad);
    public void RestarOroGastado(int cantidad) => datosPartida.RestarOroGastado(cantidad);
    public void RegistrarDanoRecibido(float cantidad) => datosPartida.RegistrarDanoRecibido(cantidad);
    public void RegistrarDanoInfligido(float cantidad) => datosPartida.RegistrarDanoInfligido(cantidad);
    public void RegistrarRondaCompletada() => datosPartida.RegistrarRondaCompletada();
    public void IniciarNuevaRonda() => datosPartida.IniciarNuevaRonda();
    public void EstablecerEstado(string estado) => datosPartida.EstablecerEstado(estado);
    public void EstablecerNivel(string nombreNivel) => datosPartida.EstablecerNivel(nombreNivel);

    // Prepara una nueva sesión de datos preservando las torres construidas, el nivel y del jugador
    public void ResetearParaModoInfinito()
    {
        int userId = datosPartida.id_user;
        string nivelActual = datosPartida.nivel;
        List<DetalleTorre> torresGuardadas = new List<DetalleTorre>();

        foreach (DetalleTorre torre in datosPartida.torres)
        {
            torresGuardadas.Add(new DetalleTorre { nombre = torre.nombre, cantidad = torre.cantidad });
        }

        datosPartida = new PartidaJSON();

        datosPartida.id_user = userId;
        datosPartida.nivel = nivelActual;
        datosPartida.estado = "infinito";
        datosPartida.torres = torresGuardadas;
    }

    // Inicia la corrutina para enviar los datos a la base de datos
    public void GuardarPartidaAWS()
    {
        StartCoroutine(EnviarPartidaAWS());
    }

    // Transforma los datos a formato JSON y realiza la petición POST a la API de AWS
    IEnumerator EnviarPartidaAWS()
    {
        string json = JsonUtility.ToJson(datosPartida);

        using (UnityWebRequest request = new UnityWebRequest(savePartidaAPI, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();
        }
    }

    // Limpia todo el progreso actual para empezar un mapa nuevo, manteniendo únicamente el ID del jugador
    public void ResetDatosPartida()
    {
        int userId = datosPartida.id_user; 
        
        datosPartida = new PartidaJSON();
        
        datosPartida.id_user = userId;
        datosPartida.estado = string.Empty;
    }
}