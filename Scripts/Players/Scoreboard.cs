using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro; 

public class Scoreboard : MonoBehaviourPunCallbacks
{
    public Transform scoreboard;
    public bool showing = false;
    public static Scoreboard instance;
    [SerializeField] GameObject scoreboardItemPrefab;
    Vector2 originalPosition;

    public List<ScoreboardItem> ScoreList = new List<ScoreboardItem>();

    Dictionary<Player, ScoreboardItem> scoreboardItems = new Dictionary<Player, ScoreboardItem>();
    private void Start()
    {
        instance = this;
        originalPosition = scoreboard.transform.position;
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            AddScoreboardItem(player);
        }
        InvokeRepeating("rearrangeRows", 1f, 1f);
    }

    private void Update()
    {
        showScoreboard();
        if (GameSceneManager.instance.gameEnded)
        {
            showing = true;
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            showing = !showing;
        }


    }

    private void AddScoreboardItem(Player player)
    {
        ScoreboardItem item = Instantiate(scoreboardItemPrefab, scoreboard).GetComponent<ScoreboardItem>();
        item.Initialize(player);
        scoreboardItems[player] = item;
        ScoreList.Add(item);
    }

    public void RemoveScoreboardItem(Player player)
    {
        Destroy(scoreboardItems[player].gameObject);
        scoreboardItems.Remove(player);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        AddScoreboardItem(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        RemoveScoreboardItem(otherPlayer);
    }

    public void rearrangeRows() 
    {
        var SortedList = ScoreList.OrderByDescending(item => item.score).ToList();
        for (int i=0; i< ScoreList.Count-1; i++)
        {
            SortedList[i].transform.SetSiblingIndex(i + 1);
        }
    }

    public void showScoreboard()
    {
        if (showing)
        {
            scoreboard.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            scoreboard.rotation = Quaternion.Euler(90, 0, 0);
        }
    }



}
