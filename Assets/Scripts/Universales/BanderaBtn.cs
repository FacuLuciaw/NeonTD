using UnityEngine;
using UnityEngine.UI;

// ¡ESTA LÍNEA ES LA CULPABLE! Debe tener ": MonoBehaviour" al final
public class BanderaBtn : MonoBehaviour 
{
    public Sprite banderaEspana;
    public Sprite banderaInglaterra;
    
    private Image miImagen;

    void OnEnable() 
    {
        ActualizarBandera();
    }

    public void ActualizarBandera()
    {
        if (miImagen == null) miImagen = GetComponent<Image>();
        
        int idioma = PlayerPrefs.GetInt("IdiomaActual", 0);
        
        if (idioma == 0)
        {
            miImagen.sprite = banderaEspana;
        }
        else
        {
            miImagen.sprite = banderaInglaterra;
        }
    }
}