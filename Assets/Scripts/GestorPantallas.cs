using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class GestorPantallas : MonoBehaviour
{
    [Header("Conecta tus pantallas aquí")]
    public GameObject pantallaInicioSesion;
    public GameObject pantallaRegistro;
    public GameObject pantallaMenu;
    public GameObject pantallaHistorial;
    public GameObject pantallaEstadisticas;

    [Header("Campos de Texto (Inputs)")]
    public TMP_InputField userLogin;
    public TMP_InputField passLogin;
    public TMP_InputField userRegister;
    public TMP_InputField passRegister;

    [Header("Configuración del Historial")]
    public GameObject prefabTarjeta;
    public Transform contenedorContent;

    [Header("Textos de Estadísticas")]
    public TMP_Text txtDanoRecibido;
    public TMP_Text txtDanoInfligido;
    public TMP_Text txtTotalTorres;

    [Header("Configuración AWS")]
    public string apiURL = "https://68l3t5abk0.execute-api.us-east-1.amazonaws.com/default/Lambda_Unuty_user";

    // --- NAVEGACIÓN BÁSICA ---

    public void IrARegistro()
    {
        DesactivarTodasLasPantallas();
        pantallaRegistro.SetActive(true);
    }

    public void IrAInicioSesion()
    {
        DesactivarTodasLasPantallas();
        pantallaInicioSesion.SetActive(true);
    }

    public void IrAMenuPrincipal()
    {
        DesactivarTodasLasPantallas();
        pantallaMenu.SetActive(true);
    }

    public void AbrirHistorial()
    {
        DesactivarTodasLasPantallas();
        pantallaHistorial.SetActive(true);
        GenerarDatosDePrueba(); // Generamos las tarjetas al entrar
    }

    public void AbrirEstadisticas()
    {
        DesactivarTodasLasPantallas();
        pantallaEstadisticas.SetActive(true);

        // Datos de prueba para estadísticas
        txtDanoRecibido.text = "15,400";
        txtDanoInfligido.text = "42,850";
        txtTotalTorres.text = "128";
    }

    // Esta es la función que te faltaba para que no de error
    private void DesactivarTodasLasPantallas()
    {
        pantallaInicioSesion.SetActive(false);
        pantallaRegistro.SetActive(false);
        pantallaMenu.SetActive(false);
        pantallaHistorial.SetActive(false);
        pantallaEstadisticas.SetActive(false);
    }

    // --- LÓGICA DE AWS ---

    public void ClickLogin()
    {
        // Borra la línea de StartCoroutine(EnviarPeticion...);
        IrAMenuPrincipal(); // Salto directo
    }

    public void ClickRegistro()
    {
        // Borra la línea de StartCoroutine(EnviarPeticion...);
        IrAInicioSesion(); // Salto directo
    }

    public void ClickJugar()
{
    // Carga la escena de tu juego. Pon el nombre EXACTO de tu escena de las torres.
    SceneManager.LoadScene("Juego"); 
}

    /*
    
    public void ClickLogin()
    {
        StartCoroutine(EnviarPeticion("login", userLogin.text, passLogin.text));
    }

    public void ClickRegistro()
    {
        StartCoroutine(EnviarPeticion("registro", userRegister.text, passRegister.text));
    }

    IEnumerator EnviarPeticion(string accion, string user, string pass)
    {
        string json = "{\"nickname\":\"" + user + "\", \"contrasena\":\"" + pass + "\", \"accion\":\"" + accion + "\"}";

        using (UnityWebRequest request = new UnityWebRequest(apiURL, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                if (accion == "registro")
                {
                    IrAInicioSesion();
                }
                else
                {
                    IrAMenuPrincipal();
                }
            }
            else
            {
                Debug.LogError("Error de AWS: " + request.error);
            }
        }
    }
    */

    // --- LÓGICA DE HISTORIAL (TARJETAS) ---

    private void GenerarDatosDePrueba()
    {
        // 1. Limpiamos la caja por si ya habíamos entrado antes
        foreach (Transform hijo in contenedorContent)
        {
            Destroy(hijo.gameObject);
        }

        // 2. Creamos 5 partidas falsas
        for (int i = 0; i < 5; i++)
        {
            // 1. Clonar (el 'false' mantiene las proporciones del UI)
            GameObject nuevaTarjeta = Instantiate(prefabTarjeta, contenedorContent, false);

            // 2. FORZAR TAMAÑO Y POSICIÓN (¡Esto pintará la tarjeta!)
            nuevaTarjeta.transform.localScale = Vector3.one; // Fuerza la escala a 1,1,1
            nuevaTarjeta.transform.localPosition = new Vector3(nuevaTarjeta.transform.localPosition.x, nuevaTarjeta.transform.localPosition.y, 0f); // Fuerza la Z a 0

            // 3. Obtenemos su cerebro
            TarjetaPartida scriptTarjeta = nuevaTarjeta.GetComponent<TarjetaPartida>();

            // 4. Inventamos datos y los mandamos
            string resultadoFalso = (Random.value > 0.5f) ? "Victoria" : "Derrota";
            string fechaFalsa = "13/04/2026";
            string duracionFalsa = Random.Range(5, 25).ToString() + " min";

            scriptTarjeta.ConfigurarTarjeta(duracionFalsa, fechaFalsa, resultadoFalso);
        }
    }

}
