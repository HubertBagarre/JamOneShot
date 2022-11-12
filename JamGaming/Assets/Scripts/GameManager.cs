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
    [SerializeField] private int timeBeforeMove = 3;
    [SerializeField] private float displayDuration = 5f;
    [SerializeField] private int targetScore = 6;
    [SerializeField] private GameObject scoreOverlayParent;
    [SerializeField] private TextMeshProUGUI countdownText;
    private GameObject countDownObj;
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
        countDownObj = countdownText.gameObject;
        countDownObj.SetActive(false);
        SetupGame();
        DisplayScore();
    }

    private void SetupGame()
    {
        Debug.Log($"Setting up  game with {players.Count} players");
        sm.StartGameMusic();
        ScoreDisplayer.maxScore = targetScore;
        playedMaps.Clear();
        currentRound = -1;
        waitScore = new WaitForSeconds(displayDuration);
        waitMove = new WaitForSeconds(1);
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
                player.SetupForRound();
                
            }
        }
        StartCoroutine(CountdownRoutine());
        
    }

    private IEnumerator CountdownRoutine()
    {
        countDownObj.SetActive(true);
        for (int i = timeBeforeMove; i > 0; i--)
        {
            countdownText.text = i.ToString();
            yield return waitMove;
        }
        
        countDownObj.SetActive(false);
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
        sm.PlayMenuMusic();
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
        var alive = 0;
        foreach (var player in players)
        {
            if (player.isAlive) alive++;
        }
        if(alive != 1) return;
        EndRound();
    }

    private void EndRound()
    {
        foreach (var player in players)
        {
            player.IncreaseScore();
            player.CanMove(false);
            player.CanLook(false);
        }
        DisplayScore();
    }

    private void ChangeMap()
    {
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
        var player = players[playerIndex];
        player.isAlive = false;
        sm.PlaySound(player,1);
        player.transform.localScale = Vector3.zero;
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        player.CanMove(false);
        CheckToEndRound();
    }
}
