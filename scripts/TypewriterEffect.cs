using System.Collections;
using UnityEngine;
using TMPro;

public class TypewriterEffect : MonoBehaviour
{
    // Referencia al componente TextMeshProUGUI
    public TextMeshProUGUI uiText;

    // Texto completo que se mostrará
    [TextArea]
    public string fullText;

    // Tiempo de retraso entre la aparición de cada letra
    public float delay = 0.1f;

    // AudioSource para reproducir el sonido de cada letra
    public AudioSource typeSound;

    // Duración del fundido al finalizar el texto
    public float fadeDuration = 1f;

    private void Start()
    {
        // Iniciar la coroutine para mostrar el texto letra por letra
        StartCoroutine(ShowText());
    }

    private IEnumerator ShowText()
    {
        uiText.text = "";  // Vaciar el texto al inicio

        foreach (char letter in fullText.ToCharArray())
        {
            uiText.text += letter;  // Añadir la letra actual al texto mostrado

            // Reproducir el sonido si está asignado
            if (typeSound != null)
            {
                typeSound.Play();
            }

            // Esperar el tiempo especificado antes de mostrar la siguiente letra
            yield return new WaitForSeconds(delay);
        }

        // Una vez completada la escritura, iniciar el fundido
        StartCoroutine(FadeOutText());
    }

    private IEnumerator FadeOutText()
    {
        float timer = 0f;
        Color originalColor = uiText.color;
        Color transparentColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0f); // Color completamente transparente

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            uiText.color = Color.Lerp(originalColor, transparentColor, timer / fadeDuration);
            yield return null;
        }

        // Asegurar que el texto esté completamente transparente al finalizar el fundido
        uiText.color = transparentColor;
    }
}
