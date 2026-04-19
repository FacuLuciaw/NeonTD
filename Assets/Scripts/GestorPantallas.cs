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
    public GameObject pantallaSeleccionMapa; 

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
        Debug.Log("¡El botón ha hecho clic y ha llamado a la función!"); // <--- AÑADE ESTA LÍNEA
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
        pantallaSeleccionMapa.SetActive(false);
    }

    void Start()
    {
        // El menú arranca y comprueba si hay una nota secreta de volver a jugar
        if (PlayerPrefs.GetInt("AbrirCarrusel", 0) == 1)
        {
            // 1. Borramos la nota
            PlayerPrefs.SetInt("AbrirCarrusel", 0);

            // 2. Apagamos todo y encendemos el Carrusel
            DesactivarTodasLasPantallas();
            pantallaSeleccionMapa.SetActive(true); 
        }
        else
        {
            // Si NO hay nota, abrimos la pantalla normal del menú
            DesactivarTodasLasPantallas();
            pantallaInicioSesion.SetActive(true); // O la pantalla inicial que uses
        }
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
    DesactivarTodasLasPantallas();
    pantallaSeleccionMapa.SetActive(true); // Abre el carrusel
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
        // 1. Limpiamos la caja
        for (int i = contenedorContent.childCount - 1; i >= 0; i--)
        {
            Transform hijo = contenedorContent.GetChild(i);
            hijo.SetParent(null); 
            Destroy(hijo.gameObject); 
        }

        // 2. Creamos 5 partidas falsas
        for (int i = 0; i < 5; i++)
        {
            GameObject nuevaTarjeta = Instantiate(prefabTarjeta, contenedorContent, false);
            nuevaTarjeta.transform.localScale = Vector3.one; 
            nuevaTarjeta.transform.localPosition = new Vector3(nuevaTarjeta.transform.localPosition.x, nuevaTarjeta.transform.localPosition.y, 0f); 

            TarjetaPartida scriptTarjeta = nuevaTarjeta.GetComponent<TarjetaPartida>();

            // 3. Inventamos ORO, FECHA y RESULTADO
            string resultadoFalso = (Random.value > 0.5f) ? "Victoria" : "Derrota";
            string fechaFalsa = "14/04/2026";
            
            // Generamos oro aleatorio en lugar de minutos (ej. 150 a 2000)
            string oroFalso = Random.Range(150, 2000).ToString();

            // 4. Mandamos los datos limpios a la tarjeta
            scriptTarjeta.ConfigurarTarjeta(oroFalso, fechaFalsa, resultadoFalso);
        }
    }

}
