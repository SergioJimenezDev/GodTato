using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenu; // Referencia al men� de pausa
    public GameObject shopDef; // Referencia al objeto "ShopDef"
    private bool isPaused = false; // Flag para controlar si el juego est� pausado

    void Start()
    {
        // Desactivar el men� de pausa al inicio si est� asignado
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(false);
        }
        else
        {
            Debug.LogError("Pause menu not assigned in PauseManager");
        }

        // Comprobar si shopDef est� asignado
        if (shopDef == null)
        {
            Debug.LogError("ShopDef not assigned in PauseManager");
        }
    }

    void Update()
    {
        // Comprobar si shopDef est� asignado y activo
        if (shopDef != null && shopDef.activeSelf)
        {
            return; // No permitir pausar el juego si el objeto "ShopDef" est� activo
        }

        // Detectar la pulsaci�n de la tecla Escape para mostrar/ocultar el men� de pausa y pausar/reanudar el juego
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        if (!isPaused)
        {
            Time.timeScale = 0f; // Pausar el tiempo del juego solo si no estaba pausado antes
            isPaused = true;

            Debug.Log("Game Paused");

            // Mostrar el men� de pausa si est� asignado
            if (pauseMenu != null)
            {
                pauseMenu.SetActive(true);

                // Mostrar el cursor y desbloquearlo para interactuar con los botones
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Debug.LogError("Pause menu not assigned in PauseManager");
            }
        }
    }

    public void ResumeGame()
    {
        if (isPaused)
        {
            // Ocultar el men� de pausa si est� asignado
            if (pauseMenu != null)
            {
                pauseMenu.SetActive(false);
            }
            else
            {
                Debug.LogError("Pause menu not assigned in PauseManager");
            }

            // Reanudar el tiempo del juego solo si estaba pausado antes de presionar Escape
            if (isPaused)
            {
                Time.timeScale = 1f;
                Debug.Log("Game Resumed");
            }

            isPaused = false; // Marcar que el juego ya no est� pausado

            // Ocultar el cursor y bloquearlo de nuevo
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void LoadMainMenu()
    {
        // Reanudar el tiempo del juego antes de cambiar de escena
        Time.timeScale = 1f;
        Debug.Log("Loading Main Menu");
        SceneManager.LoadScene("MainMenu");
    }
}
