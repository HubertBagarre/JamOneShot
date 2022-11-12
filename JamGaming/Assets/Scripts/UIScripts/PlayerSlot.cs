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
        sm.PlaySound(4);
        playerNameText.text = $"Player {playerInfo.playerIndex}";
        faceObject.SetActive(true);
    }

    public void UpdateModel(Color color,Sprite hat)
    {
        sm.PlaySound(4);
        modelImage.color = color;
        hatImage.sprite = hat;
    }

    public void UpdateReady()
    {
        sm.PlaySound(4);
        readyPanel.SetActive(playerInfo.isReady);
    }
}
