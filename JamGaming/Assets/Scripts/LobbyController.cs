using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LobbyController : MonoBehaviour
{
    public int score = 0;

    private void Start()
    {
        JoinLobby.instance.AddPlayer(this);
    }

    public void OnInputPressed(InputAction.CallbackContext ctx)
    {
        if(!ctx.started) return;
        Debug.Log($"{ctx} DID SOMETHING");
        
        score++;
    }
}
