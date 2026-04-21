using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
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
    public string apiURL = "https://pr3m2sbom5.execute-api.us-east-1.amazonaws.com/default/L_Unity_Login_towerdefens";

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
        //GenerarDatosDePrueba(); // Generamos las tarjetas al entrar
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

   /* public void ClickLogin()
    {
        // Borra la línea de StartCoroutine(EnviarPeticion...);
        IrAMenuPrincipal(); // Salto directo
    }

    public void ClickRegistro()
    {
        // Borra la línea de StartCoroutine(EnviarPeticion...);
        IrAInicioSesion(); // Salto directo
    }*/

public void ClickJugar()
{
    DesactivarTodasLasPantallas();
    pantallaSeleccionMapa.SetActive(true); // Abre el carrusel
}

    
public void ClickLogin()
    {
        if (string.IsNullOrEmpty(userLogin.text) || string.IsNullOrEmpty(passLogin.text)) {
            Debug.LogWarning("Por favor, rellena todos los campos.");
            return;
        }
        StartCoroutine(EnviarPeticion("login", userLogin.text, passLogin.text));
    }

    public void ClickRegistro()
    {
        StartCoroutine(EnviarPeticion("registro", userRegister.text, passRegister.text));
    }

    // --- CORRUTINA DE RED ---

    IEnumerator EnviarPeticion(string accion, string user, string pass)
    {
        // 1. Crear el objeto de datos
        AuthRequest datos = new AuthRequest {
            nickname = user,
            contrasena = pass,
            accion = accion
        };

        string json = JsonUtility.ToJson(datos);

        // 2. Configurar la petición
        Debug.Log("JSON enviado: " + json);
        using (UnityWebRequest request = new UnityWebRequest(apiURL, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            Debug.Log($"Conectando a AWS para {accion}...");

            yield return request.SendWebRequest();

            // 3. Manejo de Errores de Red (Aquí es donde sale el "Cannot resolve host")
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error de Red/DNS: " + request.error);
                Debug.LogError("Causa probable: URL mal escrita, falta de internet o firewall.");
            }
            else
            {
                // 4. Procesar Respuesta Exitosa
                Debug.Log("Respuesta recibida: " + request.downloadHandler.text);
                
                // Si el login es correcto, pasamos al menú
                if (request.responseCode == 200 || request.responseCode == 201)
                {
                    if (accion == "registro") IrAInicioSesion();
                    else
                    {
                        IrAMenuPrincipal();
                        ActualizarHistorialReal(request.downloadHandler.text);
                    } 
                }
                else {
                    Debug.LogWarning("AWS rechazó la petición: " + request.downloadHandler.text);
                }
            }
        }
    }

    // Clase auxiliar para convertir a JSON 
    [System.Serializable]
    public class AuthRequest {
        public string nickname;
        public string contrasena;
        public string accion;
    }

    [System.Serializable]
    public class PartidaData{
    public int id_partida;
    public string fecha;
    public string estado;
    public string nivel;
    public int oro_ganado;
    public int total_enemigos;
    public int total_torres;
    }

    [System.Serializable]
    public class RespuestaAWS {
        public string status;
        public int id_user;
        public List<PartidaData> partidas;
    }


    // --- LÓGICA DE HISTORIAL (TARJETAS) ---

/*private void GenerarDatosDePrueba()
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
            scriptTarjeta.ConfigurarTarjeta(oroFalso, fechaFalsa, resultadoFalso,"Dificil","17","4");
        }
    }*/

    private void ActualizarHistorialReal(string json)
    {
        RespuestaAWS datos = JsonUtility.FromJson<RespuestaAWS>(json);
        if (datos == null || datos.partidas == null || contenedorContent == null) return;

        foreach (Transform hijo in contenedorContent) { Destroy(hijo.gameObject); }

        foreach (PartidaData p in datos.partidas)
        {
            GameObject nueva = Instantiate(prefabTarjeta, contenedorContent, false);
            nueva.transform.localScale = Vector3.one;
            TarjetaPartida script = nueva.GetComponent<TarjetaPartida>();
            if (script != null)
            {
                script.ConfigurarTarjeta(
                    p.oro_ganado.ToString(),
                    p.fecha,
                    p.estado,
                    p.nivel,
                    p.total_enemigos.ToString(),
                    p.total_torres.ToString()
                );
            }
        }
    }

}
