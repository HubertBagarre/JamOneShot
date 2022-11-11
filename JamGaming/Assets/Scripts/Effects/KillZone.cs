using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour, IEffect
{
    public void OnTrigger(SlimeController slimeController)
    {
        Debug.Log("OOF");
        var gm = GameManager.instance;
        if(gm == null) Debug.Log("No gm");
        gm.EliminatePlayer(slimeController.GetComponent<PlayerInfo>().playerIndex);
    }
}
