using System.Collections.Generic;
using UnityEngine;
using TMPro; // Asegúrate de tener esta referencia para TextMeshPro
using UnityEngine.AI; // Asegúrate de tener esta referencia para NavMeshAgent

public class EnemySpawner : MonoBehaviour
{
    public List<GameObject> mutantPrefabs; // Lista de prefabs de enemigos ("Mutant", "Mutant2", etc.)
    public GameObject bossPrefab; // Prefab del primer jefe
    public GameObject bossPrefabRound15; // Prefab del segundo jefe para la ronda 15
    public GameObject bossPrefabRound20; // Prefab del tercer jefe para la ronda 20
    public List<Transform> spawnAreas; // Lista de áreas de spawn (cada Transform define un área)
    public float playerProximityThreshold = 10.0f; // Distancia mínima entre el jugador y el punto de spawn para desactivarlo

    public GameObject spawnEffectPrefab; // Prefab para el efecto de spawn

    private List<BoxCollider> spawnAreaColliders = new List<BoxCollider>();
    private float spawnInterval = 1.0f; // Intervalo de tiempo inicial entre cada aparición de enemigos
    private WaveManager waveManager; // Referencia al WaveManager
    private Transform player; // Referencia al jugador
    private bool bossSpawned = false; // Booleano para rastrear si el jefe ya ha sido instanciado
    private bool secondBossSpawned = false; // Booleano para rastrear si el segundo jefe de la ronda 15 ya ha sido instanciado
    private bool thirdBossSpawned = false; // Booleano para rastrear si el tercer jefe de la ronda 20 ya ha sido instanciado

    public TextMeshProUGUI enemyCounterText; // Referencia al TextMeshProUGUI para mostrar el número de enemigos

    private void Start()
    {
        // Obtener los Box Collider de cada área de spawn
        foreach (Transform area in spawnAreas)
        {
            BoxCollider collider = area.GetComponent<BoxCollider>();
            if (collider != null)
            {
                spawnAreaColliders.Add(collider);
            }
        }

        waveManager = WaveManager.Instance; // Obtener la instancia del WaveManager
        player = GameObject.FindGameObjectWithTag("Player").transform; // Obtener la referencia del jugador

        InvokeRepeating("SpawnEnemies", 0f, spawnInterval); // Llamar al método SpawnEnemies cada spawnInterval segundos
    }

    private void Update()
    {
        if (!waveManager.WaveRunning()) return;

        // Obtener la oleada actual desde el WaveManager
        int currentWave = waveManager.GetCurrentWave();

        // Ajustar el spawnInterval según la oleada
        SetSpawnIntervalByWave(currentWave);

        // Detectar si se presiona la tecla "P" para avanzar a la siguiente oleada
        if (Input.GetKeyDown(KeyCode.P))
        {
            waveManager.StartNextWave();
        }

        // Actualizar el contador de enemigos
        UpdateEnemyCounter();
    }

    private void SetSpawnIntervalByWave(int currentWave)
    {
        // Ajustar el spawnInterval según la oleada
        switch (currentWave)
        {
            case 1:
                SetSpawnInterval(1f);
                break;
            case 2:
                SetSpawnInterval(0.9f);
                break;
            case 3:
                SetSpawnInterval(0.85f);
                break;
            case 4:
                SetSpawnInterval(0.8f);
                break;
            case 5:
                SetSpawnInterval(0.75f);
                break;
            case 6:
                SetSpawnInterval(0.65f);
                break;
            case 7:
            case 8:
            case 9:
                SetSpawnInterval(0.65f);
                break;
            case 10:
                SetSpawnInterval(0.6f);
                break;
            case 11:
            case 12:
            case 13:
                SetSpawnInterval(0.55f);
                break;
            case 14:
                SetSpawnInterval(0.5f);
                break;
            case 15:
                SetSpawnInterval(0.45f);
                break;
            case 16:
                SetSpawnInterval(0.4f);
                break;
            case 17:
                SetSpawnInterval(0.3f);
                break;
            case 18:
                SetSpawnInterval(0.25f);
                break;
            case 19:
                SetSpawnInterval(0.2f);
                break;
            case 20:
                SetSpawnInterval(0.15f);
                break;
            default:
                SetSpawnInterval(0.1f); // Valor por defecto
                break;
        }
    }

