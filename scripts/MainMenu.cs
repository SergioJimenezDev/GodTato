using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; // Importa System.Collections para usar IEnumerator

public class MainMenu : MonoBehaviour
{
    // Este m�todo se puede vincular al evento OnClick del bot�n en el Inspector de Unity
    public void ChangeToGameScene()
    {
        // Carga la escena y sus datos de iluminaci�n
        StartCoroutine(LoadYourAsyncScene("GameScene"));
    }

    private IEnumerator LoadYourAsyncScene(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // Espera hasta que la escena est� completamente cargada
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Ajusta la iluminaci�n despu�s de cargar la escena
        LightProbes.Tetrahedralize();
        DynamicGI.UpdateEnvironment();
    }

    // Este m�todo se puede vincular al evento OnClick del bot�n en el Inspector de Unity
    public void QuitGame()
    {
        Application.Quit();

    }
}
