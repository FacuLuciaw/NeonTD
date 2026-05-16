using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;

public class GestorPantallas : MonoBehaviour
{
    

    public static bool sesionActiva = false;


    [Header("Conecta tus pantallas aquí")]
    public GameObject pantallaInicioSesion;
    public GameObject pantallaRegistro;
    public GameObject pantallaMenu;
    public GameObject pantallaHistorial;
    public GameObject pantallaEstadisticas;
    public GameObject pantallaSeleccionMapa;
    public GameObject pantallaConfiguracion;

    [Header("Campos de Texto (Inputs)")]
    public TMP_InputField userLogin;
    public TMP_InputField passLogin;
    public TMP_InputField userRegister;
    public TMP_InputField passRegister;

    [Header("Configuración del Historial")]
    public GameObject prefabTarjeta;
    public Transform contenedorContent;

    [Header("Notificación de Error")]
    public GameObject panelError;
    public TMP_Text txtMensajeError;
    public float tiempoAvisoError = 3f;
    private Coroutine rutinaErrorActiva;

    [Header("Textos de Estadísticas")]
    public TMP_Text txtDanoRecibido;
    public TMP_Text txtDanoInfligido;
    public TMP_Text txtTotalTorres;

    [Header("Configuración AWS")]
    public string apiURL = "https://pr3m2sbom5.execute-api.us-east-1.amazonaws.com/default/L_Unity_Login_towerdefens";
    public string urlHistorial = "https://688mn3wjo2.execute-api.us-east-1.amazonaws.com/default/L_Unity_Historial_towerdefender"; 

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

        
        if (GestorDatosPartida.instancia != null && GestorDatosPartida.instancia.datosPartida.id_user != 0)
        {
            int idActual = GestorDatosPartida.instancia.datosPartida.id_user;
            StartCoroutine(ObtenerHistorialActualizado(idActual));
        }
    }

    public void AbrirEstadisticas()
    {
        DesactivarTodasLasPantallas();
        pantallaEstadisticas.SetActive(true);

        txtDanoRecibido.text = "15,400";
        txtDanoInfligido.text = "42,850";
        txtTotalTorres.text = "128";
    }

    public void AbrirConfiguracion()
    {
        DesactivarTodasLasPantallas();
        pantallaConfiguracion.SetActive(true);
    }


    public void CerrarSesion()
    {
        Debug.Log("Cerrando sesión y borrando memoria temporal...");
        sesionActiva = false;
        IrAInicioSesion();
    }

    private void DesactivarTodasLasPantallas()
    {
        pantallaInicioSesion.SetActive(false);
        pantallaRegistro.SetActive(false);
        pantallaMenu.SetActive(false);
        pantallaHistorial.SetActive(false);
        pantallaEstadisticas.SetActive(false);
        pantallaSeleccionMapa.SetActive(false);

        if (pantallaConfiguracion != null) pantallaConfiguracion.SetActive(false);
    }

    void Start()
    {

        if (panelError != null) panelError.SetActive(false);

        
        if (PlayerPrefs.GetInt("AbrirCarrusel", 0) == 1)
        {
            PlayerPrefs.SetInt("AbrirCarrusel", 0);
            DesactivarTodasLasPantallas();
            pantallaSeleccionMapa.SetActive(true);
        }
        
        else if (sesionActiva == true)
        {
            Debug.Log("✓ Sesión activa en memoria RAM. Saltando al Menú Principal.");
            DesactivarTodasLasPantallas();
            pantallaMenu.SetActive(true);
        }
        
        else
        {
            DesactivarTodasLasPantallas();
            pantallaInicioSesion.SetActive(true);
        }
    }

    public void ClickLoginrapido()
    {
        IrAMenuPrincipal();
    }

    public void ClickJugar()
    {
        DesactivarTodasLasPantallas();
        pantallaSeleccionMapa.SetActive(true);
    }

    public void ClickLogin()
    {
        if (string.IsNullOrEmpty(userLogin.text) || string.IsNullOrEmpty(passLogin.text))
        {
            Debug.LogWarning("Por favor, rellena todos los campos.");
            MostrarError(GestorIdiomas.ObtenerErrorCamposVacios()); 
            return;
        }

        StartCoroutine(EnviarPeticion("login", userLogin.text, passLogin.text));
    }

    public void ClickRegistro()
    {
        
        if (string.IsNullOrEmpty(userRegister.text) || string.IsNullOrEmpty(passRegister.text))
        {
            Debug.LogWarning("Por favor, rellena todos los campos.");
            MostrarError(GestorIdiomas.ObtenerErrorCamposVacios());
            return;
        }

        StartCoroutine(EnviarPeticion("registro", userRegister.text, passRegister.text));
    }
    
    public void MostrarError(string mensajeTraducido)
    {
        if (panelError != null) panelError.SetActive(true);
        
        if (txtMensajeError != null)
        {
            txtMensajeError.text = mensajeTraducido;
        }

        
        if (rutinaErrorActiva != null)
        {
            StopCoroutine(rutinaErrorActiva);
        }
        
        
        rutinaErrorActiva = StartCoroutine(OcultarErrorAutomatico());
    }

    private System.Collections.IEnumerator OcultarErrorAutomatico()
    {
        
        yield return new WaitForSeconds(tiempoAvisoError);

        
        if (panelError != null)
        {
            panelError.SetActive(false);
        }
    }
    
    IEnumerator EnviarPeticion(string accion, string user, string pass)
    {
        AuthRequest datos = new AuthRequest
        {
            nickname = user,
            contrasena = pass,
            accion = accion
        };

        string json = JsonUtility.ToJson(datos);

        using (UnityWebRequest request = new UnityWebRequest(apiURL, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error de Red/DNS: " + request.error);
                
                if (accion == "registro") MostrarError(GestorIdiomas.ObtenerErrorRegistro());
                else MostrarError(GestorIdiomas.ObtenerErrorLogin());
            }
            else
            {
                if (request.responseCode == 200 || request.responseCode == 201)
                {
                    if (accion == "registro") IrAInicioSesion();
                    else
                    {
                        
                        sesionActiva = true;

                        if (GestorDatosPartida.instancia != null)
                        {
                            RespuestaAWS awsDatos = JsonUtility.FromJson<RespuestaAWS>(request.downloadHandler.text);
                            if (awsDatos != null)
                            {
                                GestorDatosPartida.instancia.datosPartida.id_user = awsDatos.id_user;
                                Debug.Log("✓ id_user guardado en GestorDatosPartida: " + awsDatos.id_user);
                            }
                        }

                        IrAMenuPrincipal();
                    }
                }
                else
                {
                    
                    Debug.LogWarning("Respuesta del servidor no válida (Ej. Contraseña incorrecta)");
                    
                    if (accion == "registro") MostrarError(GestorIdiomas.ObtenerErrorRegistro());
                    else MostrarError(GestorIdiomas.ObtenerErrorLogin());
                }
            }
        }
    }

    IEnumerator ObtenerHistorialActualizado(int idUsuario)
    {
        
        string json = "{\"id_user\":" + idUsuario + "}";

        using (UnityWebRequest request = new UnityWebRequest(urlHistorial, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                ActualizarHistorialReal(request.downloadHandler.text);
            }
            else
            {
                Debug.LogError("Error actualizando historial: " + request.error);
            }
        }
    }

    [System.Serializable]
    public class AuthRequest
    {
        public string nickname;
        public string contrasena;
        public string accion;
    }

    [System.Serializable]
    public class PartidaData
    {
        public int id_partida;
        public string fecha;
        public string estado;
        public string nivel;
        public int oro_ganado;
        public int total_enemigos;
        public int total_torres;
    }

    [System.Serializable]
    public class RespuestaAWS
    {
        public string status;
        public int id_user;
        public List<PartidaData> partidas;
    }

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