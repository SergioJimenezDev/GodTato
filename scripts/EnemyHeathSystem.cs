using UnityEngine;

public class EnemyHealthSystem : MonoBehaviour
{
    public float maxHitPoint = 100f;
    public GameObject deathParticlesPrefab; // Prefab de partículas de muerte
    private float currentHitPoint;

    void Start()
    {
        // Configuración inicial de la vida según el nombre del prefab
        string enemyName = gameObject.name;
        if (enemyName.Contains("Mutant2"))
        {
            maxHitPoint = 200f;
        }
        if (enemyName.Contains("Mutant3"))
        {
            maxHitPoint = 500f;
        }
        if (enemyName.Contains("Mutant4"))
        {
            maxHitPoint = 1000f;
        }
        if (enemyName.Contains("Mutant5"))
        {
            maxHitPoint = 1500f;
        }
        if (enemyName.Contains("Boss"))
        {
            maxHitPoint = 25000f;
        }
        if (enemyName.Contains("Boss2"))
        {
            maxHitPoint = 75000f;
        }
        if (enemyName.Contains("Boss3"))
        {
            maxHitPoint = 100000f;
        }

        currentHitPoint = maxHitPoint;
    }

    public void TakeDamage(float damage)
    {
        currentHitPoint -= damage;
        Debug.Log("Vida actual del enemigo: " + currentHitPoint);

        if (currentHitPoint <= 0)
        {
            EnemyDeaths();
        }
    }

    private void EnemyDeaths()
    {
        // Instanciar las partículas de muerte
        GameObject deathParticles = Instantiate(deathParticlesPrefab, transform.position, Quaternion.identity);

        // Destruir las partículas después de 2 segundos
        Destroy(deathParticles, 2f);

        // Destruir el gameObject del enemigo
        Destroy(gameObject);

        // Incrementar contador de enemigos muertos usando TotalEnemyDeathsCounter.Instance
        if (TotalEnemyDeathsCounter.Instance != null)
        {
            TotalEnemyDeathsCounter.Instance.IncreaseEnemyDeathCount();
        }
        else
        {
            Debug.LogError("TotalEnemyDeathsCounter instance not found!");
        }

        // Incrementar monedas si CoinManager.Instance existe
        if (CoinManager.Instance != null)
        {
            CoinManager.Instance.IncreaseCoins(gameObject.name); // Pasar el nombre del enemigo a IncreaseCoins
        }
        else
        {
            Debug.LogError("CoinManager instance not found!");
        }
    }
}
