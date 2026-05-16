using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class GestorMusica : MonoBehaviour
{
    public static GestorMusica instancia;

    [Header("Conexiones")]
    public AudioSource fuenteMusica;
    public AudioSource fuenteEfectos; 
    public AudioMixer mezcladorAudio; 

    [Header("Lista de Reproducción")]
    public AudioClip[] listaCanciones; 
    
    private int indiceActual = 0;
    private bool cambiandoCancion = false;

    // Singletone 
    void Awake()
    {
        if (instancia == null)
        {
            instancia = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += AlCargarNuevaEscena;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= AlCargarNuevaEscena;
    }

    void AlCargarNuevaEscena(Scene escena, LoadSceneMode modo)
    {
        Invoke("AplicarVolumenGuardado", 0.1f);
    }

    void Start()
    {
        Invoke("AplicarVolumenGuardado", 0.1f);

        if (listaCanciones.Length > 0)
        {
            ReproducirCancion(0);
        }
    }

void Update()
    {
        if (listaCanciones.Length == 0 || fuenteMusica == null) return;

        // Mantener cancion aunque se cambie de pantalla
        if (!Application.isFocused) return;

        if (!fuenteMusica.isPlaying && !cambiandoCancion)
        {
            cambiandoCancion = true; 
            Invoke("SiguienteCancion", 1f); 
        }
        else if (fuenteMusica.isPlaying)
        {
            cambiandoCancion = false; 
        }
    }

    private void AplicarVolumenGuardado()
    {
        if (mezcladorAudio == null) return;

        // Cargar valores o inicializarlos
        float volMusica = PlayerPrefs.GetFloat("GuardadoVolMusica", 0.5f);
        float volFXs = PlayerPrefs.GetFloat("GuardadovolFXs", 0.5f);

        // Codigo para evitar errores
        volMusica = Mathf.Max(0.0001f, volMusica);
        volFXs = Mathf.Max(0.0001f, volFXs);

        mezcladorAudio.SetFloat("VolMusica", Mathf.Log10(volMusica) * 20);
        mezcladorAudio.SetFloat("VolFXs", Mathf.Log10(volFXs) * 20);
    }

    private void SiguienteCancion()
    {
        indiceActual++;
        if (indiceActual >= listaCanciones.Length)
        {
            indiceActual = 0;
        }
        ReproducirCancion(indiceActual);
    }

    private void ReproducirCancion(int indice)
    {
        fuenteMusica.clip = listaCanciones[indice];
        fuenteMusica.Play();
    }

    public void ReproducirEfecto(AudioClip sonidoFX)
    {
        if (sonidoFX != null && fuenteEfectos != null)
        {
            fuenteEfectos.PlayOneShot(sonidoFX);
        }
    }
}