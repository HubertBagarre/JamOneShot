using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportZone : MonoBehaviour, IEffect
{
    [SerializeField] private float teleportCd = 0.5f;
    [SerializeField] private Transform otherPortal;
    private WaitForSeconds wait;

    private void Start()
    {
        wait = new WaitForSeconds(teleportCd);
    }

    public void OnTrigger(SlimeController slimeController)
    {
        var player = slimeController.GetComponent<PlayerInfo>();
        if(!player.canTeleport) return;
        player.canTeleport = false;
        var slimeTransform = slimeController.transform;
        var rb = slimeController.slimeRb;
        var velocity = rb.velocity;
        var enterVelocity = transform.InverseTransformDirection(velocity);
        var exitVelocity = otherPortal.TransformDirection(enterVelocity);
        slimeTransform.position = otherPortal.position;
        rb.velocity = exitVelocity * -1;
        StartCoroutine(TpCdRoutine(player));
    }

    private IEnumerator TpCdRoutine(PlayerInfo player)  
    {
        yield return wait;
        player.canTeleport = true;
    }
}
