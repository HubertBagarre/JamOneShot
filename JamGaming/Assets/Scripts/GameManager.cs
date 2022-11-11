using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Config"),SerializeField] private List<Map> maps = new ();
    [SerializeField] private TextMeshProUGUI timeDisplayText;
    [SerializeField] private float maxRoundTime = 90f;
    [SerializeField] private float timeBeforeMove = 3f;
    [SerializeField] private float displayDuration = 5f;
    
    [Header("Current Game")]
    public static List<PlayerInfo> players = new List<PlayerInfo>();

    [SerializeField] private Map currentMap;
    [SerializeField] private float elapsedTime;
    [SerializeField] private bool isDisplayingScore = true;

    public static GameManager instance;

    private void Awake()
    {
        instance = this;
    }


    private void Start()
    {
        DisplayScore();
    }

    private void Update()
    {
        if(isDisplayingScore) return;
        elapsedTime += Time.deltaTime;
        timeDisplayText.text = ConvertedElapsedTime(elapsedTime);
        if (elapsedTime >= maxRoundTime)
        {
            StartDeathZone();
        }
        
    }

    private static string ConvertedElapsedTime(float time)
    {
        var zero = time%60 < 10 ? "0":"";
        return $"{(int) time / 60}:{zero}{(int)time % 60}";
    }

    private void StartDeathZone()
    {
        
    }

    private void StartNewRound()
    {
        ChangeMap();
        elapsedTime = 0;
        for (int i = 0; i < players.Count; i++)
        {
            if(i >=0 ||  i < currentMap.spawnPoints.Count) players[i].transform.position = currentMap.spawnPoints[i].position;
        }
        foreach (var player in players)
        {
            player.isAlive = true;
        }
        //Routine (countdown)
        isDisplayingScore = false;
    }
    
    private void DisplayScore()
    {
        isDisplayingScore = true;
        //Routine (display score for x amount of time)
        StartNewRound();
    }

    private void CheckVictory()
    {
        
    }

    private void CheckToEndRound()
    {
        
    }

    public void OnPlayerDeath()
    {
        CheckToEndRound();
    }

    private void ChangeMap()
    {
        Destroy(currentMap);
        currentMap = maps[0];
        Instantiate(currentMap, Vector3.zero, Quaternion.identity);
    }

    public void EliminatePlayer(int playerIndex)
    {
        players[playerIndex].gameObject.SetActive(false);
    }
}
