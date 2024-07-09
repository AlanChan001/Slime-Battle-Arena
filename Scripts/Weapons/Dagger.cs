using System.Collections;
using UnityEngine;
using Photon.Pun;
public class Dagger : MonoBehaviour, IWeapon
{
    [SerializeField] private BoxCollider2D weaponCollider;
    public float weaponCooldown;
    public int weaponDamage;
    public float weaponRange;
    public float staminaCost;
    public bool playingAttackAnim;

    private Animator myAnimator;

    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
    }

    private void Start()
    {
        //GetComponent<PhotonAnimatorView>().enabled = true;
    }


    public void Attack()
    {
        weaponCollider.enabled = true;
        myAnimator.SetBool("IsAttacking",true);
        playingAttackAnim = true;
    }

    public void DoneAttackingAnimEvent()
    {
        myAnimator.SetBool("IsAttacking", false) ;
        weaponCollider.enabled = false;
        playingAttackAnim = false;
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
