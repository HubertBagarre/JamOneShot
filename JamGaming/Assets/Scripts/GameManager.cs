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
    [SerializeField] private int targetScore = 6;
    [SerializeField] private GameObject scoreOverlayParent;
    private WaitForSeconds waitScore;
    private WaitForSeconds waitMove;
    
    [Header("Current Game")]
    public static List<PlayerInfo> players = new List<PlayerInfo>();

    [SerializeField] private Map currentMap;
    private GameObject currentMapObj;
    [SerializeField] private int currentRound = 0;
    [SerializeField] private float elapsedTime;
    [SerializeField] private bool canMove = false;

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
        waitScore = new WaitForSeconds(displayDuration);
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
        StartCoroutine(CountdownRoutine());
        
    }

    private IEnumerator CountdownRoutine()
    {
        canMove = false;
        yield return waitMove;
        canMove = true;
    }
    
    private void DisplayScore()
    {
        StartCoroutine(DisplayScoreRoutine());
    }

    private IEnumerator DisplayScoreRoutine()
    {
        scoreOverlayParent.SetActive(true);
        yield return waitScore;
        scoreOverlayParent.SetActive(false);
        foreach (var player in players)
        {
            player.CanMove(true);
            player.SetHatActive(true);
        }
        if(!DidPlayerWin()) StartNewRound();
    }

    private bool DidPlayerWin()
    {
        return false;
    }

    private void CheckToEndRound()
    {
        Debug.Log($"Checking Round End");
        var alive = 0;
        foreach (var player in players)
        {
            if (player.isAlive) alive++;
        }
        Debug.Log($"{alive} players Left!");
        if(alive>1) return;
        EndRound();
    }

    private void EndRound()
    {
        Debug.Log($"Ending Round");
        foreach (var player in players)
        {
            player.IncreaseScore();
        }
        DisplayScore();
    }

    private void ChangeMap()
    {
        Debug.Log($"Changing Map");
        if(currentMapObj!=null) Destroy(currentMapObj);
        currentMap = maps[0];
        currentMapObj = Instantiate(currentMap, Vector3.zero, Quaternion.identity).gameObject;
    }

    public void EliminatePlayer(int playerIndex)
    {
        Debug.Log($"Killing player {playerIndex}");
        var player = players[playerIndex];
        player.isAlive = false;
        player.gameObject.SetActive(false);
        player.CanMove(false);
        player.SetHatActive(false);
        CheckToEndRound();
    }
}
