using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMainMenu : MonoBehaviour
{
    // M�todo p�blico que ser� llamado por el bot�n para cargar la escena MainMenu
    public void LoadMainMenuScene()
    {
        // Reanudar el tiempo si estaba pausado
        Time.timeScale = 1f;

        // Cargar la escena MainMenu
        SceneManager.LoadScene("MainMenu");
    }
}
