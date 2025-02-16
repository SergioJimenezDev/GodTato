using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneAutoLoader : MonoBehaviour
{
    // Tiempo en segundos antes de cargar la siguiente escena
    public float delayBeforeLoading = 7f;

    // Nombre de la siguiente escena que se cargará
    public string nextSceneName;

    private Coroutine loadSceneCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        // Iniciar la coroutine para cargar la siguiente escena después de un retraso
        loadSceneCoroutine = StartCoroutine(LoadNextSceneAfterDelay());
    }

    // Update is called once per frame
    void Update()
    {
        // Comprobar si se ha pulsado la tecla "Escape"
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Si se pulsa "Escape", interrumpir la coroutine y cargar la siguiente escena inmediatamente
            if (loadSceneCoroutine != null)
            {
                StopCoroutine(loadSceneCoroutine);
            }
            LoadNextScene();
        }
    }

    private IEnumerator LoadNextSceneAfterDelay()
    {
        // Esperar el tiempo especificado
        yield return new WaitForSeconds(delayBeforeLoading);

        // Cargar la siguiente escena
        LoadNextScene();
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
