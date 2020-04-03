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
        Spawn();
    }

    void Update()
    {
        delaySpawnTmp += GetComponent<Timeline>().deltaTime;
        delaySpawnTmp = Mathf.Clamp(delaySpawnTmp, 0, Mathf.Infinity);
        if (delaySpawnTmp >= delaySpawn)
        {
            Spawn();
            delaySpawnTmp = 0f;
        }
    }

    private void Spawn()
    {
        GameObject f = Instantiate(follower);
        f.GetComponent<Follower>().pathCreator = pathCreator;
    }
}
