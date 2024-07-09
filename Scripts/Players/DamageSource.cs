using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.UtilityScripts;


public class DamageSource : MonoBehaviour
{
    public bool canKnockBack = false;
    public float knockBackThrust;
    public float pushTime;
    public int weaponDamage;
    public float staminaReduce;
    public float weaponCooldown;

    public GameObject damageOwner;

    private PhotonView view;
    public List<GameObject> ImmunePlayers;


    private void Awake()
    {
        view = GetComponent<PhotonView>();    
    }

    private void Start()
    {
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (view.IsMine)
        {
            if (other.GetComponent<BaseHealth>())
            {
                if (other.GetComponent<BaseHealth>().team == damageOwner.GetComponent<PlayerHealth>().team)
                {
                    return;
                }
            }
            if (other.GetComponent<PlayerHealth>())
            {
                if (other.GetComponent<PlayerHealth>().team == damageOwner.GetComponent<PlayerHealth>().team)
                {
                    return;
                }
            }

            foreach (GameObject ImmunePlayer in ImmunePlayers)
            {
                if (other.gameObject == ImmunePlayer) 
                    return;
            }
            ImmunePlayers.Add(other.gameObject);
            if (!GetComponent<Projectile>())
            StartCoroutine(CooldownRoutine());
            if (other.GetComponent<PlayerHealth>()  

                && !other.GetComponent<PlayerHealth>().IsDead)
            {
                damageOwner.GetComponent<UpdatePlayerProperties>().myScore += weaponDamage;
                if (weaponDamage >= other.GetComponent<PlayerHealth>().currentHealth)
                {
                    damageOwner.GetComponent<UpdatePlayerProperties>().kills++;
                    damageOwner.GetComponent<UpdatePlayerProperties>().myScore += 100;
                }
                other.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, weaponDamage);
                damageOwner.GetComponent<UpdatePlayerProperties>().SetHashes();
                //other.GetComponent<PlayerHealth>().TakeDamage(weaponDamage);
            }

            if (other.GetComponent<GettingPushed>())
            {
                if (canKnockBack)
                {
                    other.GetComponent<PhotonView>().RPC("GetPushedBack", RpcTarget.All,
                      (Vector2)transform.position, knockBackThrust, pushTime);
                    //other.GetComponent<GettingPushed>().GetPushedBack((Vector2)transform.position, knockBackThrust, pushTime);
                }
            }
            if (other.GetComponent<Stamina>())
                other.GetComponent<PhotonView>().RPC("consumeStamina", RpcTarget.All, staminaReduce);

            if (other.GetComponent<BaseHealth>()
                && !other.GetComponent<BaseHealth>().IsDead)
            {
                damageOwner.GetComponent<UpdatePlayerProperties>().myScore += weaponDamage;
                if (weaponDamage >= other.GetComponent<BaseHealth>().currentHealth)
                {
                    //damageOwner.GetComponent<UpdatePlayerProperties>().myScore += 100;
                }
                other.GetComponent<PhotonView>().RPC("BaseTakeDamage", RpcTarget.All, weaponDamage);
                damageOwner.GetComponent<UpdatePlayerProperties>().SetHashes();
            }
        }
    }

    private IEnumerator CooldownRoutine()
    {
        yield return new WaitForSeconds(weaponCooldown - 0.05f);
        ImmunePlayers.Clear();
    }

    
    
}
