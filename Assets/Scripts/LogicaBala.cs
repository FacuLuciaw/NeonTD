using UnityEngine;

public class LogicaBala : MonoBehaviour {
    public Transform objetivo;
    public float velocidad = 7f;

    // Aquí iría tu variable compensacionRotacion...
    public float compensacionRotacion = -90f; 

    // ✅ ¡PARTE NUEVA 1: LA VARIABLE! 
    // Ahora puedes modificar este daño desde el Inspector de Unity.
    // Por defecto es 1, pero puedes subirlo a 50, 100, etc.
    [Header("Estadísticas del Proyectil")]
    public float dano = 1f; 

    [Header("Efecto Daño Continuo")]
    public bool aplicaDañoContinuo = false; // Casilla para activar si es bala de Torre 4
    public float danoPorSegundo = 2f;
    public float duracionEfecto = 10f;

    void Update() {
        if (objetivo == null) {
            Destroy(gameObject);
            return;
        }

        // 1. Mover la bala hacia el objetivo
        transform.position = Vector2.MoveTowards(transform.position, objetivo.position, velocidad * Time.deltaTime);

        // 2. Rotar la imagen de la bala para que apunte hacia el enemigo
        Vector2 direccion = objetivo.position - transform.position;
        float angulo = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angulo + compensacionRotacion));

        // 3. Comprobar si impactamos
        if (Vector2.Distance(transform.position, objetivo.position) < 0.2f) {
            LogicaEnemigo scriptEnemigo = objetivo.GetComponent<LogicaEnemigo>();
            
            if (scriptEnemigo != null) {
                // ✅ ¡PARTE NUEVA 2: USAR LA VARIABLE!
                // Cambiamos el 1f fijo por el valor que le pongas a la variable "dano"
                scriptEnemigo.RecibirDaño(dano); 

                // ✅ ¡PARTE NUEVA 3: DAÑO CONTINUO DE LA TORRE 4!
                if (aplicaDañoContinuo) {
                    scriptEnemigo.AplicarDañoContinuo(danoPorSegundo, duracionEfecto);
                }
            }

            Destroy(gameObject);
        }
    }
}