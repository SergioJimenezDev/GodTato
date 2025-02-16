using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] TextMeshProUGUI waveText;

    public static WaveManager Instance;

    public event Action OnWaveStart;

    bool waveRunning = true;
    int currentWave = 0;
    int currentWaveTime;

    private Gun playerGun;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        // Encuentra el objeto Gun en la escena
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerGun = player.GetComponentInChildren<Gun>();
            if (playerGun == null)
            {
                Debug.LogError("Player gun component not found!");
            }
        }
        else
        {
            Debug.LogError("Player not found!");
        }
    }

    private void Start()
    {
        StartNewWave();
    }

    public bool WaveRunning() => waveRunning;

    public int GetCurrentWave() => currentWave;

    public int GetCurrentWaveTime() => currentWaveTime;

    private void StartNewWave()
    {
        StopAllCoroutines();
        currentWave++;
        waveRunning = true;

        // Ajustar el tiempo de la ola basado en el número de la ola
        if (currentWave == 10 || currentWave == 15 || currentWave == 20)
        {
            currentWaveTime = 60;
        }
        else
        {
            currentWaveTime = 45;
        }

        // Eliminar "Block" si estamos en la oleada número 2
        if (currentWave == 3)
        {
            string[] blockNames = { "Tier2", "Tier2 (1)", "Tier2 (2)", "Tier2 (3)" };
            foreach (string blockName in blockNames)
            {
                GameObject block = FindInactiveObjectByName(blockName);
                if (block != null)
                {
                    UnityEngine.Object.Destroy(block);
                }
                else
                {
                    Debug.LogError($"{blockName} not found!");
                }
            }
        }
        else if (currentWave == 7)
        {
            string[] blockNames = { "Tier3", "Tier3 (1)", "Tier3 (2)", "Tier3 (3)" };
            foreach (string blockName in blockNames)
            {
                GameObject block = FindInactiveObjectByName(blockName);
                if (block != null)
                {
                    UnityEngine.Object.Destroy(block);
                }
                else
                {
                    Debug.LogError($"{blockName} not found!");
                }
            }
        }
        else if (currentWave == 12)
        {
            string[] blockNames = { "Tier4", "Tier4 (1)", "Tier4 (2)", "Tier4 (3)" };
            foreach (string blockName in blockNames)
            {
                GameObject block = FindInactiveObjectByName(blockName);
                if (block != null)
                {
                    UnityEngine.Object.Destroy(block);
                }
                else
                {
                    Debug.LogError($"{blockName} not found!");
                }
            }
        }
        else if (currentWave == 16)
        {
            string[] blockNames = { "Tier5", "Tier5 (1)", "Tier5 (2)", "Tier5 (3)" };
            foreach (string blockName in blockNames)
            {
                GameObject block = FindInactiveObjectByName(blockName);
                if (block != null)
                {
                    UnityEngine.Object.Destroy(block);
                }
                else
                {
                    Debug.LogError($"{blockName} not found!");
                }
            }
        }

        UpdateUI();
        StartCoroutine(WaveTimer());
        TeleportPlayer();

        // Llamar al evento de inicio de la wave
        OnWaveStart?.Invoke();

        // Reiniciar la habilidad de "R" en el arma del jugador
        if (playerGun != null)
        {
            playerGun.ResetRAbility();
        }
        else
        {
            Debug.LogError("Player gun not found!");
        }
    }

    IEnumerator WaveTimer()
    {
        while (waveRunning)
        {
            yield return new WaitForSeconds(1f);
            currentWaveTime--;

            timeText.text = currentWaveTime.ToString();

            if (currentWaveTime < 0)
                WaveComplete();
        }
    }

    private void WaveComplete()
    {
        waveRunning = false;
        StopAllCoroutines();

        // Encuentra todos los GameObjects en la escena
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();

        // Recorre todos los objetos encontrados
        foreach (GameObject obj in allObjects)
        {
            // Verifica si el nombre del objeto contiene "Mutant", "Mutant2", "Mutant3", "Mutant4"
            if (obj.name.Contains("Mutant") || obj.name.Contains("Mutant2") || obj.name.Contains("Mutant3") || obj.name.Contains("Mutant4") || obj.name.Contains("Boss") || obj.name.Contains("Boss2") || obj.name.Contains("Boss3") || obj.name.Contains("Heart") || obj.name.Contains("Heart(Clone)"))
            {
                // Destruye el objeto
                UnityEngine.Object.Destroy(obj);
            }
        }

        // Recuperar toda la vida del jugador
        if (HealthSystem.Instance != null)
        {
            HealthSystem.Instance.HealDamage(HealthSystem.Instance.maxHitPoint);
        }
        else
        {
            Debug.LogError("HealthSystem instance not found!");
        }

        // Actualiza el número de la oleada
        currentWave--;
        currentWave--;

        // Actualiza el texto de la oleada
        UpdateUI();

        // Pausa el juego utilizando el GameManager
        if (GameManager.Instance != null)
        {
            GameManager.Instance.PauseGame();
        }
        else
        {
            Debug.LogError("GameManager instance not found!");
        }

        // Reiniciar la siguiente wave
        StartNewWave();
    }

    private void UpdateUI()
    {
        timeText.text = currentWaveTime.ToString();
        waveText.text = currentWave.ToString();
    }

    private void TeleportPlayer()
    {
        // Teleportar al jugador al inicio de la wave
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.position = new Vector3(158f, 20.6599998f, 275.600006f);
        }
        else
        {
            Debug.LogError("Player not found!");
        }
    }

    public void StartNextWave()
    {
        StartNewWave();
    }

    private GameObject FindInactiveObjectByName(string name)
    {
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (obj.name == name)
            {
                return obj;
            }
        }
        return null;
    }
}
