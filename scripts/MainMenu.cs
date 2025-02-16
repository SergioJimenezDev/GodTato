using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; // Importa System.Collections para usar IEnumerator

public class MainMenu : MonoBehaviour
{
    // Este método se puede vincular al evento OnClick del botón en el Inspector de Unity
    public void ChangeToGameScene()
    {
        // Carga la escena y sus datos de iluminación
        StartCoroutine(LoadYourAsyncScene("GameScene"));
    }

    private IEnumerator LoadYourAsyncScene(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // Espera hasta que la escena esté completamente cargada
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Ajusta la iluminación después de cargar la escena
        LightProbes.Tetrahedralize();
        DynamicGI.UpdateEnvironment();
    }

    // Este método se puede vincular al evento OnClick del botón en el Inspector de Unity
    public void QuitGame()
    {
        Application.Quit();

    }
}
