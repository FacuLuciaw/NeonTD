using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using TMPro; // Necesario para leer los InputFields de TextMeshPro

public class GestorPantallas : MonoBehaviour
{
    [Header("Conecta tus pantallas aquí")]
    public GameObject pantallaInicioSesion;
    public GameObject pantallaRegistro;
    public GameObject pantallaMenu;

    [Header("Campos de Texto (Inputs)")]
    public TMP_InputField userLogin;
    public TMP_InputField passLogin;
    public TMP_InputField userRegister;
    public TMP_InputField passRegister;

    [Header("Configuración AWS")]
    public string apiURL = "https://68l3t5abk0.execute-api.us-east-1.amazonaws.com/default/Lambda_Unuty_user";


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
}