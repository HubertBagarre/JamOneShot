using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour, IEffect
{
    public void OnTrigger(SlimeController slimeController)
    {
        GameManager.instance.EliminatePlayer(slimeController.GetComponent<PlayerInfo>().playerIndex);
    }
}
