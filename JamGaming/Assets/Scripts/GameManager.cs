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
    private WaitForSeconds wait;
    
    [Header("Current Game")]
    public static List<PlayerInfo> players = new List<PlayerInfo>();

    [SerializeField] private Map currentMap;
    [SerializeField] private int currentRound = 0;
    [SerializeField] private float elapsedTime;
    [SerializeField] private bool canMove = true;

    public static GameManager instance;

    private void Awake()
    {
        instance = this;
    }


    private void Start()
    {
        SetupGame();
        DisplayScore();
    }

    private void SetupGame()
    {
        Debug.Log($"Setting up  game with {players.Count} players");
        currentRound = -1;
        wait = new WaitForSeconds(displayDuration);
        foreach (var player in players)
        {
            player.SetupForGame();
        }
    }

    private void Update()
    {
        if(canMove) return;
        elapsedTime += Time.deltaTime;
        timeDisplayText.text = ConvertedElapsedTime(elapsedTime);
        if (elapsedTime >= maxRoundTime)
        {
            Debug.Log($"Max time {maxRoundTime} has been reached!");
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
        Debug.Log($"Starting Round up  game with {players.Count} players");
        currentRound++;
        ChangeMap();
        elapsedTime = 0;
        for (int i = 0; i < players.Count; i++)
        {
            var player = players[i];
            if (i >= 0 || i < currentMap.spawnPoints.Count)
            {
                player.transform.position = currentMap.spawnPoints[i].position;
                player.gameObject.SetActive(true);
                player.isAlive = true;
            }
        }
        //Routine (countdown)
        canMove = false;
    }
    
    private void DisplayScore()
    {
        canMove = true;
        //Routine (display score for x amount of time)
        StartNewRound();
    }

    private IEnumerator DisplayScoreRoutine()
    {
        yield return wait;
        
    }

    private void CheckVictory()
    {
        
    }

    private void CheckToEndRound()
    {
        var alive = 0;
        foreach (var player in players)
        {
            if (player.isAlive) alive++;
        }
        if(alive>1) return;
        EndRound();
    }

    private void EndRound()
    {
        foreach (var player in players)
        {
            player.IncreaseScore();
        }
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
        CheckToEndRound();
    }
}
