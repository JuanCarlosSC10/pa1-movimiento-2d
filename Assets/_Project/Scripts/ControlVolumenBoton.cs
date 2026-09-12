using UnityEngine;
using UnityEngine.UI;

public class ControlVolumenBoton : MonoBehaviour
{
    [Header("Sprites para el Botón")]
    [SerializeField] private Sprite spriteEncendido;
    [SerializeField] private Sprite spriteApagado;

    private Image imagenBoton;

    private void Awake()
    {
        // Guardamos la referencia a la imagen del mismo botón
        imagenBoton = GetComponent<Image>();
    }

    private void Start()
    {
        ActualizarIcono();
    }


    public void MutoONoMute()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.AlternarMute();

            ActualizarIcono();
        }
    }

    public void ActualizarIcono()
    {
        if (AudioManager.Instance != null && imagenBoton != null)
        {
       
            bool estaMuteado = AudioManager.Instance.estaMuteado;

            if (estaMuteado && spriteApagado != null)
            {
                imagenBoton.sprite = spriteApagado;
            }
            else if (!estaMuteado && spriteEncendido != null)
            {
                imagenBoton.sprite = spriteEncendido;
            }
        }
    }
}