    private void SetSpawnInterval(float interval)
    {
        if (spawnInterval != interval)
        {
            spawnInterval = interval;
            CancelInvoke("SpawnEnemies");
            InvokeRepeating("SpawnEnemies", 0f, spawnInterval);
            Debug.Log("SpawnInterval ajustado a: " + spawnInterval);
        }
    }

    private void SpawnEnemies()
    {
        // Obtener el número actual de enemigos en la escena
        int currentEnemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;

        // Dejar de spawnear si ya hay 375 o más enemigos
        if (currentEnemyCount >= 375)
        {
            // Eliminar un enemigo aleatorio para mantener el límite en 375
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            if (enemies.Length > 0)
            {
                int randomIndex = Random.Range(0, enemies.Length);
                Destroy(enemies[randomIndex]);
            }
            return;
        }

        int currentWave = waveManager.GetCurrentWave();
        int enemiesToSpawn = currentWave >= 11 ? 3 : 2; // A partir de la wave 11, spawnear 3 enemigos, sino 2

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            // Obtener un BoxCollider de spawn aleatorio que esté lejos del jugador
            BoxCollider chosenCollider = GetRandomSpawnAreaFarFromPlayer();
            if (chosenCollider == null) continue;

            // Lógica para determinar qué enemigos instanciar según la oleada actual
            GameObject enemyPrefab = GetEnemyPrefabForWave(currentWave);
            if (enemyPrefab != null)
            {
                Vector3 spawnPosition = GetRandomPositionWithinRandomArea(chosenCollider);
                GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

                // Mostrar el efecto de spawn
                if (spawnEffectPrefab != null)
                {
                    GameObject spawnEffect = Instantiate(spawnEffectPrefab, spawnPosition, Quaternion.identity);
                    Destroy(spawnEffect, 2f); // Destruir el efecto de spawn después de 2 segundos
                }

                // Ajustar la velocidad del nuevo enemigo si ya hay 1000 o más en la escena
                if (currentEnemyCount >= 1000)
                {
                    AdjustEnemySpeed(enemy, 20f);
                }
            }
        }

        // Spawnear un segundo jefe específico en la ronda 15 si no ha sido spawnado aún
        if (currentWave == 15 && !secondBossSpawned)
        {
            secondBossSpawned = true;
            BoxCollider chosenCollider = GetRandomSpawnAreaFarFromPlayer();
            if (chosenCollider != null)
            {
                Vector3 spawnPosition = GetRandomPositionWithinRandomArea(chosenCollider);
                GameObject boss = Instantiate(bossPrefabRound15, spawnPosition, Quaternion.identity);

                // Mostrar el efecto de spawn
                if (spawnEffectPrefab != null)
                {
                    GameObject spawnEffect = Instantiate(spawnEffectPrefab, spawnPosition, Quaternion.identity);
                    Destroy(spawnEffect, 2f); // Destruir el efecto de spawn después de 2 segundos
                }
            }
        }

