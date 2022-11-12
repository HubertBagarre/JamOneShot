using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSlot : MonoBehaviour
{
    public PlayerInfo playerInfo;
    [SerializeField] private Image modelImage;
    [SerializeField] private Image hatImage;
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private GameObject readyPanel;
    [SerializeField] private GameObject faceObject;

    private void Start()
    {
        readyPanel.SetActive(false);
        faceObject.SetActive(false);
    }

    public void UpdatePlayerReady()
    {
        playerNameText.text = $"Player {playerInfo.playerIndex}";
        faceObject.SetActive(true);
    }

    public void UpdateModel(Color color,Sprite hat)
    {
        modelImage.color = color;
        hatImage.sprite = hat;
    }

    public void UpdateReady()
    {
        readyPanel.SetActive(playerInfo.isReady);
    }
}
