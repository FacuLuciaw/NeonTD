using UnityEngine;

public class LogicaBala : MonoBehaviour 
{
    public Transform objetivo;
    public float velocidad = 7f;
    public float compensacionRotacion = -90f; 

    [Header("Estadísticas del Proyectil")]
    public float dano = 1f; 

    [Header("Efecto Daño Continuo")]
    public bool aplicaDanoContinuo = false; 
    public float danoPorSegundo = 2f;
    public float duracionEfecto = 10f;

    void Update() 
    {
        // Si el enemigo al que la bala perseguía ya ha sido destruido, la bala también desaparece
        if (objetivo == null) 
        {
            Destroy(gameObject);
            return;
        }

        // 1. Mueve el proyectil progresivamente hacia la posición del objetivo
        transform.position = Vector2.MoveTowards(transform.position, objetivo.position, velocidad * Time.deltaTime);

        // 2. Calcula el ángulo para que el sprite de la bala rote y apunte visualmente hacia el enemigo
        Vector2 direccion = objetivo.position - transform.position;
        float angulo = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angulo + compensacionRotacion));

        // 3. Comprueba si la distancia entre la bala y el enemigo es lo bastante pequeña para ser un impacto
        if (Vector2.Distance(transform.position, objetivo.position) < 0.2f) 
        {
            LogicaEnemigo scriptEnemigo = objetivo.GetComponent<LogicaEnemigo>();
            
            if (scriptEnemigo != null) 
            {
                // Reduce la salud del enemigo
                scriptEnemigo.RecibirDano(dano); 

                // Aplica el efecto de D.O.T. si la bala pertenece a una torre con ese efecto
                if (aplicaDanoContinuo) 
                {
                    scriptEnemigo.AplicarDanoContinuo(danoPorSegundo, duracionEfecto, true);
                }
            }

            // Destruye el proyectil tras colisionar e infligir el daño
            Destroy(gameObject);
        }
    }
}