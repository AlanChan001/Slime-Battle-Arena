using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class UpdateName : MonoBehaviour
{
    [SerializeField] TextMeshPro nicknameText;
    private PhotonView view;
    // Start is called before the first frame update
    void Start()
    {
        view = GetComponent<PhotonView>();
        nicknameText.text = view.Owner.NickName;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
