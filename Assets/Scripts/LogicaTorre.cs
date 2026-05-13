using UnityEngine;
using System.Collections.Generic;

public class LogicaTorre : MonoBehaviour 
{
    [Header("Referencias Visuales")]
    public Transform canonGiratorio; 
    public Transform puntoDeDisparo; 

    [Header("Configuración Disparo")]
    public GameObject balaPrefab;
    public float cadenciaDisparo = 1f;
    private float tiempoSiguienteDisparo;
    
    public float compensacionRotacion = -90f; 

    private Transform objetivoActual;
    public List<GameObject> enemigosEnRango = new List<GameObject>();

    // Actualiza el estado de la torre en cada frame: limpia objetivos eliminados, busca nuevos y gestiona el disparo
    void Update() 
    {
        LimpiarLista(); 
        ActualizarObjetivo(); 

        if (objetivoActual != null) 
        {
            RotarHaciaObjetivo();

            if (Time.time >= tiempoSiguienteDisparo) 
            {
                Disparar();
                
                tiempoSiguienteDisparo += cadenciaDisparo; 

                // Previene ráfagas instantáneas si la torre pasó mucho tiempo inactiva sin enemigos
                if (tiempoSiguienteDisparo < Time.time) 
                {
                    tiempoSiguienteDisparo = Time.time + cadenciaDisparo;
                }
            }
        }
    }

    // Calcula el ángulo hacia el objetivo y rota la cabeza de la torre para apuntar
    void RotarHaciaObjetivo() 
    {
        if (canonGiratorio == null) return;

        Vector2 direccion = objetivoActual.position - canonGiratorio.position;
        float angulo = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;
        
        Quaternion rotacionDeseada = Quaternion.Euler(new Vector3(0, 0, angulo + compensacionRotacion));
        canonGiratorio.rotation = Quaternion.Lerp(canonGiratorio.rotation, rotacionDeseada, Time.deltaTime * 10f); 
    }

    // Instancia el proyectil en el punto de disparo del cañón y le transfiere la referencia del objetivo actual
    void Disparar() 
    {
        if (balaPrefab != null && objetivoActual != null && puntoDeDisparo != null) 
        {
            GameObject nuevaBala = Instantiate(balaPrefab, puntoDeDisparo.position, puntoDeDisparo.rotation);
            nuevaBala.GetComponent<LogicaBala>().objetivo = objetivoActual;
        }
    }

    // Examina la lista de enemigos en rango y asigna el primero como objetivo prioritario
    void ActualizarObjetivo() 
    {
        if (objetivoActual != null) return;

        GameObject candidato = null;
        foreach (GameObject enemigo in enemigosEnRango) 
        {
            if (enemigo != null) 
            {
                candidato = enemigo; 
                break; 
            }
        }
        
        if (candidato != null) objetivoActual = candidato.transform;
    }

    // Detecta a los enemigos que entran en el área de ataque y los añade a la lista de objetivos
    void OnTriggerEnter2D(Collider2D otro) 
    {
        if (otro.CompareTag("Enemigo")) 
        {
            enemigosEnRango.Add(otro.gameObject);
        }
    }

    // Elimina de la lista a los enemigos que salen del área 
    void OnTriggerExit2D(Collider2D otro) 
    {
        if (otro.CompareTag("Enemigo")) 
        {
            enemigosEnRango.Remove(otro.gameObject);
            
            if (objetivoActual == otro.transform) 
            {
                objetivoActual = null; 
            }
        }
    }

    // Limpia las referencias nulas de la lista causadas por enemigos destruidos antes de salir del rango
    void LimpiarLista() 
    {
        enemigosEnRango.RemoveAll(item => item == null);
    }
}