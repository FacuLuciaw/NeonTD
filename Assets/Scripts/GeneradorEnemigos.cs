using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class GrupoEnemigos
{
    public string nombreGrupo = "Grupo";
    public GameObject prefabEnemigo;
    public int cantidad;
    public float tiempoEntreEllos = 1.5f;
    public float tiempoEsperaDespues = 3.0f;
    public string nombreEnemigoDB = "";
}

[System.Serializable]
public class Ronda
{
    public string nombreRonda = "Ronda";
    public GrupoEnemigos[] grupos;
}

public class GeneradorEnemigos : MonoBehaviour
{
    [Header("Configuración del Camino")]
    public Transform puntoDeSalida;
    public Transform[] puntosCamino;

    [Header("Configuración de Oleadas")]
    public Ronda[] rondas;
    private int indiceRondaActual = 0;

    [Header("Interfaz")]
    public Button botonEmpezar;
    public AdministradorNivel adminNivel;

    [Header("HUD de Rondas")]
    public TextMeshProUGUI textoContadorRondas;

    [Header("Modo Infinito")]
    public bool modoInfinitoActivo = false;
    private int nivelInfinitoActual = 1;

    public int gruposInicialesInfinito = 5;
    public GrupoEnemigos[] poolGruposInfinitos;

    void Start()
    {
        ActualizarTextoRondas(1);
    }

    public void EmpezarSiguienteRonda()
    {
        if (modoInfinitoActivo)
        {
            ActualizarTextoRondas(nivelInfinitoActual);
            StartCoroutine(SpawnRondaInfinita());
        }
        else if (indiceRondaActual < rondas.Length)
        {
            ActualizarTextoRondas(indiceRondaActual + 1);
            StartCoroutine(SpawnRonda(rondas[indiceRondaActual]));
            indiceRondaActual++;
        }
    }

    // Gestiona la creación de enemigos en las rondas normales 
    IEnumerator SpawnRonda(Ronda rondaActual)
    {
        if (GestorDatosPartida.instancia != null)
        {
            GestorDatosPartida.instancia.IniciarNuevaRonda();
        }

        if (botonEmpezar != null) botonEmpezar.interactable = false;

        foreach (GrupoEnemigos grupo in rondaActual.grupos)
        {
            for (int i = 0; i < grupo.cantidad; i++)
            {
                CrearEnemigo(grupo.prefabEnemigo, grupo.nombreEnemigoDB);
                yield return new WaitForSeconds(grupo.tiempoEntreEllos);
            }
            yield return new WaitForSeconds(grupo.tiempoEsperaDespues);
        }

        // Bloquea el final de ronda hasta que el jugador haya eliminado al último enemigo
        while (GameObject.FindGameObjectWithTag("Enemigo") != null)
        {
            yield return new WaitForSeconds(0.5f);
        }

        if (GestorDatosPartida.instancia != null)
        {
            GestorDatosPartida.instancia.RegistrarRondaCompletada();
        }

        if (botonEmpezar != null) botonEmpezar.interactable = true;

        // Si ya no quedan más rondas, avisa al administrador para que muestre la pantalla de victoria
        if (indiceRondaActual >= rondas.Length && adminNivel != null && !adminNivel.juegoFinalizado)
        {
            adminNivel.MostrarVictoria();
        }
        else
        {
            ActualizarTextoRondas(indiceRondaActual + 1);
        }
    }

    // Funciona igual que la ronda normal, pero escoge grupos de enemigos al azar de forma infinita
    IEnumerator SpawnRondaInfinita()
    {
        if (GestorDatosPartida.instancia != null)
        {
            GestorDatosPartida.instancia.IniciarNuevaRonda();
        }

        if (poolGruposInfinitos == null || poolGruposInfinitos.Length == 0)
        {
            yield break;
        }

        if (botonEmpezar != null) botonEmpezar.interactable = false;

        // La cantidad de grupos aumenta en 1 con cada nueva oleada infinita
        int cantidadGruposEstaRonda = gruposInicialesInfinito + (nivelInfinitoActual - 1);

        for (int g = 0; g < cantidadGruposEstaRonda; g++)
        {
            int indiceAleatorio = Random.Range(0, poolGruposInfinitos.Length);
            GrupoEnemigos grupoElegido = poolGruposInfinitos[indiceAleatorio];

            for (int i = 0; i < grupoElegido.cantidad; i++)
            {
                CrearEnemigo(grupoElegido.prefabEnemigo, grupoElegido.nombreEnemigoDB);
                yield return new WaitForSeconds(grupoElegido.tiempoEntreEllos);
            }
            yield return new WaitForSeconds(grupoElegido.tiempoEsperaDespues);
        }

        while (GameObject.FindGameObjectWithTag("Enemigo") != null)
        {
            yield return new WaitForSeconds(0.5f);
        }

        if (GestorDatosPartida.instancia != null)
        {
            GestorDatosPartida.instancia.RegistrarRondaCompletada();
        }

        nivelInfinitoActual++;
        ActualizarTextoRondas(nivelInfinitoActual);

        if (botonEmpezar != null) botonEmpezar.interactable = true;
    }

    // Crea los enemigos en el punto de salida y les asigna la ruta que deben seguir
    void CrearEnemigo(GameObject prefab, string nombreEnemigoDB = "")
    {
        if (prefab != null && puntoDeSalida != null)
        {
            GameObject nuevoEnemigo = Instantiate(prefab, puntoDeSalida.position, Quaternion.identity);
            LogicaEnemigo scriptEnemigo = nuevoEnemigo.GetComponent<LogicaEnemigo>();

            if (scriptEnemigo != null)
            {
                scriptEnemigo.puntos = puntosCamino;
                scriptEnemigo.nombreEnemigo = !string.IsNullOrEmpty(nombreEnemigoDB) ? nombreEnemigoDB : prefab.name;

                if (GestorDatosPartida.instancia != null)
                {
                    GestorDatosPartida.instancia.AgregarEnemigoActivo(scriptEnemigo.nombreEnemigo);
                }
            }
        }
    }

    // Cambia el texto de ronda dependiendo del modo (infinito/normal)
    public void ActualizarTextoRondas(int numeroDeRondaActual)
    {
        if (textoContadorRondas != null)
        {
            if (modoInfinitoActivo)
            {
                string etiqueta = GestorIdiomas.ObtenerEtiquetaInfinita();
                textoContadorRondas.text = etiqueta + ": " + numeroDeRondaActual;
            }
            else
            {
                int rondaVisual = Mathf.Min(numeroDeRondaActual, rondas.Length);
                textoContadorRondas.text = rondaVisual + " / " + rondas.Length;
            }
        }
    }

    // Llama al modo infinito (solo si hay un camino)
    public void ActivarModoInfinito()
    {
        modoInfinitoActivo = true;
        ActualizarTextoRondas(nivelInfinitoActual);
    }

    // Llama al modo infinito (cuando hay varios caminos)
    public void ActivarModoInfinitoEnTodosLosScripts()
    {
        GeneradorEnemigos[] todosLosGeneradores = GetComponents<GeneradorEnemigos>();

        foreach (GeneradorEnemigos generador in todosLosGeneradores)
        {
            generador.modoInfinitoActivo = true;
            generador.ActualizarTextoRondas(generador.nivelInfinitoActual);
        }
    }
}