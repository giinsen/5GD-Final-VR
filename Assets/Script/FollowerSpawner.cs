using Chronos;
using PathCreation;
using PathCreation.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerSpawner : MonoBehaviour
{
    public GameObject follower;
    public PathCreator pathCreator;
    public float delaySpawn;
    private float delaySpawnTmp;

    void Start()
    {
        
    }

    void Update()
    {
        delaySpawnTmp += GameObject.FindObjectOfType<Timeline>().deltaTime;
        delaySpawnTmp = Mathf.Clamp(delaySpawnTmp, 0, Mathf.Infinity);
        if (delaySpawnTmp >= delaySpawn)
        {
            GameObject f = Instantiate(follower);
            f.GetComponent<PathFollower>().pathCreator = pathCreator;
            delaySpawnTmp = 0f;
        }
    }
}
