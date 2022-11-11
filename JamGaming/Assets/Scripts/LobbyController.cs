using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LobbyController : MonoBehaviour
{
    public bool isInLobby = true;
    private LobbyManager lobbyManager;
    private PlayerInfo infos;
    
    private void Start()
    {
        infos = GetComponent<PlayerInfo>();
        lobbyManager = LobbyManager.instance;
        if(lobbyManager == null) return;
        lobbyManager.AddPlayer(infos);
        infos.currentColor = lobbyManager.GetNewColor(infos.playerIndex);
    }

    public void NextColor(InputAction.CallbackContext ctx)
    {
        if(!isInLobby) return;
        if(!ctx.started) return;
        if(infos.isReady) return;
        infos.currentColor = lobbyManager.ChangeColor(infos.currentColor, true,infos.playerIndex);
    }

    public void PreviousColor(InputAction.CallbackContext ctx)
    {
        if(!isInLobby) return;
        if(!ctx.started) return;
        if(infos.isReady) return;
        infos.currentColor = lobbyManager.ChangeColor(infos.currentColor, false,infos.playerIndex);
    }

    public void ToggleReady(InputAction.CallbackContext ctx)
    {
        if(!isInLobby) return;
        if(!ctx.started) return;
        infos.isReady = !infos.isReady;
        lobbyManager.SetReady(infos.playerIndex);
    }
}
