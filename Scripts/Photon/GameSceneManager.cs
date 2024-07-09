using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using TMPro;

public class GameSceneManager : MonoBehaviourPunCallbacks
{
    public static GameSceneManager instance;
    public GameObject GamePlayerPrefab;
    public GameObject daggerPrefab;
    public GameObject AxePrefab;
    public GameObject BowPrefab;

    public GameObject LargeMap;
    public GameObject SmallMap;

    public List<Transform> largeMapSpawnPos = new List<Transform>();
    public List<Transform> smallMapSpawnPos = new List<Transform>();

    public List<Transform> redTeamSpawnPos = new List<Transform>();
    public List<Transform> blueTeamSpawnPos = new List<Transform>();

    public List<Transform> SpawnPos = new List<Transform>();
    public Vector2 randomPosition;

    public string mode;
    [SerializeField] private TextMeshProUGUI countdownTimer;
    [SerializeField] float countdownTime = 6f;
    [SerializeField] private TextMeshProUGUI timer;
    public float remainingTime = 300f;
    [SerializeField] float exitTime = 10f;
    public bool gameStarted = false;
    public bool gameEnded = false;
    public float respawnTime;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        instance = this;
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        if (mode == "Solo Mode")
        {
            if (PhotonNetwork.PlayerList.Length <= 4)
            {
                SmallMap.SetActive(true);
                LargeMap.SetActive(false);
            }
            else
            {
                SmallMap.SetActive(false);
                LargeMap.SetActive(true);
            }
        }
        SpawnPlayer();
    }

    private void Update()
    {
        countdownTimerUpdate();
        timerUpdate();
    }

    private void countdownTimerUpdate()
    {
        if (countdownTime > 0)
        {
            countdownTime -= Time.deltaTime;
            countdownTimer.text = Mathf.FloorToInt(countdownTime).ToString();
        }
        else
        {
            if (gameStarted == true) { return; }
            gameStarted = true;
            countdownTimer.text = "Start!";
            Invoke("startGame", 1f);
        }
    }

    private void timerUpdate()
    {
        if (gameStarted == false)
        { return; }
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
        }
        else
        {
            remainingTime = 0;
            timer.color = Color.red;
            if (gameEnded == false)
                StartCoroutine(endGameRoutine());
        }
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private IEnumerator endGameRoutine()
    {
        gameEnded = true;
        yield return new WaitForSeconds(exitTime);
        if (mode == "Solo Mode")
        PhotonNetwork.LoadLevel("Solo Mode Waiting Room");
        else if (mode == "Team Mode")
        PhotonNetwork.LoadLevel("Team Mode Waiting Room");
    }

    private void SpawnPlayer()
    {
        if (CustomizePlayer.weaponIndex == 1)
        {
            GamePlayerPrefab = PhotonNetwork.Instantiate(daggerPrefab.name, Vector2.zero, Quaternion.identity);
        }
        if (CustomizePlayer.weaponIndex == 2)
        {
            GamePlayerPrefab = PhotonNetwork.Instantiate(AxePrefab.name, Vector2.zero, Quaternion.identity);
        }
        if (CustomizePlayer.weaponIndex == 3)
        {
            GamePlayerPrefab = PhotonNetwork.Instantiate(BowPrefab.name, Vector2.zero, Quaternion.identity);
        }

        if (mode == "Solo Mode")
        {
            if (LargeMap.activeSelf == true)
            {
                SpawnPos = largeMapSpawnPos;
            }
            else
            {
                SpawnPos = smallMapSpawnPos;
            }
        }
        else
        {
            if (!PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("team"))
            {
                SpawnPos = redTeamSpawnPos;
            }
            else
            {
                string team = (string)PhotonNetwork.LocalPlayer.CustomProperties["team"];
                if (team == "Red")
                    SpawnPos = redTeamSpawnPos;
                else if (team == "Blue")
                    SpawnPos = blueTeamSpawnPos;
            }
        }
        int i = Random.Range(0, SpawnPos.Count);
        randomPosition = SpawnPos[i].position;
        GamePlayerPrefab.transform.position = randomPosition;

    }

    private void startGame()
    {
        countdownTimer.enabled = false;
    }



}
