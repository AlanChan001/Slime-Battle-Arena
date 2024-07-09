using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoomItem : MonoBehaviour
{
    public void OnClickJoinRoom()
    {
        LobbySceneManager.instance.OnClickJoinRoom
            (transform.GetChild(1).GetComponent<TextMeshProUGUI>().text);
    }
}
