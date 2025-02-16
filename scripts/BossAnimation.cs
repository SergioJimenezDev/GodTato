using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimation : MonoBehaviour
{
    public float detectionRange = 20f; // Rango de detección del jugador
    private Transform player; // Referencia al jugador
    private Animator animator; // Referencia al componente Animator

    // Start es llamado antes de la primera frame update
    void Start()
    {
        // Encuentra al jugador por etiqueta (tag)
        player = GameObject.FindGameObjectWithTag("Player").transform;
        // Obtén el componente Animator del enemigo
        animator = GetComponent<Animator>();
    }

    // Update es llamado una vez por frame
    void Update()
    {
        // Calcula la distancia al jugador
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Si la distancia al jugador es menor que el rango de detección, cambia la animación a "attack_01"
        if (distanceToPlayer < detectionRange)
        {
            animator.SetBool("isAttacking", true);
        }
        else
        {
            // De lo contrario, vuelve a la animación "idle"
            animator.SetBool("isAttacking", false);
        }
    }
}
