using PathCreation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathObject : MonoBehaviour
{
    public float ejectForce;
    public GameObject portal;

    private EndOfPathInstruction endOfPathInstruction = EndOfPathInstruction.Stop;
    private PathCreator pathCreator;
    void Start()
    {
        pathCreator = GetComponent<PathCreator>();
        Vector3 portalPosition = pathCreator.path.GetPointAtDistance(pathCreator.path.length, endOfPathInstruction);
        Quaternion portalRotation = pathCreator.path.GetRotationAtDistance(pathCreator.path.length, endOfPathInstruction);
        Instantiate(portal, portalPosition, portalRotation, transform);
    }

    void Update()
    {
        
    }

    public void RemoveFollowers()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i) != null && transform.GetChild(i).GetComponent<Follower>() != null)
            {
                transform.GetChild(i).GetComponent<Follower>().Remove();
            }
        }
    }
}
