using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyZone : MonoBehaviour, IEffect
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void OnTrigger(SlimeController slimeController)
    {
        slimeController._remainingRebound++;
        slimeController._maxRebound++;
        animator.SetTrigger("Bounce");
        
    }
}
