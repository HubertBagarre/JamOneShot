using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectTriggerer : MonoBehaviour
{
    public SlimeController controller;
    public Rigidbody2D slimeRb;
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == 9)
        {
            Debug.Log("Deflect !");
            controller.Deflect(col.GetComponent<EffectTriggerer>().controller.slimeRb.velocity.normalized);
        }
        else
        {
            if (col.gameObject.layer != 7) return;
            GameManager.instance.EliminatePlayer(controller.infos.playerIndex);
        }
    }
}
