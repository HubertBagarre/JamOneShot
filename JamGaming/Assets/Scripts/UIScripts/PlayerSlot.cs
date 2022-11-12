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

    private SoundManager sm;

    private void Start()
    {
        sm = SoundManager.instance;
        readyPanel.SetActive(false);
        faceObject.SetActive(false);
    }

    public void UpdatePlayerReady()
    {
        playerNameText.text = $"Player {playerInfo.playerIndex}";
        faceObject.SetActive(true);
        sm.PlaySound(playerInfo,4);
    }

    public void UpdateModel(Color color,Sprite hat)
    {
        modelImage.color = color;
        hatImage.sprite = hat;
        sm.PlaySound(playerInfo,4);
    }

    public void UpdateReady()
    {
        sm.PlaySound(4);
        readyPanel.SetActive(playerInfo.isReady);
        sm.PlaySound(playerInfo,4);
    }
}
