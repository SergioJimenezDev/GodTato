using UnityEngine;

public class EnemyDeath : MonoBehaviour
{
    private int hitCount = 0;
    public int maxHits = 2; // Número máximo de impactos antes de destruirse
    private bool isDead = false;
    public ParticleSystem deathParticles;

    private TotalEnemyDeathsCounter deathsCounter; // Referencia al contador de muertes totales

    void Start()
    {
        deathsCounter = FindObjectOfType<TotalEnemyDeathsCounter>();
        if (deathsCounter == null)
        {
            Debug.LogError("TotalEnemyDeathsCounter script not found in the scene!");
        }
    }

    public void TakeHit()
    {
        if (isDead) return;

        hitCount++;
        if (hitCount >= maxHits)
        {
            EnemyDeaths(); // Llamar al método de muerte
        }
    }

    private void EnemyDeaths()
    {
        Instantiate(deathParticles, transform.position, Quaternion.identity);
        Destroy(gameObject);

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
