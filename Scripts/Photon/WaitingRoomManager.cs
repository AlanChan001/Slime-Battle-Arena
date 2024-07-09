using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Text;
using Photon.Pun;
using Photon.Realtime;

public class WaitingRoomManager : MonoBehaviourPunCallbacks
{
    [SerializeField] TextMeshProUGUI textRoomname;
    [SerializeField] Button StartGameButton;

    [SerializeField] GameObject playerListItem;
    public Transform redTeamPlayerList;
    public Transform blueTeamPlayerList;
    public List<PlayerListItem> playerList = new List<PlayerListItem>();
    Dictionary<Player, PlayerListItem> playerListItems = new Dictionary<Player, PlayerListItem>();



    private void Start()
    {
        if (PhotonNetwork.CurrentRoom == null)
        {
            SceneManager.LoadScene("Lobby");
        }
        else
        {
            textRoomname.text = PhotonNetwork.CurrentRoom.Name;
            //UpdatePlayerList();
        }
        StartGameButton.interactable = PhotonNetwork.IsMasterClient;
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            AddPlayerListItem(player);
        }
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        StartGameButton.interactable = PhotonNetwork.IsMasterClient;
    }

    private void AddPlayerListItem(Player player)
    {
        PlayerListItem item = Instantiate(playerListItem, redTeamPlayerList).GetComponent<PlayerListItem>();
        item.Initialize(player);
        playerListItems[player] = item;
        playerList.Add(item);
    }

    public void RemovePlayerListItem(Player player)
    {
        Destroy(playerListItems[player].gameObject);
        playerListItems.Remove(player);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        AddPlayerListItem(newPlayer);
        
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        RemovePlayerListItem(otherPlayer);
    }

    public void OnClickStartSoloMode()
    {
        if (!PhotonNetwork.IsConnected)
            return;
        PhotonNetwork.LoadLevel("Solo Mode");
        RoomOptions options = new RoomOptions();
        options.BroadcastPropsChangeToAll = true;
    }

    public void OnClickStartTeamMode()
    {
        if (!PhotonNetwork.IsConnected)
            return;
        PhotonNetwork.LoadLevel("Team Mode");
        RoomOptions options = new RoomOptions();
        options.BroadcastPropsChangeToAll = true;
    }

    public void OnClickLeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("Lobby");
    }



    public void OnClickChangeBlueTeam()
    {
        CustomizePlayer.instance.SetTeam("Blue");
        Player player = PhotonNetwork.LocalPlayer;
        GetComponent<PhotonView>().RPC("ChangeBlue", RpcTarget.AllBuffered,player);
    }

    [PunRPC]
    public void ChangeBlue(Player player)
    {
        playerListItems[player].transform.SetParent(blueTeamPlayerList);
    }


    public void OnClickChangeRedTeam()
    {
        CustomizePlayer.instance.SetTeam("Red");
        Player player = PhotonNetwork.LocalPlayer;
        GetComponent<PhotonView>().RPC("ChangeRed", RpcTarget.AllBuffered, player);
    }

    [PunRPC]
    public void ChangeRed(Player player)
    {
        playerListItems[player].transform.SetParent(redTeamPlayerList);
    }



}
