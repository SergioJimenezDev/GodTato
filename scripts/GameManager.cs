using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject shopDef; // Referencia al objeto ShopDef
    public GameObject noShop; // Referencia al objeto NoShop
    public Button continueButton; // Referencia al botón Continue

    private bool isDestroyed = false; // Flag para controlar si el GameManager ha sido destruido

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        if (continueButton != null)
        {
            continueButton.onClick.AddListener(OnContinueButtonPressed);
        }
        else
        {
            Debug.LogError("Continue button not assigned in GameManager");
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (isDestroyed) return; // Salir si el GameManager ha sido destruido

        if (scene.name != "GameScene")
        {
            Destroy(gameObject);
            isDestroyed = true;
        }
    }

    public void PauseGame()
    {
        if (isDestroyed) return;

        Time.timeScale = 0f;
        Debug.Log("Game Paused");
        ShowShop();
    }

    public void ResumeGame()
    {
        if (isDestroyed) return;

        Time.timeScale = 1f;
        Debug.Log("Game Resumed");
        HideShop();
        if (WaveManager.Instance != null)
        {
            WaveManager.Instance.StartNextWave();
        }
        else
        {
            Debug.LogError("WaveManager instance not found!");
        }
    }

    private void ShowShop()
    {
        if (isDestroyed) return;

        if (shopDef != null)
        {
            shopDef.SetActive(true);
            Cursor.visible = true; // Muestra el cursor
            Cursor.lockState = CursorLockMode.None; // Permite mover el cursor
        }
        else
        {
            Debug.LogError("ShopDef not assigned in GameManager");
        }

        if (noShop != null)
        {
            noShop.SetActive(false);
        }
        else
        {
            Debug.LogError("NoShop not assigned in GameManager");
        }
    }

    private void HideShop()
    {
        if (isDestroyed) return;

        if (shopDef != null)
        {
            shopDef.SetActive(false);
            Cursor.visible = false; // Oculta el cursor
            Cursor.lockState = CursorLockMode.Locked; // Bloquea el cursor en el centro de la pantalla
        }
        else
        {
            Debug.LogError("ShopDef not assigned in GameManager");
        }

        if (noShop != null)
        {
            noShop.SetActive(true);
        }
        else
        {
            Debug.LogError("NoShop not assigned in GameManager");
        }
    }

    private void OnContinueButtonPressed()
    {
        if (isDestroyed) return;

        ResumeGame();
    }
}
