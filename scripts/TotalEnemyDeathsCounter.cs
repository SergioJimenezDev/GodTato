using UnityEngine;
using UnityEngine.UI;

public class TotalEnemyDeathsCounter : MonoBehaviour
{
    public static TotalEnemyDeathsCounter Instance;

    private int enemyDeathCount = 0;
    public Text enemyDeathCountText; // Referencia al texto en el UI donde se mostrará el contador

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("Duplicate TotalEnemyDeathsCounter instance found!");
            Destroy(gameObject);
        }
    }

    public void IncreaseEnemyDeathCount()
    {
        enemyDeathCount++;
        UpdateEnemyDeathCountUI();
    }

    private void UpdateEnemyDeathCountUI()
    {
        if (enemyDeathCountText != null)
        {
            enemyDeathCountText.text = "" + enemyDeathCount.ToString();
        }
        else
        {
            Debug.LogWarning("No UI Text assigned for enemy death count!");
        }
    }
}
