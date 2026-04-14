using UnityEngine;

public class LogicaBala : MonoBehaviour {
    public Transform objetivo;
    public float velocidad = 7f;

    // Dependiendo de cómo dibujaste tu bala (mirando arriba, a la derecha...), 
    // puede que necesites poner esto en -90, 0 o 90 para que la punta mire bien.
    public float compensacionRotacion = -90f; 

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
                scriptEnemigo.RecibirDaño(1f); 
            }

            Destroy(gameObject);
        }
    }
}