using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class HealthSystem : MonoBehaviour
{
    public static HealthSystem Instance;

    public Image currentHealthBar;
    public float hitPoint = 100f;
    public float maxHitPoint = 100f;

    // Armor
    public float armor = 0f; // Initialize armor to 0

    // Damage inflicted by bullets
    public float damage = 40f; // Daño infligido por la bala

    // UI Text references
    public TextMeshProUGUI armorText; // Referencia al UI Text para la armadura
    public TextMeshProUGUI maxHealthText; // Referencia al UI Text para la salud máxima
    public TextMeshProUGUI fireRateText; // Referencia al UI Text para el normalFireRate
    public TextMeshProUGUI damageText; // Referencia al UI Text para el daño infligido
    public TextMeshProUGUI criticalChanceText; // Referencia al UI Text para la probabilidad de crítico
    public TextMeshProUGUI speedText; // Referencia al UI Text para la velocidad

    // Probabilidad de crítico (ejemplo: 10%)
    public float criticalChancePercentage = 10f;

    //==============================================================
    // Regenerate Health
    //==============================================================
    public bool Regenerate = true;
    public float regen = 0.1f;
    private float timeleft = 0.0f;  // Left time for current interval
    public float regenUpdateInterval = 1f;

    public bool GodMode;

    // Referencia al PlayerMovement para acceder a la velocidad
    public PlayerMovement playerMovement;

    // GameObject para GameOver
    public GameObject gameOverScreen;

    //==============================================================
    // Awake
    //==============================================================
    void Awake()
    {
        Instance = this;
    }

    //==============================================================
    // Start
    //==============================================================
    void Start()
    {
        // Intenta encontrar los textos incluso si PlayerStats está desactivado al inicio
        if (armorText == null)
        {
            armorText = GameObject.Find("Armor")?.GetComponent<TextMeshProUGUI>();
        }
        if (maxHealthText == null)
        {
            maxHealthText = GameObject.Find("MaxHealth")?.GetComponent<TextMeshProUGUI>();
        }
        if (fireRateText == null)
        {
            fireRateText = GameObject.Find("FireRate")?.GetComponent<TextMeshProUGUI>();
        }
        if (damageText == null)
        {
            damageText = GameObject.Find("Damage")?.GetComponent<TextMeshProUGUI>();
        }
        if (criticalChanceText == null)
        {
            criticalChanceText = GameObject.Find("CriticalChance")?.GetComponent<TextMeshProUGUI>();
        }
        if (speedText == null)
        {
            speedText = GameObject.Find("Speed")?.GetComponent<TextMeshProUGUI>();
        }

        UpdateGraphics();
        UpdateUIStats(); // Actualiza las estadísticas en la UI al inicio
    }

    //==============================================================
    // Update
    //==============================================================
    void Update()
    {
        if (Regenerate)
            Regen();

        // Actualiza la velocidad del personaje
        UpdateSpeed();
    }

    //==============================================================
    // Regenerate Health
    //==============================================================
    private void Regen()
    {
        timeleft -= Time.deltaTime;

        if (timeleft <= 0.0) // Interval ended - update health and start new interval
        {
            // Debug mode
            if (GodMode)
            {
                HealDamage(maxHitPoint);
            }
            else
            {
                HealDamage(regen);
            }

            UpdateGraphics();
            timeleft = regenUpdateInterval;
        }
    }

    //==============================================================
    // Health Logic
    //==============================================================
    private void UpdateHealthBar()
    {
        float ratio = hitPoint / maxHitPoint;
        currentHealthBar.rectTransform.localPosition = new Vector3(currentHealthBar.rectTransform.rect.width * ratio - currentHealthBar.rectTransform.rect.width, 0, 0);
    }

    public void TakeDamage(float damage)
    {
        float damageTaken = damage; // Guardar el daño original para mostrar en la consola

        // Calcular el porcentaje de reducción de daño basado en la armadura
        float damageReductionPercentage = armor * 0.0125f; // Convertir la armadura a un porcentaje (por ejemplo, 50 armadura = 50% de reducción de daño)

        // Aplicar la reducción de daño basado en la armadura
        float reducedDamage = damageTaken * (1f - damageReductionPercentage);

        // Asegurarse de que el daño reducido no sea nulo (como mínimo, se recibe el 1% del daño original)
        reducedDamage = Mathf.Max(reducedDamage, damageTaken * 0.4f);

        // Mostrar en consola la cantidad de vida que se ha quitado al jugador
        Debug.Log("Vida perdida: " + reducedDamage);

        // Aplicar el daño reducido
        hitPoint -= reducedDamage;

        if (hitPoint < 1)
        {
            hitPoint = 0;
            HandlePlayerDeath();
        }

        UpdateGraphics(); // Actualizar la interfaz gráfica de salud
        StartCoroutine(PlayerHurts()); // Mostrar la animación de daño
    }

    private void HandlePlayerDeath()
    {
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(true); // Hacer visible el objeto GameOver
            Time.timeScale = 0f; // Pausar el tiempo
            Cursor.lockState = CursorLockMode.None; // Desbloquear el cursor
            Cursor.visible = true; // Mostrar el cursor
        }
        else
        {
            Debug.LogError("GameOver screen not assigned in HealthSystem.");
        }
    }

    public void HealDamage(float heal)
    {
        hitPoint += heal;
        if (hitPoint > maxHitPoint)
            hitPoint = maxHitPoint;

        UpdateGraphics(); // Actualizar la interfaz gráfica de salud
    }

    public void IncreaseMaxHealth(float amount)
    {
        maxHitPoint += amount;
        hitPoint += amount;

        UpdateGraphics(); // Actualizar la interfaz gráfica de salud
        UpdateUIStats(); // Actualizar las estadísticas en la interfaz de usuario
    }

    //==============================================================
    // Update all Health UI graphics
    //==============================================================
    public void UpdateGraphics()
    {
        UpdateHealthBar(); // Actualizar la barra de salud
        UpdateUIStats(); // Actualizar las estadísticas en la interfaz de usuario
    }

    //==============================================================
    // Coroutine Player Hurts
    //==============================================================
    IEnumerator PlayerHurts()
    {
        if (hitPoint < 1) // Health is Zero!!
        {
            yield return StartCoroutine(PlayerDied()); // Hero is Dead
        }
        else
        {
            yield return null;
        }
    }

    //==============================================================
    // Hero is dead
    //==============================================================
    IEnumerator PlayerDied()
    {
        HandlePlayerDeath();
        yield break; // Indicamos explícitamente que no hay más operaciones en esta coroutine
    }

    //==============================================================
    // Update UI Stats
    //==============================================================
    public void UpdateUIStats()
    {
        // Actualiza los valores de los Text UI con las estadísticas actuales del jugador
        if (armorText != null)
        {
            armorText.text = "" + armor.ToString(); // Actualiza el texto de la armadura
        }
        if (maxHealthText != null)
        {
            maxHealthText.text = "" + maxHitPoint.ToString(); // Actualiza el texto de la salud máxima
        }
        if (fireRateText != null)
        {
            fireRateText.text = "" + Gun.GetNormalFireRate().ToString("F1"); // Actualiza el texto del fire rate
        }
        if (damageText != null)
        {
            damageText.text = "" + damage.ToString(); // Actualiza el texto del daño
        }
        if (criticalChanceText != null)
        {
            criticalChanceText.text = criticalChancePercentage.ToString() + "%"; // Mostrar la probabilidad de crítico en el texto
        }
        if (speedText != null)
        {
            speedText.text = "" + playerMovement.speed.ToString(); // Mostrar la velocidad del personaje
        }
    }

    //==============================================================
    // Aumentar el daño
    //==============================================================
    public void IncreaseDamage(float bonus)
    {
        damage += bonus;
        UpdateGraphics(); // Asegúrate de llamar a UpdateGraphics si actualizas visualmente algo más
        UpdateUIStats(); // Llama a UpdateUIStats al final para actualizar los textos de UI
    }

    //==============================================================
    // Aumentar la armadura
    //==============================================================
    public void IncreaseArmor(float bonus)
    {
        armor += bonus;
        UpdateGraphics(); // Asegúrate de llamar a UpdateGraphics si actualizas visualmente algo más
        UpdateUIStats(); // Llama a UpdateUIStats al final para actualizar los textos de UI
    }

    //==============================================================
    // Actualizar la velocidad del personaje
    //==============================================================
    private void UpdateSpeed()
    {
        if (playerMovement != null && speedText != null)
        {
            speedText.text = "" + playerMovement.speed.ToString(); // Actualiza el texto de la velocidad
        }
    }
}
