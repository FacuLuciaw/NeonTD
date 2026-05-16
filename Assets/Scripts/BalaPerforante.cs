using System.Collections.Generic;
using UnityEngine;

public class BalaPerforante : MonoBehaviour 
{
    [Header("Estadísticas")]
    public float velocidad = 30f;
    public float dano = 2f;
    public float tiempoDeVida = 5f; 
    public float compensacionRotacion = -90f;

    [Header("Efecto D.O.T.")]
    public bool aplicaDanoContinuo = false; 
    public bool seAcumulaElDano = false; 
    public float danoPorSegundo = 2f;
    public float duracionEfecto = 10f;

    private Vector2 direccionDisparo;
    private List<GameObject> enemigosGolpeados = new List<GameObject>();

    // Inicializa la trayectoria del proyectil, ajusta su rotación visual y programa su destrucción automática
    public void ConfigurarDireccion(Vector2 direccion)
    {
        direccionDisparo = direccion.normalized;
        
        float angulo = Mathf.Atan2(direccionDisparo.y, direccionDisparo.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angulo + compensacionRotacion);

        Destroy(gameObject, tiempoDeVida);
    }

    // Desplaza la bala de forma constante en la dirección configurada, ignorando obstáculos físicos
    void Update()
    {
        transform.Translate(direccionDisparo * velocidad * Time.deltaTime, Space.World);
    }

    // Detecta colisiones, aplica el daño base y continuo, asegurando que cada enemigo sea golpeado solo una vez
    private void OnTriggerEnter2D(Collider2D colision)
    {
        LogicaEnemigo enemigo = colision.GetComponentInParent<LogicaEnemigo>();

        if (enemigo != null)
        {
            if (!enemigosGolpeados.Contains(colision.gameObject))
            {
                enemigo.RecibirDano(dano);

                if (aplicaDanoContinuo)
                {
                    enemigo.AplicarDanoContinuo(danoPorSegundo, duracionEfecto, seAcumulaElDano);
                }

                enemigosGolpeados.Add(colision.gameObject); 
            }
        }
    }
}