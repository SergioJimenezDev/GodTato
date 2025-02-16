using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimation : MonoBehaviour
{
    public float detectionRange = 20f; // Rango de detecci�n del jugador
    private Transform player; // Referencia al jugador
    private Animator animator; // Referencia al componente Animator

    // Start es llamado antes de la primera frame update
    void Start()
    {
        // Encuentra al jugador por etiqueta (tag)
        player = GameObject.FindGameObjectWithTag("Player").transform;
        // Obt�n el componente Animator del enemigo
        animator = GetComponent<Animator>();
    }

    // Update es llamado una vez por frame
    void Update()
    {
        // Calcula la distancia al jugador
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Si la distancia al jugador es menor que el rango de detecci�n, cambia la animaci�n a "attack_01"
        if (distanceToPlayer < detectionRange)
        {
            animator.SetBool("isAttacking", true);
        }
        else
        {
            // De lo contrario, vuelve a la animaci�n "idle"
            animator.SetBool("isAttacking", false);
        }
    }
}
