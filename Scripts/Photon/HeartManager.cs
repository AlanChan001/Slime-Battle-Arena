using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class HeartManager : MonoBehaviour
{
    [SerializeField] GameObject heartPrefab;
    public static HeartManager instance;
    [SerializeField] private float smallMapMinX;
    [SerializeField] private float smallMapMaxX;
    [SerializeField] private float smallMapMinY;
    [SerializeField] private float smallMapMaxY;

    [SerializeField] private float largeMapMinX;
    [SerializeField] private float largeMapMaxX;
    [SerializeField] private float largeMapMinY;
    [SerializeField] private float largeMapMaxY;

    Vector2 randomPos;
    private GameObject heart;
    // Start is called before the first frame update
    void Start()
    {

        instance = this;
        //Vector2 randomPos = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
        //heart = PhotonNetwork.Instantiate(heartPrefab.name, randomPos, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        if (heart == null)
        {
            SpawnHeart();
        }
    }

    public void SpawnHeart()
    {
        if (GameSceneManager.instance.SmallMap.activeSelf)
        {
            randomPos = new Vector2(Random.Range(smallMapMinX, smallMapMaxX), Random.Range(smallMapMinY, smallMapMaxY));
        }
        else if (GameSceneManager.instance.LargeMap.activeSelf)
        {
            randomPos = new Vector2(Random.Range(largeMapMinX, largeMapMaxX), Random.Range(largeMapMinY, largeMapMaxY));
        }
        heart = PhotonNetwork.Instantiate(heartPrefab.name, randomPos, Quaternion.identity);
    }

}
