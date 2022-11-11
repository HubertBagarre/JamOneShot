using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LobbyController : MonoBehaviour
{
    public bool isInLobby = true;
    public Color currentColor;
    private LobbyManager lobbyManager;
    public int playerIndex = -1;
    public bool isReady = false;

    private void Start()
    {
        lobbyManager = LobbyManager.instance;
        if(lobbyManager == null) return;
        lobbyManager.AddPlayer(this);
        currentColor = lobbyManager.GetNewColor(playerIndex);
    }

    public void NextColor(InputAction.CallbackContext ctx)
    {
        if(!isInLobby) return;
        if(!ctx.started) return;
        if(isReady) return;
        currentColor = lobbyManager.ChangeColor(currentColor, true,playerIndex);
    }

    public void PreviousColor(InputAction.CallbackContext ctx)
    {
        if(!isInLobby) return;
        if(!ctx.started) return;
        if(isReady) return;
        currentColor = lobbyManager.ChangeColor(currentColor, false,playerIndex);
    }

    public void ToggleReady(InputAction.CallbackContext ctx)
    {
        if(!isInLobby) return;
        if(!ctx.started) return;
        isReady = !isReady;
        lobbyManager.SetReady(playerIndex);
    }
}
