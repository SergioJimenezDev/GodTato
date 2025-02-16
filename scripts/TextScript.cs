using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour
{
    public float moveSpeed = 1f;        // Velocidad de movimiento hacia arriba
    public float fadeDuration = 1f;     // Duración del desvanecimiento
    private TextMeshPro textMesh;
    private Color originalColor;
    private float startTime;
    private bool isCriticalHit = false; // Indica si el golpe fue crítico

    void Start()
    {
        textMesh = GetComponent<TextMeshPro>();
        originalColor = textMesh.color;
        startTime = Time.time; // Guardar el tiempo de inicio

        // Ajustar propiedades si es un golpe crítico
        if (isCriticalHit)
        {
            textMesh.fontSize = 15;
            moveSpeed = 3f;
            fadeDuration = 2f;
        }
    }

    void Update()
    {
        // Mover el texto hacia arriba
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;

        // Calcular el tiempo transcurrido desde el inicio
        float timeElapsed = Time.time - startTime;

        // Calcular el alpha para el desvanecimiento usando una curva cuadrática
        float alpha = Mathf.Lerp(originalColor.a, 0, timeElapsed / fadeDuration);

        // Aplicar el color con el nuevo alpha al texto
        textMesh.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

        // Destruir el objeto una vez completado el desvanecimiento
        if (timeElapsed >= fadeDuration)
        {
            Destroy(gameObject);
        }
    }

    // Método para establecer si es un golpe crítico
    public void SetCriticalHit()
    {
        isCriticalHit = true;
    }
}
