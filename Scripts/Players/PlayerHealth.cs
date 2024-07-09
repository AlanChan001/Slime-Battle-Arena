using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Realtime;


public class PlayerHealth : MonoBehaviourPunCallbacks
{
    public bool IsDead = false;
    public bool DeadAnimEnded = false;


    public string team = "Red";
    [SerializeField] private int maxHealth = 5;
    public int currentHealth;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private GameObject HPBar;
    [SerializeField] private float respawnTime;
    [SerializeField] private float respawnCounter;
    [SerializeField] private TextMeshProUGUI respawnTimer;

    public List<Transform> SpawnPos = new List<Transform>();
    public Vector2 randomPosition;

    public Camera cam;

    private Flash flash;
    private PhotonView view;
    private Animator myAnim;

    private void Awake()
    {
        flash = GetComponent<Flash>();
        view = GetComponent<PhotonView>();
        myAnim = GetComponent<Animator>();
    }

    private void Start()
    {
        if (view.IsMine)
        {
            cam.gameObject.SetActive(true);
            healthSlider.gameObject.SetActive(true);
            healthText.gameObject.SetActive(true);
        }
        UpdateCanvasHealthSlider();
        UpdateHealthText();
        UpdatePlayerHealthSlider();
        respawnTime = GameSceneManager.instance.respawnTime;
    }

    private void Update()
    {
        if (respawnCounter > 0)
        {
            respawnTimer.text = Mathf.FloorToInt(respawnCounter).ToString();
            respawnCounter -= Time.deltaTime;
        }
        UpdateCanvasHealthSlider();
        UpdateHealthText();
        UpdatePlayerHealthSlider();
    }

    [PunRPC]
    public void HealPlayer(int healAmount)
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += healAmount;
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
        }
    }

    [PunRPC]
    public void TakeDamage(int damageAmount)
    {
        if (GameSceneManager.instance.gameEnded) { return; }
        StartCoroutine(flash.FlashRoutine());
        //Hashtable table = new Hashtable();
        currentHealth -= damageAmount;
        //table.Add("currentHealth", currentHealth);
        //PhotonNetwork.LocalPlayer.SetCustomProperties(table);
        if (currentHealth <= 0 && !IsDead)
        {

            PlayerDeath();
        }
    }

    /*
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (targetPlayer == view.Owner)
        {
            currentHealth = (float) changedProps["currentHealth"];
            print(targetPlayer.NickName + ": " + currentHealth.ToString());
        }
    }
    */

    private void PlayerDeath()
    {


        GetComponent<UpdatePlayerProperties>().deaths++;
        GetComponent<UpdatePlayerProperties>().myScore -= 100;
        GetComponent<UpdatePlayerProperties>().SetHashes();
        IsDead = true;
        currentHealth = 0;
        myAnim.SetBool("IsDead", true);
        StartCoroutine(respawnRoutine());
    }



    private void UpdateCanvasHealthSlider()
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.minValue = 0;
        healthSlider.value = currentHealth;
    }

    private void UpdateHealthText()
    {
        healthText.text = currentHealth.ToString() + "/" + maxHealth.ToString();
    }

    public void UpdatePlayerHealthSlider()
    {
        float healthPercentage = (float) currentHealth / maxHealth;
        HPBar.transform.localScale = new Vector3(healthPercentage,
        transform.localScale.y, transform.localScale.z);
    }

    [PunRPC]
    public void Respawn()
    {
        SpawnPos = GameSceneManager.instance.SpawnPos;
        int i = Random.Range(0, SpawnPos.Count);
        randomPosition = SpawnPos[i].position;
       
        respawnTimer.gameObject.SetActive(false);
        transform.position = randomPosition;
        currentHealth = maxHealth;
    }

    private IEnumerator respawnRoutine()
    {
        respawnTimer.gameObject.SetActive(true);
        respawnCounter = respawnTime;
        yield return new WaitForSeconds(respawnTime);
        myAnim.SetBool("IsDead", false);
        GetComponent<PhotonView>().RPC("Respawn", RpcTarget.All);
        yield return new WaitForSeconds(1);
        IsDead = false;

    }



}
