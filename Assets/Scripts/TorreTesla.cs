using System.Collections.Generic;
using UnityEngine;

public class TorreTesla : MonoBehaviour
{
    [Header("Efecto Visual")]
    public GameObject prefabCuadradoAmarillo; 
    public LayerMask capaCamino;              
    public float tamanoCasilla = 100f;        
    public int ordenVisualEfecto = 10; 

    [Header("Estadísticas")]
    public float danoPorSegundo = 1f;
    public float multiplicadorVelocidad = 0.5f; 

    private List<LogicaEnemigo> enemigosEnRango = new List<LogicaEnemigo>();

    public static List<Vector2> casillasInfectadasGlobales = new List<Vector2>();
    private List<Vector2> misCasillasInfectadas = new List<Vector2>();

    public static List<TorreTesla> todasLasTeslas = new List<TorreTesla>();

    // Registra la torre en el grupo de comunicación y empieza el escaneo del terreno
    void Start()
    {
        todasLasTeslas.Add(this);
        EscanearYGenerarEfectos();
    }

    // Analiza las casillas adyacentes y genera el efecto visual si no están ocupadas por otra torre Tesla
    public void EscanearYGenerarEfectos()
    {
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                Vector2 posicionCasilla = new Vector2(
                    transform.position.x + (x * tamanoCasilla),
                    transform.position.y + (y * tamanoCasilla)
                );

                Collider2D impacto = Physics2D.OverlapPoint(posicionCasilla, capaCamino);

                if (impacto != null)
                {
                    Vector2 posRedondeada = new Vector2(Mathf.Round(posicionCasilla.x), Mathf.Round(posicionCasilla.y));

                    if (!casillasInfectadasGlobales.Contains(posRedondeada))
                    {
                        casillasInfectadasGlobales.Add(posRedondeada);
                        misCasillasInfectadas.Add(posRedondeada);

                        GameObject efecto = Instantiate(prefabCuadradoAmarillo, posicionCasilla, Quaternion.identity);
                        efecto.transform.SetParent(transform);

                        SpriteRenderer sr = efecto.GetComponent<SpriteRenderer>();
                        if (sr != null) sr.sortingOrder = ordenVisualEfecto; 
                    }
                }
            }
        }
    }

    // Aplica el efecto de ralentización y daño a los enemigos que entran en el área
    private void OnTriggerEnter2D(Collider2D colision)
    {
        LogicaEnemigo enemigo = colision.GetComponentInParent<LogicaEnemigo>();
        
        if (enemigo != null && !enemigosEnRango.Contains(enemigo))
        {
            enemigosEnRango.Add(enemigo);
            enemigo.EntrarEnTesla(multiplicadorVelocidad, danoPorSegundo); 
        }
    }

    // Libera a los enemigos del efecto Tesla al salir del área
    private void OnTriggerExit2D(Collider2D colision)
    {
        LogicaEnemigo enemigo = colision.GetComponentInParent<LogicaEnemigo>();
        if (enemigo != null && enemigosEnRango.Contains(enemigo))
        {
            enemigosEnRango.Remove(enemigo);
            enemigo.SalirDeTesla(); 
        }
    }

    // Gestiona la limpieza de registros al vender la torre y notifica a las demás para que cubran huecos visuales
    void OnDestroy()
    {
        todasLasTeslas.Remove(this);

        foreach (Vector2 baldosa in misCasillasInfectadas)
        {
            casillasInfectadasGlobales.Remove(baldosa);
        }

        foreach (LogicaEnemigo enemigo in enemigosEnRango)
        {
            if (enemigo != null) enemigo.SalirDeTesla();
        }

        foreach (TorreTesla torre in todasLasTeslas)
        {
            if (torre != null)
            {
                torre.EscanearYGenerarEfectos();
            }
        }
    }
}