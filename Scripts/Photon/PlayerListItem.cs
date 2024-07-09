using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.UI;

public class PlayerListItem : MonoBehaviourPunCallbacks
{
    public TMP_Text playerNameText;
    Player player;
    [SerializeField] Image background;
    [SerializeField] Color backgroundColor;

    public void Initialize(Player player)
    {
        playerNameText.text = player.NickName;
        this.player = player;
        if (player == PhotonNetwork.LocalPlayer)
        {
            background.color = backgroundColor;
        }
    }


}
