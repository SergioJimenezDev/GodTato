using TMPro;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifetime = 2f;
    public float velocidad = 10f; // Velocidad de la bala
    public GameObject damageTextPrefab; // Prefab para el texto de da�o
    public GameObject impactPrefab; // Prefab para la animaci�n de impacto
    private HealthSystem healthSystem;

    private void Start()
    {
        // Buscar el objeto HealthSystem
        healthSystem = GameObject.FindObjectOfType<HealthSystem>();

        // Guardar la rotaci�n inicial antes de aplicar la rotaci�n deseada
        Quaternion rotacionInicial = transform.rotation;

        // Aplicar rotaci�n de 90 grados en el eje X al inicio
        transform.rotation = Quaternion.Euler(90f, 0f, 0f);

        // Restaurar la rotaci�n inicial
        transform.rotation = rotacionInicial * transform.rotation;

        // Mover la bala en la direcci�n hacia adelante
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = transform.forward * velocidad;
        }
        else
        {
            Debug.LogWarning("El objeto de la bala no tiene Rigidbody adjunto.");
        }

        // Destruir la bala despu�s de cierto tiempo
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Acertado");
            EnemyHealthSystem enemyHealth = other.gameObject.GetComponent<EnemyHealthSystem>(); // Obtener el componente EnemyHealth
            if (enemyHealth != null && healthSystem != null)
            {
                float baseDamage = healthSystem.damage;

                // Utilizar la probabilidad de cr�tico del HealthSystem
                bool isCritical = Random.value < healthSystem.criticalChancePercentage / 100f;
                if (isCritical)
                {
                    baseDamage *= 1.5f;
                }

                enemyHealth.TakeDamage(baseDamage); // Infligir da�o al enemigo usando el valor de HealthSystem

                // Mostrar el texto de da�o
                ShowDamageText(baseDamage, enemyHealth.transform, isCritical);

                // Actualizar el da�o en la UI de HealthSystem
                healthSystem.UpdateUIStats();
            }

            // Mostrar la animaci�n de impacto
            ShowImpactAnimation(other.ClosestPointOnBounds(transform.position), other.transform);

            Destroy(gameObject); // Destruir la bala
        }
    }

    private void ShowDamageText(float damage, Transform enemyTransform, bool isCritical)
    {
        if (damageTextPrefab == null)
        {
            Debug.LogWarning("Damage Text Prefab no asignado.");
            return;
        }

        GameObject damageTextInstance = Instantiate(damageTextPrefab);
        CapsuleCollider capsuleCollider = enemyTransform.GetComponent<CapsuleCollider>();
        if (capsuleCollider != null)
        {
            Vector3 center = enemyTransform.TransformPoint(capsuleCollider.center);
            Vector3 damageTextPosition = center + Vector3.up * capsuleCollider.height * 0.5f;

            damageTextInstance.transform.position = damageTextPosition;

            TextMeshPro textMesh = damageTextInstance.GetComponent<TextMeshPro>();
            if (textMesh != null)
            {
                textMesh.text = "-" + damage.ToString();

                DamageText damageText = damageTextInstance.GetComponent<DamageText>();
                if (isCritical)
                {
                    textMesh.color = Color.red; // Cambiar a color rojo para da�o cr�tico
                    damageText.SetCriticalHit(); // Notificar al componente DamageText que este es un golpe cr�tico
                }

                Camera mainCamera = GameObject.Find("Camera").GetComponent<Camera>();
                if (mainCamera != null)
                {
                    Vector3 lookAtPosition = mainCamera.transform.position - damageTextInstance.transform.position;
                    lookAtPosition.y = 0;
                    damageTextInstance.transform.rotation = Quaternion.LookRotation(-lookAtPosition);
                }
                else
                {
                    Debug.LogWarning("No se encontr� una c�mara llamada 'Camera' para alinear el texto de da�o.");
                }

                Destroy(damageTextInstance, 2f);
            }
            else
            {
                Debug.LogWarning("El prefab DamageTextPrefab no tiene un componente TextMeshPro.");
            }
        }
        else
        {
            Debug.LogWarning("El enemigo no tiene un componente CapsuleCollider adjunto.");
        }
    }

    private void ShowImpactAnimation(Vector3 position, Transform enemyTransform)
    {
        if (impactPrefab != null)
        {
            // Ajustar la posici�n de impacto para que est� un poco por delante del enemigo
            Vector3 offsetPosition = enemyTransform.position + enemyTransform.forward * 1f; // Ajustar la distancia seg�n sea necesario
            GameObject impactInstance = Instantiate(impactPrefab, offsetPosition, Quaternion.identity);
            impactInstance.transform.SetParent(enemyTransform);
        }
    }
}
