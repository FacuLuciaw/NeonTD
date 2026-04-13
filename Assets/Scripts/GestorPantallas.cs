using UnityEngine;
<<<<<<< Updated upstream
=======
<<<<<<< HEAD
<<<<<<< HEAD
using TMPro;
=======
=======
>>>>>>> 95dac050e8fc6ffc13fbd20b57392a5f49844870
>>>>>>> Stashed changes
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using TMPro; // Necesario para leer los InputFields de TextMeshPro
<<<<<<< Updated upstream
=======
<<<<<<< HEAD
>>>>>>> 95dac050e8fc6ffc13fbd20b57392a5f49844870
=======
>>>>>>> 95dac050e8fc6ffc13fbd20b57392a5f49844870
>>>>>>> Stashed changes

public class GestorPantallas : MonoBehaviour
{
    [Header("Conecta tus pantallas aquí")]
    public GameObject pantallaInicioSesion;
    public GameObject pantallaRegistro;
<<<<<<< Updated upstream
=======
<<<<<<< HEAD
<<<<<<< HEAD
    public GameObject pantallaMenuPrincipal; // NUEVO: El menú
    public GameObject pantallaHistorial;     // NUEVO: El historial

    [Header("Configuración del Historial")]  // NUEVO: Las variables para las tarjetas
    public GameObject prefabTarjeta;
    public Transform contenedorContent;

    [Header("Pantalla de Estadísticas")]
    public GameObject pantallaEstadisticas;
    public TMP_Text txtDanoRecibido;
    public TMP_Text txtDanoInfligido;
    public TMP_Text txtTotalTorres;

    // --- FUNCIONES DE AUTENTICACIÓN ---
=======
=======
>>>>>>> 95dac050e8fc6ffc13fbd20b57392a5f49844870
>>>>>>> Stashed changes
    public GameObject pantallaMenu;

    [Header("Campos de Texto (Inputs)")]
    public TMP_InputField userLogin;
    public TMP_InputField passLogin;
    public TMP_InputField userRegister;
    public TMP_InputField passRegister;

    [Header("Configuración AWS")]
    public string apiURL = "https://68l3t5abk0.execute-api.us-east-1.amazonaws.com/default/Lambda_Unuty_user";

<<<<<<< Updated upstream
=======
<<<<<<< HEAD
>>>>>>> 95dac050e8fc6ffc13fbd20b57392a5f49844870
=======
>>>>>>> 95dac050e8fc6ffc13fbd20b57392a5f49844870
>>>>>>> Stashed changes

    public void IrARegistro()
    {
        pantallaInicioSesion.SetActive(false);
        pantallaRegistro.SetActive(true);
        Debug.Log("Cambiando a pantalla de Registro");
    }

    public void IrAInicioSesion()
    {
        pantallaRegistro.SetActive(false);
        pantallaInicioSesion.SetActive(true);
        Debug.Log("Cambiando a pantalla de Inicio de Sesión");
    }

<<<<<<< Updated upstream
=======
<<<<<<< HEAD
<<<<<<< HEAD
    // --- NUEVAS FUNCIONES DE NAVEGACIÓN ---

    // Esta función nos sirve para entrar al menú tras el Login, o volver desde el Historial
    public void IrAMenuPrincipal()
    {
        pantallaInicioSesion.SetActive(false);
        pantallaHistorial.SetActive(false); // Apagamos el historial por si venimos de ahí

        pantallaMenuPrincipal.SetActive(true);
        Debug.Log("Cambiando al Menú Principal");
    }

    // Entrar al historial y cargar los datos
    public void IrAHistorial()
    {
        pantallaMenuPrincipal.SetActive(false);
        pantallaHistorial.SetActive(true);
        Debug.Log("Cambiando a pantalla de Historial");

        // Cuando abrimos la pantalla, le decimos que dibuje las tarjetas
        GenerarDatosDePrueba();
    }

    // --- LÓGICA DE DATOS (MOCK DATA) ---

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

    // Función para abrir estadísticas con datos de prueba
    public void IrAEstadisticas()
    {
        pantallaMenuPrincipal.SetActive(false);
        pantallaEstadisticas.SetActive(true);

        // Datos de prueba (Mock Data)
        txtDanoRecibido.text = "15,400";
        txtDanoInfligido.text = "42,850";
        txtTotalTorres.text = "128";
    }
=======
=======
>>>>>>> 95dac050e8fc6ffc13fbd20b57392a5f49844870
>>>>>>> Stashed changes

    // Se llama desde el botón "Iniciar Sesión"
    public void ClickLogin()
    {
        StartCoroutine(EnviarPeticion("login", userLogin.text, passLogin.text));
    }

    // Se llama desde el botón "Crear"
    public void ClickRegistro()
    {
        StartCoroutine(EnviarPeticion("registro", userRegister.text, passRegister.text));
    }

    IEnumerator EnviarPeticion(string accion, string user, string pass)
    {
        // Creamos el Json de datos para enviar
        string json = "{\"nickname\":\"" + user + "\", \"contrasena\":\"" + pass + "\", \"accion\":\"" + accion + "\"}";

        using (UnityWebRequest request = new UnityWebRequest(apiURL, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            Debug.Log($"Enviando {accion} para el usuario: {user}");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Respuesta de AWS: " + request.downloadHandler.text);
                
                if (accion == "registro")
                {
                    // Si el registro fue bien, lo mandamos al login para que entre
                    IrAInicioSesion();
                    //FALTA: Crear algo que me permita decirle que su cuenta fue creada con exito
                }
                else
                {
                    // Login exitoso
                    Debug.Log("¡Acceso concedido! Cargando juego...");
                    pantallaInicioSesion.SetActive(false);
                    pantallaMenu.SetActive(true);
                    
                }
            }
            else
            {
                if (request.responseCode == 404) 
                    Debug.LogWarning("El usuario no existe. Pulsa en Registrarse.");
                else if (request.responseCode == 409) 
                    Debug.LogError("El nombre ya está pillado. Elige otro.");
                else if (request.responseCode == 401)
                    Debug.LogError("Contraseña incorrecta.");
                else
                    Debug.LogError("Error de conexión: " + request.error);
            }
        }
    }
<<<<<<< Updated upstream
=======
<<<<<<< HEAD
>>>>>>> 95dac050e8fc6ffc13fbd20b57392a5f49844870
=======
>>>>>>> 95dac050e8fc6ffc13fbd20b57392a5f49844870
>>>>>>> Stashed changes
}