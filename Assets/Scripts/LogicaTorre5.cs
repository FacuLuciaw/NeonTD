using System.Collections.Generic;
using UnityEngine;

public class LogicaTorre5 : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject prefabBala;
    public Transform puntoDisparo;
    public Transform canon; 

    [Header("Configuración")]
    public float cadencia = 2f;
    public float compensacionRotacion = -90f; 
    
    private float temporizador;
    
    private List<Transform> enemigosEnRango = new List<Transform>();
    private Transform objetivoActual;

    // Gestiona la torre: eliminación de objetivos, control del tiempo y disparo
    void Update()
    {
        enemigosEnRango.RemoveAll(enemigo => enemigo == null);

        temporizador += Time.deltaTime;

        // Limita el temporizador a la cadencia máxima para evitar disparos instantáneos acumulados tras periodos de inactividad
        if (temporizador > cadencia) 
        {
            temporizador = cadencia;
        }

        ActualizarObjetivo();

        if (objetivoActual != null)
        {
            Vector2 direccion = objetivoActual.position - canon.position;
            float angulo = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;
            canon.rotation = Quaternion.Euler(new Vector3(0, 0, angulo + compensacionRotacion));

            if (temporizador >= cadencia)
            {
                Disparar();
                
                temporizador -= cadencia; 
            }
        }
    }

    // Mantiene el enfoque en el objetivo actual si sigue en rango, o selecciona el primero disponible
    void ActualizarObjetivo()
    {
        if (objetivoActual != null && enemigosEnRango.Contains(objetivoActual))
        {
            return; 
        }

        if (enemigosEnRango.Count > 0)
        {
            objetivoActual = enemigosEnRango[0];
        }
        else
        {
            objetivoActual = null;
        }
    }

    // Instancia el proyectil y le transfiere la dirección hacia el objetivo
    void Disparar()
    {
        GameObject nuevaBala = Instantiate(prefabBala, puntoDisparo.position, Quaternion.identity);
        Vector2 direccion = objetivoActual.position - puntoDisparo.position;

        BalaPerforante scriptBala = nuevaBala.GetComponent<BalaPerforante>();
        if (scriptBala != null)
        {
            scriptBala.ConfigurarDireccion(direccion);
        }
    }

    // Registra a los enemigos que entran dentro del área 
    private void OnTriggerEnter2D(Collider2D colision)
    {
        if (colision.CompareTag("Enemigo"))
        {
            enemigosEnRango.Add(colision.transform);
        }
    }

    // Elimina de los registros a los enemigos que salen del área
    private void OnTriggerExit2D(Collider2D colision)
    {
        if (colision.CompareTag("Enemigo"))
        {
            enemigosEnRango.Remove(colision.transform);
        }
    }
}