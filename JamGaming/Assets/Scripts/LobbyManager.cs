using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class LobbyManager : MonoBehaviour
{
    [SerializeField] private List<PlayerSlot> slots;
    [SerializeField] private List<Color> availableColors = new List<Color>();

    public static LobbyManager instance;

    public List<PlayerInfo> players = new ();

    public void Awake()
    {
        instance = this;
    }

    public void AddPlayer(PlayerInfo lc)
    {
        lc.playerIndex = players.Count;
        slots[lc.playerIndex].playerInfo = lc;
        slots[lc.playerIndex].UpdatePlayerReady();
        players.Add(lc);
    }

    public void RemovePlayer(PlayerInfo lc)
    {
        if (!players.Contains(lc)) return;
        players.Remove(lc);
    }

    public Color GetNewColor(int slotIndex)
    {
        var newColor = availableColors[Random.Range(0, availableColors.Count)];
        availableColors.Remove(newColor);
        slots[slotIndex].UpdateColor(newColor);
        return newColor;
    }

    public Color ChangeColor(Color previousColor,bool next,int slotIndex)
    {
        var currentIndex = availableColors.IndexOf(previousColor);
        currentIndex = next ? currentIndex + 1 : currentIndex - 1;
        if (currentIndex < 0) currentIndex = availableColors.Count - 1;
        if (currentIndex >= availableColors.Count) currentIndex = 0;
        var newColor = availableColors[currentIndex];
        availableColors.Remove(newColor);
        availableColors.Add(previousColor);
        slots[slotIndex].UpdateColor(newColor);
        return newColor;
    }

    public void SetReady(int playerIndex)
    {
        slots[playerIndex].UpdateReady();
        CheckStartGame();
    }

    private void CheckStartGame()
    {
        if(players.Count<=1) return;
        var canStart = true;
        foreach (var player in players)
        {
            if (!player.isReady) canStart = false;
        }
        if(canStart) Debug.Log("Starting");
    }
}