using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyZone : MonoBehaviour, IEffect
{
    public void OnTrigger(SlimeController slimeController)
    {
        slimeController._remainingRebound++;
        slimeController._maxRebound++;
        
    }
}