        // Spawnear un tercer jefe específico en la ronda 20 si no ha sido spawnado aún
        if (currentWave == 20 && !thirdBossSpawned)
        {
            thirdBossSpawned = true;
            BoxCollider chosenCollider = GetRandomSpawnAreaFarFromPlayer();
            if (chosenCollider != null)
            {
                Vector3 spawnPosition = GetRandomPositionWithinRandomArea(chosenCollider);
                GameObject boss = Instantiate(bossPrefabRound20, spawnPosition, Quaternion.identity);

                // Mostrar el efecto de spawn
                if (spawnEffectPrefab != null)
                {
                    GameObject spawnEffect = Instantiate(spawnEffectPrefab, spawnPosition, Quaternion.identity);
                    Destroy(spawnEffect, 2f); // Destruir el efecto de spawn después de 2 segundos
                }
            }
        }
    }


    private void AdjustEnemySpeed(GameObject enemy, float newSpeed)
    {
        NavMeshAgent navMeshAgent = enemy.GetComponent<NavMeshAgent>();
        if (navMeshAgent != null)
        {
            navMeshAgent.speed = newSpeed;
        }
        else
        {
            Debug.LogWarning("NavMeshAgent no encontrado en el objeto: " + enemy.name);
        }
    }

    private BoxCollider GetRandomSpawnAreaFarFromPlayer()
    {
        List<BoxCollider> availableColliders = new List<BoxCollider>();
        foreach (BoxCollider collider in spawnAreaColliders)
        {
            if (Vector3.Distance(player.position, collider.bounds.center) > playerProximityThreshold)
            {
                availableColliders.Add(collider);
            }
        }
        if (availableColliders.Count == 0)
            return null;

        return availableColliders[Random.Range(0, availableColliders.Count)];
    }

    private GameObject GetEnemyPrefabForWave(int currentWave)
    {
        if (currentWave == 10 && !bossSpawned)
        {
            bossSpawned = true;
            return bossPrefab;
        }

        if (currentWave == 15 && !secondBossSpawned)
        {
            secondBossSpawned = true;
            return bossPrefabRound15;
        }

        if (currentWave == 20 && !thirdBossSpawned)
        {
            thirdBossSpawned = true;
            return bossPrefabRound20;
        }

        if (currentWave >= 4 && currentWave <= 99) // Incluir todas las oleadas de la 4 a la 20
        {
            if (currentWave >= 18) // A partir de la oleada 16
            {
                float randomValue = Random.value;

                if (randomValue < 0.50f)
                {
                    return mutantPrefabs.Find(p => p.name == "Mutant4");
                }
                else
                {
                    return mutantPrefabs.Find(p => p.name == "Mutant5");
                }
            }
            else if (currentWave >= 16) // A partir de la oleada 16
            {
                float randomValue = Random.value;

                if (randomValue < 0.20f)
                {
                    return mutantPrefabs.Find(p => p.name == "Mutant3");
                }
                else if (randomValue < 0.80f)
                {
                    return mutantPrefabs.Find(p => p.name == "Mutant4");
                }
                else
                {
                    return mutantPrefabs.Find(p => p.name == "Mutant5");
                }
            }
            else if (currentWave >= 12) // A partir de la oleada 12
            {
                float randomValue = Random.value;
                if (randomValue < 0.15f)
                {
                    return mutantPrefabs.Find(p => p.name == "Mutant");
                }
                else if (randomValue < 0.40f)
                {
                    return mutantPrefabs.Find(p => p.name == "Mutant2");
                }
                else if (randomValue < 0.70f)
                {
                    return mutantPrefabs.Find(p => p.name == "Mutant3");
                }
                else
                {
                    return mutantPrefabs.Find(p => p.name == "Mutant4");
                }
            }
            else if (currentWave >= 8) // Para oleadas 8 a 11
            {
                float randomValue = Random.value;
                if (randomValue < 0.33f)
                {
                    return mutantPrefabs.Find(p => p.name == "Mutant");
                }
                else if (randomValue < 0.67f)
                {
                    return mutantPrefabs.Find(p => p.name == "Mutant2");
                }
                else
                {
                    return mutantPrefabs.Find(p => p.name == "Mutant3");
                }
            }
            else // Para oleadas 4 a 7
            {
                if (Random.value < 0.5f)
                {
                    return mutantPrefabs.Find(p => p.name == "Mutant");
                }
                else
                {
                    return mutantPrefabs.Find(p => p.name == "Mutant2");
                }
            }
        }
        else // Para oleadas 1, 2 y 3
        {
            return mutantPrefabs.Find(p => p.name == "Mutant");
        }
    }

    private Vector3 GetRandomPositionWithinRandomArea(BoxCollider collider)
    {
        Vector3 center = collider.bounds.center;
        Vector3 size = collider.bounds.size;

        float x = center.x + Random.Range(-size.x / 2, size.x / 2);
        float y = center.y; // Asumimos que la posición en Y es la misma que el centro del Box Collider
        float z = center.z + Random.Range(-size.z / 2, size.z / 2);

        return new Vector3(x, y, z);
    }

    private void UpdateEnemyCounter()
    {
        int enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        enemyCounterText.text = "" + enemyCount;
    }
}
