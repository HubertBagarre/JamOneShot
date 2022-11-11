using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Config"),SerializeField] private List<Map> maps = new ();
    private List<GameObject> spawnedMaps = new List<GameObject>();
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
    private List<GameObject> playedMaps = new List<GameObject>();
    private int winnerIndex;
    [SerializeField] private Map currentMap;
    private GameObject currentMapObj;
    [SerializeField] private int currentRound = 0;
    [SerializeField] private float elapsedTime;
    [SerializeField] private bool timeCanMove = false;

    private SoundManager sm;
    
    public static GameManager instance;

    private void Awake()
    {
        instance = this;
    }


    private void Start()
    {
        sm = SoundManager.instance;
        SetupGame();
        DisplayScore();
    }

    private void SetupGame()
    {
        Debug.Log($"Setting up  game with {players.Count} players");
        sm.PlayMusic(0);
        playedMaps.Clear();
        currentRound = -1;
        waitScore = new WaitForSeconds(displayDuration);
        waitMove = new WaitForSeconds(timeBeforeMove);
        foreach (var map in maps)
        {
            var spawned = Instantiate(map, Vector3.zero, Quaternion.identity);
            foreach (var spawnPoint in spawned.spawnPoints)
            {
                spawnPoint.gameObject.SetActive(false);
            }
            var mapObj = spawned.gameObject;
            mapObj.SetActive(false);
            spawnedMaps.Add(mapObj);
        }
        ActivateDisplayers();
        sm.ClearSources();
        for (var index = 0; index < players.Count; index++)
        {
            var player = players[index];
            player.displayer = displayers[index];
            player.SetupForGame();
            sm.CreateSources(player);
        }
    }

    private void ActivateDisplayers()
    {
        for (int i = 0; i < displayers.Count; i++)
        {
            displayers[i].Activate(false,i,Color.white);
            if (i < players.Count)
            {
                displayers[i].Activate(true,i,players[i].currentColor);
            }
            
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
                var transform1 = player.transform;
                transform1.position = currentMap.spawnPoints[i].position;
                transform1.localRotation = currentMap.spawnPoints[i].localRotation;
                player.ResetNormal();
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
        sm.StopMusic();
        SceneManager.LoadScene(1);
    }

    private bool DidPlayerWin()
    {
        foreach (var player in players)
        {
            if (player.score >= targetScore)
            {
                winnerIndex = player.playerIndex;
                return true;
            }
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
        if(currentMapObj!=null) currentMapObj.SetActive(false);
        if (spawnedMaps.Count <= 0)
        {
            spawnedMaps = playedMaps.ToList();
            playedMaps.Clear();
        }
        currentMapObj = spawnedMaps[Random.Range(0,spawnedMaps.Count)];
        playedMaps.Add(currentMapObj);
        spawnedMaps.Remove(currentMapObj);
        currentMap = currentMapObj.GetComponent<Map>();
        currentMapObj.SetActive(true);
    }

    public void EliminatePlayer(int playerIndex)
    {
        Debug.Log($"Killing player {playerIndex}");
        var player = players[playerIndex];
        player.isAlive = false;
        player.gameObject.SetActive(false);
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        player.CanMove(false);
        player.SetHatActive(false);
        CheckToEndRound();
    }
}
