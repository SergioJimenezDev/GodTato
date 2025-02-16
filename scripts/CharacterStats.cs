using UnityEngine;

[System.Serializable]
public class CharacterStats
{
    public float maxHealth = 100f;
    public float currentHealth = 100f;
    public float armor = 0f;
    public float regenRate = 0.1f;

    public void TakeDamage(float damage)
    {
        float damageReduction = armor * 0.1f; // Reduce damage based on armor (example calculation)
        currentHealth -= (damage - damageReduction);
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
    }

    public void SetMaxHealth(float max)
    {
        maxHealth = max;
        currentHealth = maxHealth;
    }
}

public class CharacterStatsManager : MonoBehaviour
{
    public static CharacterStatsManager Instance;

    public CharacterStats playerStats;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Ensure only one instance exists
            return;
        }

        DontDestroyOnLoad(gameObject); // Keep the stats manager between scenes if needed
    }

    void Start()
    {
        // Initialize player stats or load from save data
        playerStats = new CharacterStats();
    }

    void Update()
    {
        // Example: Trigger health regeneration based on regenRate
        RegenerateHealth();
    }

    void RegenerateHealth()
    {
        playerStats.Heal(playerStats.regenRate * Time.deltaTime);
    }
}
