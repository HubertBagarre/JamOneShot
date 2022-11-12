using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectTriggerer : MonoBehaviour
{
    public SlimeController controller;
    public bool canKill;
    private float _timer;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == 9)
        {
            canKill = false;
            Debug.Log("Deflect !");
            controller.Deflect();
        }
        else
        {
            if (!canKill) return;
            var head = col.GetComponent<HeadColliderScript>();
            if(head!=null) GameManager.instance.EliminatePlayer(head.slimeController.infos.playerIndex);
        }
    }
}
