using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bow : MonoBehaviour,IWeapon
{
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform arrowSpawnPoint;
    public float weaponCooldown;
    public int weaponDamage = 2;
    public float weaponRange;
    public float staminaCost;
    public bool playingAttackAnim;


    private Animator myAnimator;

    private void Start()
    {
        myAnimator = GetComponent<Animator>();
        //GetComponent<PhotonAnimatorView>().enabled = true;

    }

    public void Attack()
    {
        myAnimator.SetBool("IsAttacking", true);
        GameObject newArrow = PhotonNetwork.Instantiate(arrowPrefab.name, arrowSpawnPoint.position, transform.rotation);
        newArrow.SetActive(true);
        newArrow.GetComponent<DamageSource>().damageOwner = transform.parent.parent.gameObject; 
        newArrow.GetComponent<Projectile>().UpdateProjectileRange(weaponRange);
    }

    public void DoneAttackingAnimEvent()
    {
        myAnimator.SetBool("IsAttacking", false);
    }

    public bool GetPlayingAttackAnim()
    {
        return playingAttackAnim;
    }

    public float GetWeaponCooldown()
    {
        return weaponCooldown;
    }

    public int GetWeaponDamage()
    {
        return weaponDamage;
    }

    public float GetWeaponRange()
    {
        return weaponRange;
    }

    public float GetStaminaCost()
    {
        return staminaCost;
    }

}
