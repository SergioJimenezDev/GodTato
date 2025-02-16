using UnityEngine;

public class FallingAndBouncing : MonoBehaviour
{
    public float initialHeight = 7f; // Altura inicial desde la que cae el objeto (ajustada para empezar m�s abajo)
    public float gravity = -50f; // Aceleraci�n de la gravedad (aumentada para caer m�s r�pido)
    public float bounceDamping = 0.5f; // Factor de amortiguaci�n del rebote (disminuido para rebotes m�s altos)
    public int maxBounces = 3; // N�mero m�ximo de rebotes

    private Vector2 velocity;
    private int bounceCount = 0;

    void Start()
    {
        // Establecer la posici�n inicial del objeto en la altura especificada
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

            // Actualizar la posici�n del objeto
            transform.localPosition += (Vector3)(velocity * Time.deltaTime);

            // Detectar si el objeto ha tocado el l�mite inferior (y < 5)
            if (transform.localPosition.y < 5)
            {
                // Asegurarse de que el objeto no quede por debajo del l�mite inferior
                transform.localPosition = new Vector3(transform.localPosition.x, 5, transform.localPosition.z);

                // Invertir la velocidad en el eje y para simular el rebote
                velocity.y = -velocity.y * bounceDamping;

                bounceCount++;
            }
        }
    }
}
