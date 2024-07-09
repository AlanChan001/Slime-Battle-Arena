using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GettingPushed : MonoBehaviour
{
    public bool beingKnockedBack;
    private Vector2 difference = Vector2.zero;
    private PhotonView view;
    private Rigidbody2D rb;

    private void Awake()
    {
        view = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        /*if (beingPushed)
        {
            timePassed += Time.deltaTime;
            alphaT = timePassed / totalPushTime;
            alphaT = Mathf.Clamp(alphaT, 0, 1);
            transform.position = Vector2.Lerp(originalPosition, targetPos, alphaT);
            
        }
        */
    }

    /*
    [PunRPC]
    public void GetPushedBack(Vector2 damageSourcePos, float pushDistance, float pushTime)
    {
        if (view.IsMine)
        {
            beingPushed = true;
            pushDir = ((Vector2)transform.position - damageSourcePos).normalized * pushDistance;
            originalPosition = transform.position;
            targetPos = originalPosition + pushDir;
            totalPushTime = pushTime;
            timePassed = 0f;
            StartCoroutine(PushRoutine(pushTime));
        }
    }
    */

    [PunRPC]
    public void GetPushedBack(Vector2 damageSourcePos, float knockBackThrust, float pushTime)
    {
        if (view.IsMine)
        {
            beingKnockedBack = true;
            difference = ((Vector2)transform.position - damageSourcePos).normalized * knockBackThrust * rb.mass;
            rb.AddForce(difference, ForceMode2D.Impulse);
            Debug.Log("hit");

            StartCoroutine(KnockBackRoutine(pushTime));
        }
    }

    private IEnumerator KnockBackRoutine(float knockBackTime)
    {
        yield return new WaitForSeconds(knockBackTime);
        beingKnockedBack = false;
        rb.velocity = Vector2.zero;
    }

}
