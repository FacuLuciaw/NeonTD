using UnityEngine;
using System.Collections;

public class GeneradorEnemigos : MonoBehaviour
{
    [Header("Prefabs de Enemigos")]
    public GameObject enemigoNormalPrefab;
    public GameObject enemigoFuertePrefab;

    [Header("Configuración del Camino")]
    public Transform puntoDeSalida;
    public Transform[] puntosCamino;

    [Header("Tiempos")]
    public float tiempoEntreNormales = 1.5f;
    public float esperaParaElFuerte = 3.0f;

    // Quitamos el Start() y creamos esta función pública
    public void EmpezarRonda()
    {
        StartCoroutine(SpawnOleada());
    }

    IEnumerator SpawnOleada()
    {
        for (int i = 0; i < 3; i++)
        {
            CrearEnemigo(enemigoNormalPrefab);
            yield return new WaitForSeconds(tiempoEntreNormales);
        }

        yield return new WaitForSeconds(esperaParaElFuerte);

        CrearEnemigo(enemigoFuertePrefab);
        
        Debug.Log("¡Oleada completada!");
    }

    void CrearEnemigo(GameObject prefab)
    {
        if (prefab != null && puntoDeSalida != null)
        {
            GameObject nuevoEnemigo = Instantiate(prefab, puntoDeSalida.position, Quaternion.identity);
            LogicaEnemigo scriptEnemigo = nuevoEnemigo.GetComponent<LogicaEnemigo>();
            
            if (scriptEnemigo != null)
            {
                scriptEnemigo.puntos = puntosCamino;
            }
        }
    }
}