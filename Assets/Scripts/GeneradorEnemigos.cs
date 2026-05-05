using UnityEngine;
using System.Collections;
using UnityEngine.UI; 
using TMPro;

[System.Serializable]
public class GrupoEnemigos
{
    public string nombreGrupo = "Grupo (Ej: 5 Normales)"; 
    public GameObject prefabEnemigo;
    public int cantidad;
    public float tiempoEntreEllos = 1.5f;
    public float tiempoEsperaDespues = 3.0f;
    public string nombreEnemigoDB = ""; // Nombre del enemigo para la BD (si está vacío, usa el nombre del prefab)
}

[System.Serializable]
public class Ronda
{
    public string nombreRonda = "Ronda 1"; 
    public GrupoEnemigos[] grupos;
}

public class GeneradorEnemigos : MonoBehaviour
{
    [Header("Configuración del Camino")]
    public Transform puntoDeSalida;
    public Transform[] puntosCamino;

    [Header("Configuración de Oleadas")]
    public Ronda[] rondas;
    private int indiceRondaActual = 0;

    [Header("Interfaz")]
    public Button botonEmpezar;
    public AdministradorNivel adminNivel;
    
    [Header("HUD de Rondas")]
    public TextMeshProUGUI textoContadorRondas;

    // --- NUEVO: FUNCIÓN START ---
    // Esta función se ejecuta sola una vez al empezar el nivel
    void Start()
    {
        // Preparamos el texto para que muestre la ronda 1 desde el principio
        ActualizarTextoRondas(1);
    }
    // ----------------------------

    public void EmpezarSiguienteRonda()
    {
        if (indiceRondaActual < rondas.Length)
        {
            // --- NUEVO ---
            // Actualizamos el texto a la ronda que está a punto de salir
            ActualizarTextoRondas(indiceRondaActual + 1); 
            // -------------

            StartCoroutine(SpawnRonda(rondas[indiceRondaActual]));
            indiceRondaActual++;
        }
        else
        {
            Debug.Log("¡Has completado todas las rondas de este nivel!");
        }
    }

    IEnumerator SpawnRonda(Ronda rondaActual)
    {
        // 1. BLOQUEAMOS EL BOTÓN
        if (botonEmpezar != null) 
        {
            botonEmpezar.interactable = false;
        }

        // 2. Soltamos a todos los enemigos de la ronda
        foreach (GrupoEnemigos grupo in rondaActual.grupos)
        {
            for (int i = 0; i < grupo.cantidad; i++)
            {
                CrearEnemigo(grupo.prefabEnemigo);
                yield return new WaitForSeconds(grupo.tiempoEntreEllos);
            }
            yield return new WaitForSeconds(grupo.tiempoEsperaDespues);
        }

        // --- LA MAGIA NUEVA: EL VIGILANTE ---
        // Mientras siga existiendo al menos un objeto con el Tag "Enemigo" en el juego...
        while (GameObject.FindGameObjectWithTag("Enemigo") != null)
        {
            // ...esperamos medio segundo y volvemos a mirar (así no saturamos el procesador)
            yield return new WaitForSeconds(0.5f); 
        }
        // ------------------------------------

        Debug.Log("¡Mapa limpio! Ronda completada.");

        // Registrar la ronda completada
        if (GestorDatosPartida.instancia != null)
        {
            GestorDatosPartida.instancia.RegistrarRondaCompletada();
        }

        // 3. DESBLOQUEAMOS EL BOTÓN para la siguiente ronda
        if (botonEmpezar != null) 
        {
            botonEmpezar.interactable = true;
        }

        // Si ya no quedan rondas y la base sigue en pie, mostramos victoria
        if (indiceRondaActual >= rondas.Length && adminNivel != null && !adminNivel.juegoFinalizado)
        {
            adminNivel.MostrarVictoria();
        }
        else
        {
            // --- NUEVO ---
            // Si aún quedan rondas, actualizamos el texto para que anuncie la siguiente
            ActualizarTextoRondas(indiceRondaActual + 1);
            // -------------
        }
    }

    void CrearEnemigo(GameObject prefab, string nombreEnemigoDB = "")
    {
        if (prefab != null && puntoDeSalida != null)
        {
            GameObject nuevoEnemigo = Instantiate(prefab, puntoDeSalida.position, Quaternion.identity);
            LogicaEnemigo scriptEnemigo = nuevoEnemigo.GetComponent<LogicaEnemigo>();

            if (scriptEnemigo != null)
            {
                scriptEnemigo.puntos = puntosCamino;
                // Usar el nombre de BD si se proporciona, si no, usar el nombre del prefab
                scriptEnemigo.nombreEnemigo = !string.IsNullOrEmpty(nombreEnemigoDB) ? nombreEnemigoDB : prefab.name;

                // Registrar el enemigo como activo
                if (GestorDatosPartida.instancia != null)
                {
                    GestorDatosPartida.instancia.AgregarEnemigoActivo(scriptEnemigo.nombreEnemigo);
                }
            }
        }
    }

    // Esta función actualiza el texto usando el tamaño automático de tu lista
    public void ActualizarTextoRondas(int numeroDeRondaActual)
    {
        if (textoContadorRondas != null)
        {
            // Nos aseguramos de que el número visual no supere el total de rondas
            int rondaVisual = Mathf.Min(numeroDeRondaActual, rondas.Length);

            // rondas.Length detectará automáticamente si este nivel tiene 3, 5 o 20 rondas
            textoContadorRondas.text = rondaVisual + " / " + rondas.Length;
        }
    }
}