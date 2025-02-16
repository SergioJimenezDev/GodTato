using UnityEngine;
using System.Collections;

public class AutoMute : MonoBehaviour
{
    // Referencia al componente AudioSource
    private AudioSource audioSource;

    // Tiempo en segundos antes de silenciar el audio
    public float muteDelay = 10f;

    private void Start()
    {
        // Obtener el componente AudioSource adjunto al GameObject
        audioSource = GetComponent<AudioSource>();

        // Comprobar si el AudioSource existe
        if (audioSource == null)
        {
            Debug.LogError("No se encontró un AudioSource en el GameObject.");
            return;
        }

        // Iniciar la coroutine para silenciar el audio después del retraso especificado
        StartCoroutine(MuteAfterDelay());
    }

    private IEnumerator MuteAfterDelay()
    {
        // Esperar el tiempo especificado antes de silenciar el audio
        yield return new WaitForSeconds(muteDelay);

        // Silenciar el AudioSource
        audioSource.mute = true;
    }
}
