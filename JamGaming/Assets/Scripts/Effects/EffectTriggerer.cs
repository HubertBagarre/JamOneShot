using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectTriggerer : MonoBehaviour
{
    [SerializeField] private SlimeController controller;
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        col.GetComponent<IEffect>()?.OnTrigger(controller);
    }
}
