using UnityEngine;
using System.Collections;

public class GeneradorOleadasNV2 : MonoBehaviour
{
    [Header("Prefabs de Enemigos")]
    public GameObject enemigoNormalPrefab;
    public GameObject enemigoFuertePrefab;

    [Header("Las 2 Rutas (Nivel 2)")]
    public Transform[] rutaIzquierda;
    public Transform[] rutaDerecha;

    [Header("Tiempos")]
    public float tiempoEntreNormales = 1.5f;
    public float esperaParaElFuerte = 3.0f;

    // Función que llamaremos desde el Botón
    public void EmpezarRonda()
    {
        StartCoroutine(SpawnOleada());
    }

    IEnumerator SpawnOleada()
    {
        // 1. Salen 3 oleadas de enemigos normales (por los 2 caminos a la vez)
        for (int i = 0; i < 3; i++)
        {
            CrearEnemigo(enemigoNormalPrefab, rutaIzquierda);
            CrearEnemigo(enemigoNormalPrefab, rutaDerecha);
            
            yield return new WaitForSeconds(tiempoEntreNormales);
        }

        // 2. Pausa dramática antes del jefe
        yield return new WaitForSeconds(esperaParaElFuerte);

        // 3. Sale un enemigo fuerte por cada camino
        CrearEnemigo(enemigoFuertePrefab, rutaIzquierda);
        CrearEnemigo(enemigoFuertePrefab, rutaDerecha);
        
        Debug.Log("¡Oleada doble generada!");
    }

    // El "creador" universal de enemigos
    void CrearEnemigo(GameObject prefab, Transform[] rutaAsignada)
    {
        // Comprobamos que todo existe para evitar errores rojos en consola
        if (prefab != null && rutaAsignada != null && rutaAsignada.Length > 0)
        {
            // Nace en el primer waypoint de la ruta que le toque
            GameObject nuevoEnemigo = Instantiate(prefab, rutaAsignada[0].position, Quaternion.identity);
            LogicaEnemigo scriptEnemigo = nuevoEnemigo.GetComponent<LogicaEnemigo>();
            
            // Le pasamos el mapa a su cerebro
            if (scriptEnemigo != null)
            {
                scriptEnemigo.puntos = rutaAsignada;
            }
        }
    }
}