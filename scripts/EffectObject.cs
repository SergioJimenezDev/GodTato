using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectObject : MonoBehaviour
{
    public float time;
    void Start()
    {
        Destroy(gameObject, time);
    }


}
