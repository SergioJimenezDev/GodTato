using UnityEngine;

public class FallingAndBouncing : MonoBehaviour
{
    public float initialHeight = 7f; // Altura inicial desde la que cae el objeto (ajustada para empezar más abajo)
    public float gravity = -50f; // Aceleración de la gravedad (aumentada para caer más rápido)
    public float bounceDamping = 0.5f; // Factor de amortiguación del rebote (disminuido para rebotes más altos)
    public int maxBounces = 3; // Número máximo de rebotes

    private Vector2 velocity;
    private int bounceCount = 0;

    void Start()
    {
        // Establecer la posición inicial del objeto en la altura especificada
        transform.localPosition = new Vector2(transform.localPosition.x, initialHeight);
        // Inicializar la velocidad del objeto
        velocity = Vector2.zero;
    }

    void Update()
    {
        if (bounceCount < maxBounces)
        {
            // Aplicar la gravedad a la velocidad del objeto
            velocity += new Vector2(0, gravity * Time.deltaTime);

            // Actualizar la posición del objeto
            transform.localPosition += (Vector3)(velocity * Time.deltaTime);

            // Detectar si el objeto ha tocado el límite inferior (y < 5)
            if (transform.localPosition.y < 5)
            {
                // Asegurarse de que el objeto no quede por debajo del límite inferior
                transform.localPosition = new Vector3(transform.localPosition.x, 5, transform.localPosition.z);

                // Invertir la velocidad en el eje y para simular el rebote
                velocity.y = -velocity.y * bounceDamping;

                bounceCount++;
            }
        }
    }
}
