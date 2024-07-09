using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using TMPro;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

public class CustomizePlayer : MonoBehaviour
{
    public static CustomizePlayer instance;

    public string mode;

    public static int weaponIndex = 1;
    public Toggle DaggerCheckBox;
    public Toggle AxeCheckBox;
    public Toggle BowCheckBox;
    public GameObject DaggerPrefab;
    public GameObject AxePrefab;
    public GameObject BowPrefab;
    public GameObject currentActiveWeapon;

    public TMP_Dropdown colorDropdown;
    public List<Material> colorMaterials;
    private List<string> materialNames = new List<string>();
    public static List<Material> Materials;
    [HideInInspector] public int matIndex;
    public string currentMaterialName;

    public TMP_Dropdown teamDropdown;

    private PhotonHashtable hash = new PhotonHashtable();
    public GameObject customizedPlayer;


    private void Awake()
    {
        instance = this;
        //DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        Materials = colorMaterials;
        foreach (Material material in colorMaterials)
        {
            materialNames.Add(material.name);
        
        }
        if (mode == "Solo Mode")
        { 
            colorDropdown.AddOptions(materialNames);
            colorDropdown.RefreshShownValue(); 
        }
    }

    public void OnClickSelectWeapon()
    {
        if (DaggerCheckBox.isOn)
        {
            weaponIndex = 1;
        }
        else if (AxeCheckBox.isOn)
        {
            weaponIndex = 2;

        }
        else if (BowCheckBox.isOn)
        {
            weaponIndex = 3;
        }
    }

    public void SetColor(int index)
    {
        matIndex = index;
        currentMaterialName = materialNames[index];
        if (PhotonNetwork.IsConnected)
        {
            SoloModeSetHash();
        }
    }

    private void SoloModeSetHash()
    {
        hash["material"] = currentMaterialName;
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }

    public void SetTeam(string team)
    {
        if (PhotonNetwork.IsConnected)
        {
            hash["material"] = team;
            hash["team"] = team;
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }
    }


}
