using UnityEngine;
using TMPro;

public class TarjetaPartida : MonoBehaviour
{
    [Header("Referencias de Texto")]
    public TMP_Text textResultado;
    public TMP_Text textFecha;
    public TMP_Text textDificultad;
    public TMP_Text textOroObtenido;
    public TMP_Text textEnemigos;
    public TMP_Text textTorres;

    // Rellena la tarjeta del historial con los datos de una partida de la base de datos
    public void ConfigurarTarjeta(string oro, string fecha, string resultado, string dificultad, string enemigos, string torres)
    {
        if (textOroObtenido != null) textOroObtenido.text = oro;
        if (textFecha != null) textFecha.text = fecha;
        if (textEnemigos != null) textEnemigos.text = enemigos;
        if (textTorres != null) textTorres.text = torres;

        // Aplica la traducción a la tarjeta
        if (textResultado != null) textResultado.text = GestorIdiomas.ObtenerResultadoTraducido(resultado);
        if (textDificultad != null) textDificultad.text = GestorIdiomas.ObtenerDificultadTraducida(dificultad);

        // Modifica el color del texto evaluando la cadena de la base de datos
        string resCrudo = resultado.ToLower();
        
        if (resCrudo.Contains("victoria"))
        {
            textResultado.color = Color.green;
        }
        else if (resCrudo.Contains("derrota"))
        {
            textResultado.color = Color.red;
        }
        else if (resCrudo.Contains("infinito"))
        {
            textResultado.color = Color.cyan;
        }
        else
        {
            textResultado.color = new Color(1f, 0.5f, 0f); 
        }
    }
}