using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class LobbyManager : MonoBehaviour
{
    [SerializeField] private List<PlayerSlot> slots;
    [SerializeField] private List<Color> availableColors = new ();
    [SerializeField] private List<Sprite> hats = new ();
    
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
        slots[slotIndex].UpdateModel(newColor,hats[0]);
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
        slots[slotIndex].UpdateModel(newColor,players[slotIndex].currentHat);
        return newColor;
    }

    public Sprite GetNewHat(int slotIndex)
    {
        var newHat = hats[Random.Range(0, hats.Count)];
        slots[slotIndex].UpdateModel(players[slotIndex].currentColor,newHat);
        return newHat;
    }
    
    public Sprite GetNextHat(Sprite currentHat,int slotIndex)
    {
        var index = hats.IndexOf(currentHat);
        index++;
        if (index >= hats.Count) index = 0;
        var newHat = hats[index];
        slots[slotIndex].UpdateModel(players[slotIndex].currentColor,newHat);
        return newHat;
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
        if(canStart) StartGame();
    }

    private void StartGame()
    {
        GameManager.players = players;
        foreach (var player in players)
        {
            DontDestroyOnLoad(player.gameObject);
        }
        SceneManager.LoadScene(3);
    }
}