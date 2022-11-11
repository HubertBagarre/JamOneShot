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
    public bool isHatActive = false;

    [SerializeField] private Transform hatParent;
    private LobbyController lobbyController;
    private SlimeController inGameController;
    
    private void Start()
    {
        lobbyController = GetComponent<LobbyController>();
        inGameController = GetComponent<SlimeController>();
    }

    public void SetupForGame()
    {
        Debug.Log($"Setting Player {playerIndex} for game");
        score = 0;
        lobbyController.isInLobby = false;
        inGameController.enabled = true;
        CanLook(false);
        CanMove(false);
        isAlive = false;
        SetHatActive(false);
    }

    public void IncreaseScore()
    {
        if (isAlive) score++;
    }

    public void CanLook(bool value)
    {
        inGameController.canLook = value;
    }
    
    public void CanMove(bool value)
    {
        inGameController.canJump = value;
        inGameController.onWall = value;
    }

    public void SetHatActive(bool value)
    {
        isHatActive = value;
        hatParent.gameObject.SetActive(isHatActive);
    }
}