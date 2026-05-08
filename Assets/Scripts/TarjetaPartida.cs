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

    // Esta es la función que llama el GestorPantallas pasando todos los datos
    public void ConfigurarTarjeta(string oro, string fecha, string resultado, string dificultad, string enemigos, string torres)
    {
        // Asignamos cada string al componente de texto correspondiente
        if (textOroObtenido != null) textOroObtenido.text = oro;
        if (textFecha != null) textFecha.text = fecha;
        if (textResultado != null) textResultado.text = resultado.ToUpper();
        if (textDificultad != null) textDificultad.text = dificultad;
        if (textEnemigos != null) textEnemigos.text = enemigos;
        if (textTorres != null) textTorres.text = torres;

        // Extra: Cambio de color automático según el resultado
        if (resultado.ToLower() == "victoria")
            textResultado.color = Color.green;
        else if (resultado.ToLower() == "derrota")
            textResultado.color = Color.red;
        else if (resultado.ToLower() == "infinito")
            textResultado.color = Color.cyan;
        else
            textResultado.color = Color.orange; // Para casos como "Rendición"
    }
}