using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyZone : MonoBehaviour, IEffect
{
    private Animator animator;
    private SoundManager sm;
    private static readonly int Bounce = Animator.StringToHash("Bounce");

    private void Start()
    {
        animator = GetComponent<Animator>();
        sm = SoundManager.instance;
    }

    public void OnTrigger(SlimeController slimeController)
    {
        slimeController._remainingRebound++;
        slimeController._maxRebound++;
        sm.PlaySound(slimeController.infos,4);
        animator.SetTrigger(Bounce);
        
    }
}
