using Chronos;
using PathCreation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    public PathCreator pathCreator;
    public EndOfPathInstruction endOfPathInstruction;
    public float speed = 5;
    float distanceTravelled;
    float distanceBeforeRotating;

    private bool isEjected = false;
    private float distanceBeforeEjection = 0f;

    private Rigidbody rb;
    private Timeline timeline;
    private PathObject pathObject;

    void Start()
    {
        if (pathCreator != null)
        {
            // Subscribed to the pathUpdated event so that we're notified if the path changes during the game
            pathCreator.pathUpdated += OnPathChanged;
            transform.position = pathCreator.path.GetPointAtDistance(0, endOfPathInstruction);
            transform.rotation = pathCreator.path.GetRotationAtDistance(0, endOfPathInstruction);
        }
        rb = GetComponent<Rigidbody>();
        timeline = GetComponent<Timeline>();
        pathObject = pathCreator.GetComponent<PathObject>();
    }

    void Update()
    {
        if (pathCreator == null)
            return;


        distanceTravelled += speed * timeline.deltaTime;
        distanceTravelled = Mathf.Clamp(distanceTravelled, 0, Mathf.Infinity);

        distanceBeforeRotating += speed * timeline.deltaTime;

        if (distanceTravelled < pathCreator.path.length && timeline.timeScale > 0 && !isEjected) //avance quand le temps est positif et pas éjecté
        {
            distanceBeforeRotating = 0f;
            transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
            transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction) - transform.right * 0.25f;    
        }
        else if (distanceTravelled < distanceBeforeEjection && timeline.timeScale < 0 && isEjected) //le temps recule et arrive avant son point de collision
        {
            Debug.Log("return on path");
            ReturnOnPath();
        }
        else if (distanceTravelled >= pathCreator.path.length && !isEjected) //arrivée au bout du chemin
        {
            EjectFromPath();
        }
        else if (distanceTravelled <= 0 && timeline.timeScale < 0) //retour et touche le point de spawn
        {
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 8) return; //path
        EjectFromPath();
    }

    private void EjectFromPath()
    {
        if (!isEjected && rb != null)
        {
            rb.useGravity = true;
            rb.AddForce(transform.forward * pathObject.ejectForce, ForceMode.Impulse);
            distanceBeforeEjection = distanceTravelled;
            isEjected = true;
        }        
    }

    private void ReturnOnPath()
    {
        if (isEjected)
        {
            rb.useGravity = false;
            isEjected = false;
        }        
    }

    // If the path changes during the game, update the distance travelled so that the follower's position on the new path
    // is as close as possible to its position on the old path
    void OnPathChanged()
    {
        distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
    }
}
