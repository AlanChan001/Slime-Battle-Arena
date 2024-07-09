using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFollow : MonoBehaviour
{
    [SerializeField] private Camera cam;

    private void Awake()
    {

    }


    private void Update()
    {

    }

    public void FaceMouse()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 playerPosition = cam.WorldToScreenPoint(transform.position);
        Vector2 direction = mousePosition - playerPosition;
        transform.right = direction;
    }

    public void FaceEnemy(GameObject enemy)
    {
        Vector3 playerPosition = cam.WorldToScreenPoint(transform.position);
        Vector2 direction = cam.WorldToScreenPoint(enemy.transform.position) - playerPosition;
        transform.right = direction;
    }

    public void FaceMobileCursor(GameObject MobileCursor)
    {
        transform.right = MobileCursor.transform.localPosition;
    }
}
