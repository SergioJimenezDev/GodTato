using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMainMenu : MonoBehaviour
{
    // Método público que será llamado por el botón para cargar la escena MainMenu
    public void LoadMainMenuScene()
    {
        // Reanudar el tiempo si estaba pausado
        Time.timeScale = 1f;

        // Cargar la escena MainMenu
        SceneManager.LoadScene("MainMenu");
    }
}
