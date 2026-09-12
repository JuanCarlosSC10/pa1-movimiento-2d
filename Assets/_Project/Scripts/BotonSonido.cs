using UnityEngine;
using UnityEngine.EventSystems;

public class BotonSonido : MonoBehaviour, IPointerClickHandler
{
    [Header("Clip de Sonido")]
    public AudioClip sonidoClick;

    [Header("Ajuste de Volumen (0.0 a 2.0)")]
    [Range(0f, 2f)] public float volumenClick = 1.5f;

    public void OnPointerClick(PointerEventData eventData)
    {
        // Dispara el sonido al instante en el AudioManager global
        if (AudioManager.Instance != null && sonidoClick != null)
        {
            AudioManager.Instance.sfxSource.PlayOneShot(sonidoClick, volumenClick);
        }
    }
}