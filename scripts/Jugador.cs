using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementTutorial : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float groundDrag;
    public float airMultiplier;
    bool readyToJump;

    [HideInInspector] public float walkSpeed;
    [HideInInspector] public float sprintSpeed;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    public Transform orientation;
    public Transform cameraTransform;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;
    }

    private void Update()
    {
        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);

        MyInput();
        SpeedControl();

        // handle drag
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        // Obtener entradas de movimiento utilizando las teclas WASD
        horizontalInput = 0;
        verticalInput = 0;

        if (Input.GetKey(KeyCode.W))
            verticalInput = 1;
        if (Input.GetKey(KeyCode.S))
            verticalInput = -1;
        if (Input.GetKey(KeyCode.A))
            horizontalInput = -1;
        if (Input.GetKey(KeyCode.D))
            horizontalInput = 1;

        // Saltar cuando se presiona la tecla de salto y el jugador está listo para saltar y en el suelo
        if (Input.GetKeyDown(jumpKey) && readyToJump && grounded)
        {
            Jump();
        }
    }

    private void MovePlayer()
    {
        // Calcular la dirección de movimiento en relación con la cámara
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        moveDirection = forward * verticalInput + right * horizontalInput;

        // Aplicar fuerza de movimiento en el suelo y en el aire
        if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        else
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // Limitar la velocidad si es necesario
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        // Aplicar fuerza de salto
        rb.AddForce(Vector3.up * moveSpeed, ForceMode.Impulse);

        // Desactivar la capacidad de saltar hasta que el cooldown termine
        readyToJump = false;
        Invoke(nameof(ResetJump), 1.5f); // Tiempo de recarga de 1.5 segundos para poder saltar de nuevo
    }

    private void ResetJump()
    {
        readyToJump = true;
    }
}
