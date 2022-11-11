using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSlot : MonoBehaviour
{
    public LobbyController lobbyController;
    [SerializeField] private Image modelImage;
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private GameObject readyPanel;

    private void Start()
    {
        readyPanel.SetActive(false);
    }

    public void UpdatePlayerReady()
    {
        playerNameText.text = $"Player {lobbyController.playerIndex}";
    }

    public void UpdateColor(Color color)
    {
        modelImage.color = color;
    }

    public void UpdateReady()
    {
        readyPanel.SetActive(lobbyController.isReady);
    }
}
