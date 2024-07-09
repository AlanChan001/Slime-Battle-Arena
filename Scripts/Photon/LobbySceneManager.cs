using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using System.Text;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;


public class LobbySceneManager : MonoBehaviourPunCallbacks
{
    public static LobbySceneManager instance;

    public TMP_InputField RoomNameInput;
    public TMP_InputField PlayerNameInput;
    [SerializeField] TextMeshProUGUI textRoomList;
    public string roomToLoad;
    public static string roomMode;

    [Header("UI")] public Transform roomListParent;
    public GameObject roomItemPrefab;

    private List<RoomInfo> cachedRoomList = new List<RoomInfo>();
    Hashtable roomOption = new Hashtable();

    private void Start()
    {
        instance = this;
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("ConnectedToServer");
        base.OnConnectedToMaster();
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        Debug.Log("Join Lobby");
    }

    public void OnClickCreateSolo()
    {
        if (RoomNameInput.text.Length <= 15 && RoomNameInput.text.Length > 0 
            && PlayerNameInput.text.Length <= 15 && PlayerNameInput.text.Length > 0)
        {
            PhotonNetwork.CreateRoom(RoomNameInput.text);
            PhotonNetwork.LocalPlayer.NickName = PlayerNameInput.text;
            roomMode = "Solo Mode";
            roomToLoad = "Solo Mode Waiting Room";

            //roomOption["roomMode"] = "Solo";
            //PhotonNetwork.CurrentRoom.SetCustomProperties(roomOption);
        }
        else
        {
            print("Invalid Room or Player Name!");
        }
    }

    public void OnClickCreateTeam()
    {
        if (RoomNameInput.text.Length <= 15 && RoomNameInput.text.Length > 0
            && PlayerNameInput.text.Length <= 15 && PlayerNameInput.text.Length > 0)
        {
            PhotonNetwork.CreateRoom(RoomNameInput.text);
            PhotonNetwork.LocalPlayer.NickName = PlayerNameInput.text;
            roomMode = "Team Mode";
            roomToLoad = "Team Mode Waiting Room";

            //roomOption["roomMode"] = "Team";
            //PhotonNetwork.CurrentRoom.SetCustomProperties(roomOption);

        }
        else
        {
            print("Invalid Room or Player Name!");
        }
    }

    public void OnClickJoinRoom(string roomName)
    {
        if (PlayerNameInput.text.Length <= 15 && PlayerNameInput.text.Length > 0)
        {
            PhotonNetwork.JoinRoom(roomName);
            PhotonNetwork.LocalPlayer.NickName = PlayerNameInput.text;
        }
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel(roomToLoad);
    }


    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        if (cachedRoomList.Count <= 0)
        {
            cachedRoomList = roomList;
        }
        else
        {
            foreach (var room in roomList)
            {
                for (int i = 0; i < cachedRoomList.Count; i++)
                {
                    if (cachedRoomList[i].Name == room.Name)
                    {
                        List<RoomInfo> newList = cachedRoomList;

                        if (room.RemovedFromList)
                        {
                            newList.Remove(newList[i]);
                        }
                        else
                        {
                            newList[i] = room;
                        }
                        cachedRoomList = newList;

                    }
                }
            }
        }
        UpdateUI();
    }

    void UpdateUI()
    {
        foreach (Transform roomItem in roomListParent)
        {
            Destroy(roomItem.gameObject);
        }

        foreach (var room in cachedRoomList)
        {
            if (room.PlayerCount >= 1)
            {
                GameObject roomItem = Instantiate(roomItemPrefab, roomListParent);
                roomItem.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = room.Name;
                roomItem.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = room.PlayerCount.ToString() + "/16";
            }
        }
    }

    public void SwitchToRegion(string region)
    {
        StartCoroutine(SwitchToRegionCoroutine(region));
    }

    IEnumerator SwitchToRegionCoroutine(string region)
    {
        PhotonNetwork.Disconnect();
        yield return new WaitWhile(() => { return !PhotonNetwork.IsConnected; });
        PhotonNetwork.ConnectToRegion(region);
        yield return new WaitUntil(() => { return PhotonNetwork.IsConnected; });
        PhotonNetwork.JoinLobby();

    }

    public void SwitchToHongKong()
    {
        StartCoroutine(SwitchToRegionCoroutine("hk"));
    }
    public void SwitchToCanada()
    {
        StartCoroutine(SwitchToRegionCoroutine("cae"));
    }

    public void SwitchToEurope()
    {
        StartCoroutine(SwitchToRegionCoroutine("eu"));
    }
}
