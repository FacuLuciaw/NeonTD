using UnityEngine;

public class CodigoSecreto : MonoBehaviour
{
    [Header("Configuración")]
    public int oroDeRecompensa = 9999;
    public KeyCode[] secuenciaSecreta;

    private int indiceActual = 0;

    // Comprueba si la secuencia introducida por teclado es correcta y si falla empieza de nuevo
    void Update()
    {
        if (Input.anyKeyDown)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
                return;

            if (Input.GetKeyDown(secuenciaSecreta[indiceActual]))
            {
                indiceActual++;

                if (indiceActual >= secuenciaSecreta.Length)
                {
                    ActivarTruco();
                    indiceActual = 0; 
                }
            }
            else
            {
                indiceActual = 0;
            }
        }
    }

    void ActivarTruco()
    {
        if (GestorEconomia.instancia != null)
        {
            GestorEconomia.instancia.SumarOro(oroDeRecompensa);
        }
    }
}