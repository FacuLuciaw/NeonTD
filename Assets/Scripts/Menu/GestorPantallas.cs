using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;

public class GestorPantallas : MonoBehaviour
{
    // --- NUEVA VARIABLE DE MEMORIA TEMPORAL ---

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
    public string urlHistorial = "https://688mn3wjo2.execute-api.us-east-1.amazonaws.com/default/L_Unity_Historial_towerdefender"; // URL de la nueva Lambda de historial

    // ==========================================
    // NAVEGACIÓN BÁSICA
    // ==========================================

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

    // cada vez que abre el historial, se refresca con los datos más recientes de AWS
    public void AbrirHistorial()
    {
        DesactivarTodasLasPantallas();
        pantallaHistorial.SetActive(true);

        // Si tenemos un ID de usuario, pedimos el historial actualizado
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

    // ==========================================
    // INICIO DEL JUEGO
    // ==========================================

    void Start()
    {

        if (panelError != null) panelError.SetActive(false);

        // 1. Comprueba si venimos de jugar un nivel                                            
        if (PlayerPrefs.GetInt("AbrirCarrusel", 0) == 1)
        {
            PlayerPrefs.SetInt("AbrirCarrusel", 0);
            DesactivarTodasLasPantallas();
            pantallaSeleccionMapa.SetActive(true);
        }
        // 2. Comprueba la memoria RAM (Variable estática)                                                 
        else if (sesionActiva == true)
        {
            Debug.Log("✓ Sesión activa en memoria RAM. Saltando al Menú Principal.");
            DesactivarTodasLasPantallas();
            pantallaMenu.SetActive(true);
        }
        // 3. Si abrimos el juego por primera vez, la variable será false                                                                 
        else
        {
            DesactivarTodasLasPantallas();
            pantallaInicioSesion.SetActive(true);
        }
    }

    // ==========================================
    // LÓGICA DE BOTONES Y CONEXIÓN
    // ==========================================

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
            MostrarError(GestorIdiomas.ObtenerErrorCamposVacios()); // MODIFICADO
            return;
        }

        StartCoroutine(EnviarPeticion("login", userLogin.text, passLogin.text));
    }

    public void ClickRegistro()
    {
        // MODIFICADO: Añadida la comprobación de campos vacíos al registrar
        if (string.IsNullOrEmpty(userRegister.text) || string.IsNullOrEmpty(passRegister.text))
        {
            Debug.LogWarning("Por favor, rellena todos los campos.");
            MostrarError(GestorIdiomas.ObtenerErrorCamposVacios());
            return;
        }

        StartCoroutine(EnviarPeticion("registro", userRegister.text, passRegister.text));
    }

    // ==========================================
    // FUNCIONES DEL AVISO DE ERROR (MULTIUSOS)
    // ==========================================
    
    // MODIFICADO: Ahora recibe el texto exacto que debe mostrar
    public void MostrarError(string mensajeTraducido)
    {
        if (panelError != null) panelError.SetActive(true);
        
        if (txtMensajeError != null)
        {
            txtMensajeError.text = mensajeTraducido;
        }

        // Si ya hay una cuenta atrás en marcha, la detenemos para empezar de cero
        if (rutinaErrorActiva != null)
        {
            StopCoroutine(rutinaErrorActiva);
        }
        
        // Iniciamos el temporizador
        rutinaErrorActiva = StartCoroutine(OcultarErrorAutomatico());
    }

    private System.Collections.IEnumerator OcultarErrorAutomatico()
    {
        // Esperamos los segundos configurados
        yield return new WaitForSeconds(tiempoAvisoError);

        // Apagamos el panel
        if (panelError != null)
        {
            panelError.SetActive(false);
        }
    }

    // ==========================================
    // CORRUTINAS DE RED (AWS)
    // ==========================================

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
                // MODIFICADO: Decide qué error mostrar según la acción
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
                        // Encendemos la variable en la RAM tras loguearnos con éxito                                                             
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
                    // Si AWS nos devuelve un 401, 404, etc (contraseña mal, usuario no existe)
                    Debug.LogWarning("Respuesta del servidor no válida (Ej. Contraseña incorrecta)");
                    // MODIFICADO: Decide qué error mostrar según la acción
                    if (accion == "registro") MostrarError(GestorIdiomas.ObtenerErrorRegistro());
                    else MostrarError(GestorIdiomas.ObtenerErrorLogin());
                }
            }
        }
    }

    // CORRUTINA: Para refrescar el historial en tiempo real sin re-loguear
    IEnumerator ObtenerHistorialActualizado(int idUsuario)
    {
        // Creamos un objeto simple para enviar el id_user a la nueva Lambda
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

    // ==========================================
    // CLASES DE ESTRUCTURA DE DATOS
    // ==========================================

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

    // ==========================================
    // LÓGICA DE HISTORIAL (TARJETAS)
    // ==========================================

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