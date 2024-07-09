using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Tilemaps;

public class Pickup : MonoBehaviour
{
    public GameObject heartPrefab;
    [SerializeField] private int healAmount;
    
    private Rigidbody2D rb;
    private PhotonView view;
    private CapsuleCollider2D myCollider;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        view = GetComponent<PhotonView>();
        myCollider = GetComponent<CapsuleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        enableHeart();
        if (!view.IsMine)
        {
            Destroy(rb);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerHealth>())
        {
            other.GetComponent<PhotonView>().RPC("HealPlayer", RpcTarget.All, healAmount);
            if (view.IsMine)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
        if (other.GetComponent<TilemapCollider2D>())
        {
            if (view.IsMine)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }

    }

    private void enableHeart()
    { 
        myCollider.enabled = true;
        spriteRenderer.enabled = true;
    }
}
