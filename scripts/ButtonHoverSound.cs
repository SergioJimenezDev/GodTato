using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverSound : MonoBehaviour, IPointerEnterHandler
{
    public AudioSource audioSource; // Referencia al componente AudioSource
    public AudioClip hoverSound;    // Referencia al clip de sonido que quieres reproducir

    // Este método se llama cuando el ratón pasa por encima del botón
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (audioSource != null && hoverSound != null)
        {
            audioSource.PlayOneShot(hoverSound);
        }
    }
}
