using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.InputSystem;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Realtime;
using System;


public class UpdatePlayerProperties : MonoBehaviourPunCallbacks
{
    public int kills = 0;
    public int deaths = 0;
    public float myScore = 0;
    public string team = "Red";
    private float setHashCooldown = 0.5f;
    public SpriteRenderer spriteRenderer;
    public List<Material> materials;
    public string materialName = "White";

    private PhotonView view;

    private void Awake()
    {
        view = GetComponent<PhotonView>();
        spriteRenderer = GetComponent<SpriteRenderer>();

    }
    // Start is called before the first frame update
    void Start()
    {
        materials = CustomizePlayer.Materials;
        if (GameSceneManager.instance.mode == "Solo Mode")
        {
            team = view.Owner.ActorNumber.ToString();

            materialName = (string)photonView.Owner.CustomProperties["material"];
            foreach (Material material in materials)
            {
                if (material.name == materialName)
                {
                    spriteRenderer.material = material;
                    break;
                }
            }
        }

        else if (GameSceneManager.instance.mode == "Team Mode")
        {
            if (photonView.Owner.CustomProperties.ContainsKey("team"))
                team = (string)photonView.Owner.CustomProperties["team"];
            else
                team = "Red";
            foreach (Material material in materials)
            {
                if (material.name == team)
                {
                    spriteRenderer.material = material;
                    break;
                }
            }
        }
        GetComponent<PlayerHealth>().team = team;
        GetComponent<Flash>().defaultMat = spriteRenderer.material;
        InvokeRepeating("SetHashes", 1, setHashCooldown);
    }

    void Update()
    {
        if (GameSceneManager.instance.gameStarted)
        {
            if (view.IsMine)
                GetComponent<PlayerController>().enabled = true;
        }
    }

    public void SetHashes()
    {
        if (view.IsMine)
        {
            try
            {
                Hashtable hash = PhotonNetwork.LocalPlayer.CustomProperties;
                hash["score"] = myScore;
                hash["kills"] = kills;
                hash["deaths"] = deaths;
                PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
            }

            catch
            {
            }
        }
    }

    
}
