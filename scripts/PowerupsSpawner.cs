using UnityEngine;
using System.Collections.Generic;

public class PowerupsSpawner : MonoBehaviour
{
    [System.Serializable]
    public struct PowerupPrefabInfo
    {
        public GameObject prefab;
        [Range(0, 1)]
        public float spawnProbability;
    }

    [System.Serializable]
    public struct SpawnPointInfo
    {
        public Transform spawnPoint;
    }

    public List<PowerupPrefabInfo> powerupPrefabs = new List<PowerupPrefabInfo>();
    public List<SpawnPointInfo> spawnPoints = new List<SpawnPointInfo>();

    // Referencia al WaveManager
    public WaveManager waveManager;

    // Intervalo aleatorio para el spawn de powerup
    private float minSpawnTime = 10f;
    private float maxSpawnTime = 35f;

    private void Start()
    {
        if (powerupPrefabs.Count == 0 || spawnPoints.Count == 0)
        {
            Debug.LogWarning("No powerup prefabs or spawn points assigned.");
            return;
        }

        // Obtener referencia al WaveManager si no se asignó manualmente
        if (waveManager == null)
        {
            waveManager = WaveManager.Instance;
        }

        // Suscribirse al evento de inicio de nueva wave
        waveManager.OnWaveStart += StartNewWave;
    }

    private void StartNewWave()
    {
        // Cancelar cualquier invocación de spawn anterior
        CancelInvoke("SpawnRandomPowerup");

        // Determinar el tiempo aleatorio de spawn dentro del intervalo
        float spawnTime = Random.Range(minSpawnTime, maxSpawnTime);

        // Invocar el spawn después de un tiempo aleatorio dentro del intervalo
        Invoke("SpawnRandomPowerup", spawnTime);
    }

    private void SpawnRandomPowerup()
    {
        // Elegir un prefab aleatorio basado en las probabilidades de spawn
        GameObject prefabToSpawn = ChooseRandomPrefab();

        // Elegir un punto de spawn aleatorio
        Transform spawnPoint = ChooseRandomSpawnPoint();

        if (prefabToSpawn != null && spawnPoint != null)
        {
            Instantiate(prefabToSpawn, spawnPoint.position, spawnPoint.rotation);
        }
    }

    private GameObject ChooseRandomPrefab()
    {
        float totalProbability = 0f;
        foreach (var prefabInfo in powerupPrefabs)
        {
            totalProbability += prefabInfo.spawnProbability;
        }

        float randomValue = Random.value * totalProbability;

        foreach (var prefabInfo in powerupPrefabs)
        {
            if (randomValue < prefabInfo.spawnProbability)
            {
                return prefabInfo.prefab;
            }
            randomValue -= prefabInfo.spawnProbability;
        }

        // Fallback (should never happen with correct setup)
        return powerupPrefabs[0].prefab;
    }

    private Transform ChooseRandomSpawnPoint()
    {
        int randomIndex = Random.Range(0, spawnPoints.Count);
        return spawnPoints[randomIndex].spawnPoint;
    }
}
