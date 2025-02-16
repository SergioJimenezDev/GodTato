using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Powerups : MonoBehaviour
{
    private bool chromaticAberrationActive = false;
    private float chromaticAberrationDuration = 5f; // Duración en segundos antes de desactivar el Chromatic Aberration

    private GameObject postProcessVolume; // GameObject de la escena que contiene el PostProcess Volume

    private void Start()
    {
        // Buscar automáticamente el GameObject llamado "PostProcess" en la escena
        postProcessVolume = GameObject.Find("PostProcess");
        if (postProcessVolume == null)
        {
            Debug.LogError("No se encontró el GameObject 'PostProcess' en la escena.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Verificar si el objeto que colisiona es el jugador
        if (other.CompareTag("Player"))
        {
            // Obtener el CharacterController del jugador
            CharacterController controller = other.GetComponent<CharacterController>();
            if (controller != null)
            {
                // Desactivar las colisiones del CharacterController
                controller.detectCollisions = false;
            }

            // Hacer el objeto invisible
            gameObject.SetActive(false);

            // Programar la destrucción del objeto después de 6 segundos
            Invoke("DestroyObject", 6f);

            // Verificar que se haya encontrado el GameObject 'PostProcess'
            if (postProcessVolume != null)
            {
                // Obtener el componente Volume del GameObject de la escena
                Volume volume = postProcessVolume.GetComponent<Volume>();
                if (volume != null)
                {
                    // Activar el Chromatic Aberration
                    ChromaticAberration chromaticAberration;
                    if (volume.profile.TryGet(out chromaticAberration))
                    {
                        chromaticAberration.active = true;
                        chromaticAberrationActive = true;

                        // Iniciar el temporizador para desactivar el Chromatic Aberration después de un tiempo
                        Invoke("DeactivateChromaticAberration", chromaticAberrationDuration);
                    }
                }
            }
        }
    }

    private void DeactivateChromaticAberration()
    {
        if (chromaticAberrationActive && postProcessVolume != null)
        {
            // Obtener el componente Volume del GameObject de la escena
            Volume volume = postProcessVolume.GetComponent<Volume>();
            if (volume != null)
            {
                // Desactivar el Chromatic Aberration
                ChromaticAberration chromaticAberration;
                if (volume.profile.TryGet(out chromaticAberration))
                {
                    chromaticAberration.active = false;
                    chromaticAberrationActive = false;
                }
            }
        }

        // Reactivar las colisiones del CharacterController del jugador
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            CharacterController controller = player.GetComponent<CharacterController>();
            if (controller != null)
            {
                controller.detectCollisions = true;
            }
        }
    }

    private void DestroyObject()
    {
        // Destruir el objeto actual
        Destroy(gameObject);
    }
}
