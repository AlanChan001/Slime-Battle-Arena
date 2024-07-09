using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Photon.Pun;

public class Stamina : MonoBehaviour
{
    public float staminaCost = 20 ;
    public float maxStamina = 100;
    public float currentStamina = 50;
    public float staminaRecoveryPerSecond = 2;
    public bool isDashing = false;

    [SerializeField] private TrailRenderer myTrailrenderer;
    [SerializeField] float dashSpeed = 4f;
    [SerializeField] float dashCD = 2f;
    [SerializeField] float dashTime = 0.2f;
    [SerializeField] private Slider staminaSlider;
    [SerializeField] private TMP_Text staminaText;
    [SerializeField] private GameObject staminaBar;

    private bool canRecover = true;
    private PhotonView view;


    private void Awake()
    {
        view = GetComponent<PhotonView>();
    }


    void Start()
    {
        staminaCost = GetComponentInChildren<ActiveWeapon>().staminaCost;
        if (view.IsMine)
        {
            staminaSlider.gameObject.SetActive(true);
            staminaText.gameObject.SetActive(true);
        }
        UpdateCanvasStaminaSlider();
        UpdateStaminaText();
        UpdatePlayerStaminaSlider();
    }

    void Update()
    {
        recoverStamina();
        UpdateCanvasStaminaSlider();
        UpdateStaminaText();
        UpdatePlayerStaminaSlider();
    }

    [PunRPC]
    public void Dash()
    {
            if (!isDashing && currentStamina >= staminaCost)
            {
                isDashing = true;
                //GetComponent<PhotonView>().RPC("consumeStamina", RpcTarget.All,staminaCost);
                consumeStamina(staminaCost);
                myTrailrenderer.emitting = true;
                GetComponent<PlayerController>().moveSpeed *= dashSpeed;
                StartCoroutine(EndDashRoutine());
            }
    }

    public IEnumerator EndDashRoutine()
    {
        yield return new WaitForSeconds(dashTime);
        GetComponent<PlayerController>().moveSpeed /= dashSpeed;
        myTrailrenderer.emitting = false;
        yield return new WaitForSeconds(dashCD - dashTime);
        isDashing = false;
    }

    [PunRPC]
    public void consumeStamina(float staminaCost)
    {

        currentStamina -= staminaCost;
        if (currentStamina < 0)
        {
            currentStamina = 0;
        }
    }

    void recoverStamina()
    {
        if (canRecover)
        {
            currentStamina += staminaRecoveryPerSecond;
            if (currentStamina > maxStamina)
            {
                currentStamina = maxStamina;
            }
            StartCoroutine(recoverStaminaRoutine());
        }
    }

    private IEnumerator recoverStaminaRoutine()
    {
        canRecover = false;
        yield return new WaitForSeconds(1);
        canRecover = true;
    }

    private void UpdateCanvasStaminaSlider()
    {
        staminaSlider.maxValue = maxStamina;
        staminaSlider.minValue = 0;
        staminaSlider.value = currentStamina;
    }

    private void UpdateStaminaText()
    {
        staminaText.text = currentStamina.ToString() + "/" + maxStamina.ToString();
    }

    public void UpdatePlayerStaminaSlider()
    {
        float staminaPercentage = currentStamina / maxStamina;
        staminaBar.transform.localScale = new Vector3(staminaPercentage,
        transform.localScale.y, transform.localScale.z);
    }


}
