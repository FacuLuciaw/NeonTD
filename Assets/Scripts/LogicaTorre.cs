using UnityEngine;
using System.Collections.Generic;

public class LogicaTorre : MonoBehaviour {
    [Header("Referencias Visuales")]
    public Transform cañonGiratorio; // Arrastraremos el objeto Cañon aquí
    public Transform puntoDeDisparo; // Arrastraremos el objeto PuntoDeDisparo aquí

    [Header("Configuración Disparo")]
    public GameObject balaPrefab;
    public float cadenciaDisparo = 1f;
    private float tiempoSiguienteDisparo;
    
    // Este valor ajusta el ángulo del cañón. 
    // Si tu dibujo del cañón apunta hacia arriba por defecto, pon -90. 
    // Si apunta hacia la derecha, pon 0.
    public float compensacionRotacion = -90f; 

    private Transform objetivoActual;
    public List<GameObject> enemigosEnRango = new List<GameObject>();

    void Update() {
        LimpiarLista(); 
        ActualizarObjetivo(); 

        if (objetivoActual != null) {
            RotarHaciaObjetivo(); // <-- ¡Nueva función en acción!

            if (Time.time >= tiempoSiguienteDisparo) {
                Disparar();
                tiempoSiguienteDisparo = Time.time + cadenciaDisparo;
            }
        }
    }

    void RotarHaciaObjetivo() {
        if (cañonGiratorio == null) return;

        // Calculamos la dirección matemática hacia el enemigo
        Vector2 direccion = objetivoActual.position - cañonGiratorio.position;
        
        // Convertimos esa dirección en un ángulo
        float angulo = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;
        
        // Aplicamos la rotación suavemente
        Quaternion rotacionDeseada = Quaternion.Euler(new Vector3(0, 0, angulo + compensacionRotacion));
        cañonGiratorio.rotation = Quaternion.Lerp(cañonGiratorio.rotation, rotacionDeseada, Time.deltaTime * 10f); // El 10f es la velocidad de giro
    }

    void Disparar() {
        if (balaPrefab != null && objetivoActual != null && puntoDeDisparo != null) {
            // Ahora la bala sale desde la punta del cañón, no desde el centro de la base
            GameObject nuevaBala = Instantiate(balaPrefab, puntoDeDisparo.position, puntoDeDisparo.rotation);
            nuevaBala.GetComponent<LogicaBala>().objetivo = objetivoActual;
        }
    }

    void ActualizarObjetivo() {
        if (objetivoActual != null) return;

        GameObject candidato = null;
        foreach (GameObject enemigo in enemigosEnRango) {
            if (enemigo != null) {
                candidato = enemigo; 
                break; 
            }
        }
        if (candidato != null) objetivoActual = candidato.transform;
    }

    void OnTriggerEnter2D(Collider2D otro) {
        if (otro.CompareTag("Enemigo")) {
            enemigosEnRango.Add(otro.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D otro) {
        if (otro.CompareTag("Enemigo")) {
            enemigosEnRango.Remove(otro.gameObject);
            if (objetivoActual == otro.transform) {
                objetivoActual = null; 
            }
        }
    }

    void LimpiarLista() {
        enemigosEnRango.RemoveAll(item => item == null);
    }
}