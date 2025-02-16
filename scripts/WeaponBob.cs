using UnityEngine;

public class WeaponBob : MonoBehaviour
{
    [Header("Mouse Sway Settings")]
    public float swayAmount = 0.02f;  // Cantidad de balanceo
    public float maxSwayAmount = 0.06f;  // Cantidad máxima de balanceo
    public float swaySmoothness = 6f;  // Suavidad del balanceo

    [Header("Movement Sway Settings")]
    public float movementSwayAmount = 0.05f;  // Cantidad de balanceo al moverse
    public float movementSwaySpeed = 3f;  // Velocidad del balanceo al moverse
    public float swayFrequency = 1.5f;  // Frecuencia del movimiento en S

    private Vector3 initialPosition;

    void Start()
    {
        // Guardar la posición inicial del arma
        initialPosition = transform.localPosition;
    }

    void Update()
    {
        HandleMouseSway();
        HandleMovementSway();
    }

    void HandleMouseSway()
    {
        // Obtener el movimiento del ratón
        float mouseX = Input.GetAxis("Mouse X") * swayAmount;
        float mouseY = Input.GetAxis("Mouse Y") * swayAmount;

        // Limitar el balanceo máximo
        mouseX = Mathf.Clamp(mouseX, -maxSwayAmount, maxSwayAmount);
        mouseY = Mathf.Clamp(mouseY, -maxSwayAmount, maxSwayAmount);

        // Crear el nuevo objetivo de posición con el balanceo del ratón
        Vector3 finalMousePosition = new Vector3(mouseX, mouseY, 0);

        // Interpolar la posición del arma para que sea suave
        transform.localPosition = Vector3.Lerp(transform.localPosition, finalMousePosition + initialPosition, Time.deltaTime * swaySmoothness);
    }

    void HandleMovementSway()
    {
        // Obtener el movimiento del jugador
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        // Calcular la dirección del movimiento
        Vector3 movement = new Vector3(moveX, moveY, 0);
        if (movement.magnitude > 0)
        {
            // Calcular el desplazamiento en X y Y
            float swayOffsetX = Mathf.Sin(Time.time * swayFrequency) * movementSwayAmount;
            float swayOffsetY = Mathf.Abs(Mathf.Sin(Time.time * swayFrequency * 2f)) * movementSwayAmount * 0.5f;

            Vector3 finalMovePosition = new Vector3(swayOffsetX, swayOffsetY, initialPosition.z);

            // Interpolar la posición del arma para que sea suave
            transform.localPosition = Vector3.Lerp(transform.localPosition, finalMovePosition + initialPosition, Time.deltaTime * movementSwaySpeed);
        }
        else
        {
            // Si no hay movimiento, regresar al punto inicial suavemente
            transform.localPosition = Vector3.Lerp(transform.localPosition, initialPosition, Time.deltaTime * movementSwaySpeed);
        }
    }


}
