using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JoinLobby : MonoBehaviour
{
    public static JoinLobby instance;

    public List<LobbyController> players = new List<LobbyController>();

    public void Awake()
    {
        instance = this;
    }

    public void AddPlayer(LobbyController lc)
    {
        players.Add(lc);
    }
    
    public void RemovePlayer(LobbyController lc)
    {
        if(!players.Contains(lc)) return;
        players.Remove(lc);
    }
    
    public void OnPlayerJoin(InputAction.CallbackContext ctx)
    {
        
    }
    
    public void OnPlayerLeave(InputAction.CallbackContext ctx)
    {
        
    }
}
