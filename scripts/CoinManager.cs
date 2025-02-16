using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance { get; private set; }
    private int coins = 0; // Contador de monedas
    public TextMeshProUGUI coinsText; // Referencia al TextMeshPro donde se mostrará el contador de monedas
    public TextMeshProUGUI secondaryCoinsText; // Referencia al segundo TextMeshPro donde se mostrará el contador de monedas

    private bool isDestroyed = false; // Flag para controlar si el CoinManager ha sido destruido

    void Awake()
    {
        // Verificar si ya hay una instancia y destruir esta si no es la "GameScene"
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Registrar eventos de carga de escena
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        if (coinsText == null)
        {
            Debug.LogError("CoinsText no está asignado en el inspector.");
            return;
        }

        UpdateCoinsText();  // Actualiza el texto de las monedas al inicio
    }

    public void IncreaseCoins(string enemyName)
    {
        if (isDestroyed) return; // Salir si el CoinManager ha sido destruido

        int randomCoins = 0;

        // Ajustar el número de monedas según el tipo de enemigo o su clon
        switch (enemyName)
        {
            case "Mutant":
            case "Mutant(Clone)":
                randomCoins = Random.Range(3, 7);
                break;
            case "Mutant2":
            case "Mutant2(Clone)":
                randomCoins = Random.Range(7, 11);
                break;
            case "Mutant3":
            case "Mutant3(Clone)":
                randomCoins = Random.Range(10, 15);
                break;
            case "Mutant4":
            case "Mutant4(Clone)":
                randomCoins = Random.Range(12, 17);
                break;
            case "Mutant5":
            case "Mutant5(Clone)":
                randomCoins = Random.Range(14, 19);
                break;
            case "Boss":
            case "Boss(Clone)":
                randomCoins = Random.Range(350, 400);
                break;
            case "Boss2":
            case "Boss2(Clone)":
                randomCoins = Random.Range(400, 600);
                break;
            case "Boss3":
            case "Boss3(Clone)":
                randomCoins = Random.Range(600, 1000);
                break;
            default:
                Debug.LogWarning("Enemy name not recognized.");
                return;
        }

        coins += randomCoins;  // Incrementa el contador de monedas según el número aleatorio generado
        UpdateCoinsText();  // Actualiza el texto de las monedas en la UI

        if (ShopManager.Instance != null)
        {
            ShopManager.Instance.CheckPurchaseable(); // Verifica si se pueden comprar items
        }
    }

    public void UpdateTotalCoinsText(TextMeshProUGUI totalCoinsText)
    {
        if (isDestroyed) return; // Salir si el CoinManager ha sido destruido

        totalCoinsText.text = "Total Coins: " + coins.ToString();
    }

    void UpdateCoinsText()
    {
        if (isDestroyed) return; // Salir si el CoinManager ha sido destruido

        string coinTextString = coins.ToString();
        coinsText.text = coinTextString;

        if (secondaryCoinsText != null)
        {
            secondaryCoinsText.text = coinTextString;
        }
    }

    public int GetCoins()
    {
        return coins;
    }

    public void SpendCoins(int amount)
    {
        if (isDestroyed) return; // Salir si el CoinManager ha sido destruido

        coins -= amount;
        UpdateCoinsText();
    }

    public void RegisterSecondaryText(TextMeshProUGUI text)
    {
        secondaryCoinsText = text;
        UpdateCoinsText();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (isDestroyed) return; // Salir si el CoinManager ha sido destruido

        if (scene.name != "GameScene")
        {
            Destroy(gameObject);
            isDestroyed = true;
        }
    }
}
