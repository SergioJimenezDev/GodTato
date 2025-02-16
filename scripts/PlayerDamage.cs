using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDamage : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip damageSound;
    public AudioClip damageSound2; // Nuevo AudioClip

    private bool canTakeDamage = true; // Variable para controlar la capacidad de recibir da�o
    private float invulnerabilityDuration = 0.5f; // Duraci�n de la invulnerabilidad en segundos

    public Image damageScreen; // Referencia a la imagen de pantalla de da�o
    private float damageScreenDuration = 0.2f; // Duraci�n de la pantalla de da�o
    private float fadeOutDuration = 0.2f; // Duraci�n del desvanecimiento

    private bool canShowDamageScreen = true; // Controla si se puede mostrar la pantalla de da�o
    private float damageScreenCooldown = 0.5f; // Tiempo de espera entre efectos de pantalla de da�o

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogWarning("El componente AudioSource no est� adjunto al objeto.");
        }

        if (damageSound == null || damageSound2 == null)
        {
            Debug.LogWarning("Uno de los AudioClip (damageSound o damageSound2) no est� asignado en el inspector.");
        }

        if (damageScreen == null)
        {
            Debug.LogWarning("La imagen de pantalla de da�o no est� asignada en el inspector.");
        }
        else
        {
            damageScreen.enabled = true; // Aseg�rate de que la imagen est� habilitada
            Color color = damageScreen.color;
            color.a = 0; // Hacer la imagen completamente transparente al inicio
            damageScreen.color = color;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && canTakeDamage)
        {
            float baseDamage = 0f;

            // Obtener el nombre del objeto o de su ra�z (para clones)
            string enemyName = other.gameObject.name;
            if (other.gameObject.transform.parent != null)
            {
                enemyName = other.gameObject.transform.parent.name;
            }

            // Determinar el da�o seg�n el nombre del enemigo
            switch (enemyName)
            {
                case "Mutant":
                case "Mutant(Clone)":
                    baseDamage = 10f;
                    break;
                case "Mutant2":
                case "Mutant2(Clone)":
                    baseDamage = 15f;
                    break;
                case "Mutant3":
                case "Mutant3(Clone)":
                    baseDamage = 20f;
                    break;
                case "Mutant4":
                case "Mutant4(Clone)":
                    baseDamage = 40f;
                    break;
                case "Mutant5":
                case "Mutant5(Clone)":
                    baseDamage = 60f;
                    break;
                case "Boss":
                case "Boss(Clone)":
                    baseDamage = 30f;
                    break;
                case "Boss2":
                case "Boss2(Clone)":
                    baseDamage = 60f;
                    break;
                case "Boss3":
                case "Boss3(Clone)":
                    baseDamage = 70f;
                    break;
                default:
                    baseDamage = 10f; // Da�o base por defecto si no coincide con ninguno espec�fico
                    break;
            }

            HealthSystem.Instance.TakeDamage(baseDamage);

            // Activar el temporizador de invulnerabilidad
            StartCoroutine(InvulnerabilityTimer());

            // Reproducir sonido inmediatamente
            PlayDamageSound();

            // Mostrar la pantalla de da�o
            if (damageScreen != null && canShowDamageScreen)
            {
                StartCoroutine(ShowDamageScreen());
            }
        }
    }

    private IEnumerator InvulnerabilityTimer()
    {
        canTakeDamage = false;
        yield return new WaitForSeconds(invulnerabilityDuration);
        canTakeDamage = true;
    }

    private void PlayDamageSound()
    {
        if (audioSource != null && damageSound != null && damageSound2 != null)
        {
            // Generar un n�mero aleatorio entre 0 y 1
            float randomValue = Random.value;

            // Probabilidad del 50% para cada sonido
            if (randomValue < 0.5f)
            {
                Debug.Log("Reproduciendo sonido de da�o 1.");
                audioSource.PlayOneShot(damageSound);
            }
            else
            {
                Debug.Log("Reproduciendo sonido de da�o 2.");
                audioSource.PlayOneShot(damageSound2);
            }
        }
        else
        {
            Debug.LogWarning("No se puede reproducir el sonido de da�o porque AudioSource, damageSound o damageSound2 es nulo.");
        }
    }

    private IEnumerator ShowDamageScreen()
    {
        // Mostrar la imagen de pantalla de da�o
        canShowDamageScreen = false;
        damageScreen.enabled = true;

        // Hacer la imagen completamente opaca
        Color color = damageScreen.color;
        color.a = 0.25f;
        damageScreen.color = color;

        // Esperar la duraci�n especificada
        yield return new WaitForSeconds(damageScreenDuration);

        // Gradualmente desvanecer la imagen
        float elapsedTime = 0f;
        while (elapsedTime < fadeOutDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(0.25f, 0f, elapsedTime / fadeOutDuration);
            damageScreen.color = color;
            yield return null;
        }

        // Asegurarse de que la imagen est� completamente transparente
        color.a = 0f;
        damageScreen.color = color;

        // Deshabilitar la imagen de pantalla de da�o
        damageScreen.enabled = false;

        // Esperar el tiempo de enfriamiento antes de permitir otro efecto de pantalla de da�o
        yield return new WaitForSeconds(damageScreenCooldown);
        canShowDamageScreen = true;
    }
}
