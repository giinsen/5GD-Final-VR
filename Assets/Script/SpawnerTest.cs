using Chronos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerTest : MonoBehaviour
{
    public GameObject cube;
    public GameObject sphere;
    public float delay;
    private float delayTmp;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        delayTmp += GetComponent<Timeline>().deltaTime;
        if (delayTmp >= delay)
        {
            SpawnCube();
            delayTmp = 0;
        }
    }

    public void SpawnCube()
    {
        Instantiate(cube, transform.position, Quaternion.identity);
    }

    public void SpawnSphere()
    {
        Instantiate(sphere, transform.position, Quaternion.identity);
    }
}
