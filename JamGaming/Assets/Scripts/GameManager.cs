using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Config"),SerializeField] private List<Map> maps = new ();
    [SerializeField] private List<ScoreDisplayer> displayers = new List<ScoreDisplayer>();
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
    [SerializeField] private bool timeCanMove = false;

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
        waitMove = new WaitForSeconds(timeBeforeMove);
        ActivateDisplayers();
        for (var index = 0; index < players.Count; index++)
        {
            var player = players[index];
            player.displayer = displayers[index];
            player.SetupForGame();
        }
    }

    private void ActivateDisplayers()
    {
        for (int i = 0; i < displayers.Count; i++)
        {
            displayers[i].Activate(i < players.Count,i);
        }
    }

    private void Update()
    {
        if(!timeCanMove) return;
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
                player.SetHatActive(true);
                player.CanLook(true);
            }
        }
        StartCoroutine(CountdownRoutine());
        
    }

    private IEnumerator CountdownRoutine()
    {
        yield return waitMove;
        foreach (var player in players)
        {
            player.CanMove(true);
        }
        timeCanMove = true;
    }
    
    private void DisplayScore()
    {
        StartCoroutine(DisplayScoreRoutine());
    }

    private IEnumerator DisplayScoreRoutine()
    {
        timeDisplayText.text = "0:00";
        timeCanMove = false;
        scoreOverlayParent.SetActive(true);
        yield return waitScore;
        if (!DidPlayerWin())
        {
            scoreOverlayParent.SetActive(false);
            StartNewRound();
        }
        else
        {
            EndGame();
        }
    }

    private void EndGame()
    {
        foreach (var player in players)
        {
            Destroy(player.gameObject);
        }
        players.Clear();
        SceneManager.LoadScene(1);
    }

    private bool DidPlayerWin()
    {
        foreach (var player in players)
        {
            if (player.score >= targetScore) return true;
        }
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
            player.CanLook(false);
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
