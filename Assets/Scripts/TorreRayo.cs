using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class TorreRayo : MonoBehaviour
{
    [Header("Estadísticas de Daño")]
    public float danoBasePorSegundo = 5f; 
    public float aumentoDeDanoPorSegundo = 10f;
    public float danoMaximo = 50f;

    [Header("Referencias")]
    public Transform puntoDisparo; 

    [Header("Configuración Visual")]
    public float grosorRayo = 0.5f;

    private Transform objetivoActual;
    private Transform objetivoAnterior; 
    private float danoActualPorSegundo; 

    private List<Transform> enemigosEnRango = new List<Transform>();
    private LineRenderer lineRenderer;

    // Configura el rayo láser e inicializa el nivel de daño
    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.useWorldSpace = true;
        lineRenderer.enabled = false;

        danoActualPorSegundo = danoBasePorSegundo;
    }

    // Gestiona el láser, el aumento de daño por enfoque continuo y resetea las estadísticas si el objetivo cambia
    void Update()
    {
        enemigosEnRango.RemoveAll(enemigo => enemigo == null);

        ActualizarObjetivo();

        if (objetivoActual != null)
        {
            lineRenderer.enabled = true;
            lineRenderer.startWidth = grosorRayo;
            lineRenderer.endWidth = grosorRayo;
            
            lineRenderer.SetPosition(0, puntoDisparo.position);
            lineRenderer.SetPosition(1, objetivoActual.position);

            if (objetivoActual == objetivoAnterior)
            {
                danoActualPorSegundo += aumentoDeDanoPorSegundo * Time.deltaTime;

                if (danoActualPorSegundo > danoMaximo)
                {
                    danoActualPorSegundo = danoMaximo;
                }
            }
            else
            {
                danoActualPorSegundo = danoBasePorSegundo;
                objetivoAnterior = objetivoActual; 
            }

            LogicaEnemigo scriptEnemigo = objetivoActual.GetComponent<LogicaEnemigo>();
            if (scriptEnemigo != null)
            {
                scriptEnemigo.RecibirDano(danoActualPorSegundo * Time.deltaTime);
            }
        }
        else
        {
            lineRenderer.enabled = false;
            danoActualPorSegundo = danoBasePorSegundo;
            objetivoAnterior = null;
        }
    }

    // Busca el siguiente enemigo disponible si no esta atacando a uno
    void ActualizarObjetivo() 
    {
        if (objetivoActual != null && enemigosEnRango.Contains(objetivoActual)) return;
        
        if (enemigosEnRango.Count > 0) objetivoActual = enemigosEnRango[0];
        else objetivoActual = null;
    }

    // Añade enemigos a la lista cuando entran en el área de alcance de la torre
    private void OnTriggerEnter2D(Collider2D colision)
    {
        if (colision.CompareTag("Enemigo")) enemigosEnRango.Add(colision.transform);
    }

    // Retira enemigos de la lista cuando salen del área de alcance
    private void OnTriggerExit2D(Collider2D colision)
    {
        if (colision.CompareTag("Enemigo")) enemigosEnRango.Remove(colision.transform);
    }
}