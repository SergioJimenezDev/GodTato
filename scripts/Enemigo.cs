using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemigo : MonoBehaviour
{
    public Transform Objetivo;
    public float velocidad;
    public Animator animator;
    public Quaternion angulo;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

}
