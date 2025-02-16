using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverSound : MonoBehaviour, IPointerEnterHandler
{
    public AudioSource audioSource; // Referencia al componente AudioSource
    public AudioClip hoverSound;    // Referencia al clip de sonido que quieres reproducir

    // Este m�todo se llama cuando el rat�n pasa por encima del bot�n
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (audioSource != null && hoverSound != null)
        {
            audioSource.PlayOneShot(hoverSound);
        }
    }
}
