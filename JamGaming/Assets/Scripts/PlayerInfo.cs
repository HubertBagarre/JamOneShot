using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public int playerIndex = -1;
    public bool isReady = false;
    public Color currentColor;
    
    public int score = 0;
    public bool isAlive = false;
    
    private LobbyController lobbyController;
    private SlimeController inGameController;
    
    private void Start()
    {
        lobbyController = GetComponent<LobbyController>();
        inGameController = GetComponent<SlimeController>();
    }

    public void SetupForGame()
    {
        score = 0;
        lobbyController.isInLobby = false;
        inGameController.enabled = false;
        isAlive = false;
    }

    public void IncreaseScore()
    {
        if (isAlive) score++;
    }

    public void CanMove(bool value)
    {
        inGameController.enabled = value;
    }
}