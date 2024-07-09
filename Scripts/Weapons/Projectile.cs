using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 22f;
    [SerializeField] private GameObject projectileVFX;
    [SerializeField] private float projectileRange = 10f;
    public PhotonView view;

    private Vector2 startPosition;

    private void Start()
    {
        startPosition = transform.position;
        view = GetComponent<PhotonView>();
    }

    private void Update()
    {
        MoveProjectile();
        DetectFireDistance();
    }

    public void UpdateProjectileRange(float projectileRange)
    {
        this.projectileRange = projectileRange;
    
    }
    public void UpdateMoveSpeed(float moveSpeed)
    {
        this.moveSpeed = moveSpeed;

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerHealth>() && other.GetComponent<PlayerHealth>().team ==
            GetComponent<DamageSource>().damageOwner.GetComponent<PlayerHealth>().team)
        { return; }
        if (other.GetComponent<Indestructible>()|| other.GetComponent<PlayerHealth>()|| other.GetComponent<BaseHealth>())
        {
            Instantiate(projectileVFX, transform.position, transform.rotation);
            if (view.AmOwner)
            PhotonNetwork.Destroy(gameObject);
        }
    }

    private void DetectFireDistance()
    {

         if (Vector2.Distance(transform.position, startPosition) > projectileRange)
            {
                if (view.AmOwner)
                PhotonNetwork.Destroy(gameObject);
            }

    }


    private void MoveProjectile()
    {
        transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
    }
}
