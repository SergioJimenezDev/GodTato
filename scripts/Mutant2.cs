using UnityEngine;
using TMPro;

public class Mutant2 : MonoBehaviour
{
    public float maxHitPoint = 100f; // Vida máxima del enemigo
    public float currentHitPoint; // Vida actual del enemigo
    public ParticleSystem deathParticles;
    public GameObject damageTextPrefab; // Referencia al prefab del texto de daño

    void Start()
    {
        currentHitPoint = maxHitPoint; // Establecer la vida actual al máximo al inicio
    }

    public void TakeDamage(float damage)
    {
        currentHitPoint -= damage; // Reducir la vida en función del daño recibido
        Debug.Log("Vida actual del enemigo: " + currentHitPoint); // Mostrar log de la vida actual del enemigo

        // Mostrar el texto de daño
        ShowDamageText(damage);

        if (currentHitPoint < 1) // Verificar si el enemigo está muerto
        {
            EnemyDeaths();
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

    private void ShowDamageText(float damage)
    {
        if (damageTextPrefab != null)
        {
            // Obtener el centro del collider del enemigo
            Collider enemyCollider = GetComponent<Collider>(); // Asegúrate de tener un collider en el enemigo
            if (enemyCollider != null)
            {
                Vector3 damageTextPosition = enemyCollider.bounds.center;
                damageTextPosition.y += enemyCollider.bounds.extents.y + 0.5f; // Ajusta el offset vertical según tu preferencia

                GameObject damageTextInstance = Instantiate(damageTextPrefab, damageTextPosition, Quaternion.identity);
                TextMeshPro textMesh = damageTextInstance.GetComponent<TextMeshPro>();
                if (textMesh != null)
                {
                    textMesh.text = "-" + damage.ToString(); // Agregar el guion "-" antes del texto de daño

                    // Buscar la cámara llamada "Camera" en la escena
                    Camera mainCamera = GameObject.Find("Camera").GetComponent<Camera>();
                    if (mainCamera != null)
                    {
                        // Alinear el texto hacia la cámara
                        damageTextInstance.transform.LookAt(mainCamera.transform);

                        // Invertir la escala en el eje X para simular un modo espejo
                        Vector3 localScale = damageTextInstance.transform.localScale;
                        localScale.x *= -1f;
                        damageTextInstance.transform.localScale = localScale;
                    }
                    else
                    {
                        Debug.LogWarning("No se pudo encontrar una cámara llamada 'Camera' para alinear el texto de daño.");
                    }

                    Destroy(damageTextInstance, 2f); // Destruir el texto de daño después de 2 segundos
                }
                else
                {
                    Debug.LogWarning("El prefab DamageTextPrefab no tiene un componente TextMeshPro.");
                }
            }
            else
            {
                Debug.LogWarning("El enemigo no tiene un collider adjunto.");
            }
        }
        else
        {
            Debug.LogWarning("Damage Text Prefab no asignado.");
        }
    }
}
