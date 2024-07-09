using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Realtime;


public class BaseHealth : MonoBehaviourPunCallbacks
{
    public bool IsDead { get; private set; } = false;
    public bool DeadAnimEnded = false;
    public int currentHealth = 2000;
    public string team = "Red";
    [SerializeField] private int maxHealth = 2000;
    [SerializeField] private GameObject baseHPBar;
    [SerializeField] private TMP_Text baseHealthText;
    [SerializeField] private Slider canvasHPSlider;
    [SerializeField] private TextMeshProUGUI canvasHealthText;
    [SerializeField] private TextMeshProUGUI winnerText;
    [SerializeField] private string showText;


    private Flash flash;
    private PhotonView view;

    private void Awake()
    {
        flash = GetComponent<Flash>();
        view = GetComponent<PhotonView>();
    }

    private void Start()
    {
        maxHealth = PhotonNetwork.PlayerList.Length * 200;
        currentHealth = maxHealth;
        UpdateHealthText();
        UpdateBaseHealthSlider();
        UpdateCanvasHealthSlider();

    }

    private void Update()
    {
        UpdateHealthText();
        UpdateBaseHealthSlider();
        UpdateCanvasHealthSlider();
    }

    [PunRPC]
    public void BaseTakeDamage(int damageAmount)
    {
        if (GameSceneManager.instance.gameEnded) { return; }
        StartCoroutine(flash.FlashRoutine());
        currentHealth -= damageAmount;
        if (currentHealth <= 0 && !IsDead)
        {
            baseDestroyed();
        }
    }

    private void baseDestroyed()
    {
        IsDead = true;
        currentHealth = 0;
        GameSceneManager.instance.remainingTime = 0;
        showWinner();
        //GetComponent<Animator>().SetTrigger("Destroyed");
    }

    private void UpdateHealthText()
    {
        canvasHealthText.text = currentHealth.ToString();
        baseHealthText.text = currentHealth.ToString();
    }

    public void UpdateBaseHealthSlider()
    {
        float healthPercentage = (float)currentHealth / maxHealth;
        baseHPBar.transform.localScale = new Vector3(healthPercentage,
        baseHPBar.transform.localScale.y, baseHPBar.transform.localScale.z);
    }

    private void UpdateCanvasHealthSlider()
    {
        canvasHPSlider.maxValue = maxHealth;
        canvasHPSlider.minValue = 0;
        canvasHPSlider.value = currentHealth;
    }

    public void showWinner()
    {
        winnerText.text = showText;
    }

    public void OnDeathAnimEnd()
    {
       
    }


}
