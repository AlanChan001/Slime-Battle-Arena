using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.UI;
using System;


public class ScoreboardItem : MonoBehaviourPunCallbacks
{
    public TMP_Text usernameText;
    public TMP_Text killDeathText;
    public TMP_Text scoreText;
    public int score;
    [SerializeField] Image background;
    [SerializeField] Color backgroundColor;


    Player player;
    public void Initialize(Player player)
    {
        usernameText.text = player.NickName;
        this.player = player;
        if (player == PhotonNetwork.LocalPlayer)
        {
            background.color = backgroundColor;
        }
    }

    void UpdateStats()
    {
            player.CustomProperties.TryGetValue("score", out object myScore);
            scoreText.text = myScore.ToString();
            score = Convert.ToInt32(myScore);
            player.CustomProperties.TryGetValue("kills", out object kills);
            player.CustomProperties.TryGetValue("deaths", out object deaths);
            killDeathText.text = kills.ToString() + "/ " + deaths.ToString();
    }

    
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (targetPlayer == player)
        {
            if (changedProps.ContainsKey("kills") || changedProps.ContainsKey("deaths") 
                || changedProps.ContainsKey("score"))
            {
                if (GameSceneManager.instance.gameEnded == false)
                UpdateStats(); 
            }
        }
    }
    


}
