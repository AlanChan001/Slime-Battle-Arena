using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class ActiveWeapon : MonoBehaviour
{
    public MonoBehaviour CurrentActiveWeapon; 
    //{ get; private set; }
    private PlayerControls playerControls;

    public FixedJoystick attackJoystick;
    public float weaponCooldown;
    public float weaponRange;
    public float staminaCost;
    public bool attackButtonDown;
    public bool isAttacking = false;

    private PhotonView view;


    private void Awake()
    {
        NewWeapon();
        playerControls = new PlayerControls();
        view = GetComponentInParent<PhotonView>();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }
    // Start is called before the first frame update
    private void Start()
    {
        playerControls.Combat.Attack.started += _ => StartAttacking(); 
        playerControls.Combat.Attack.canceled += _ => StopAttacking();
        StartCoroutine(AttackCooldownRoutine());
    }

    private void Update()
    {
        if (view.IsMine && !GetComponentInParent<PlayerHealth>().IsDead)
        {
            Attack();
        }
    }

    
    private void Attack()
    {
        if (!isAttacking && CurrentActiveWeapon != null)
        {
                if (GetComponentInParent<PlayerController>().SelectedPlayerInRange() || attackButtonDown|| attackJoystick.Direction != Vector2.zero)
                {
                    StopAllCoroutines();
                    (CurrentActiveWeapon as IWeapon).Attack();
                    StartCoroutine(AttackCooldownRoutine());
                }
        }
    }

    private IEnumerator AttackCooldownRoutine()
    {
        isAttacking = true;
        yield return new WaitForSeconds(weaponCooldown);
        isAttacking = false;
    }

    public void NewWeapon()
    {
        //CurrentActiveWeapon = transform.GetChild(0).GetComponent<MonoBehaviour>();
        weaponCooldown = (CurrentActiveWeapon as IWeapon).GetWeaponCooldown();
        weaponRange = (CurrentActiveWeapon as IWeapon).GetWeaponRange();
        staminaCost = (CurrentActiveWeapon as IWeapon).GetStaminaCost();
        StartCoroutine(AttackCooldownRoutine());
    }

    private void StartAttacking()
    {
        attackButtonDown = true;
    }

    private void StopAttacking()
    {
        attackButtonDown = false;
    }

}