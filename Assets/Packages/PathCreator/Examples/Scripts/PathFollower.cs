using Chronos;
using System.Collections;
using UnityEngine;

namespace PathCreation.Examples
{
    // Moves along a path at constant speed.
    // Depending on the end of path instruction, will either loop, reverse, or stop at the end of the path.
    public class PathFollower : MonoBehaviour
    {
        public PathCreator pathCreator;
        public EndOfPathInstruction endOfPathInstruction;
        public float speed = 5;
        float distanceTravelled;
        float distanceBeforeRotating;

        public float delay = 3f;
        private float delayTmp = 0f;
        private bool isEjected = false;

        private Rigidbody rb;

        void Start() {
            if (pathCreator != null)
            {
                // Subscribed to the pathUpdated event so that we're notified if the path changes during the game
                pathCreator.pathUpdated += OnPathChanged;
                transform.position = pathCreator.path.GetPointAtDistance(0, endOfPathInstruction);
                transform.rotation = pathCreator.path.GetRotationAtDistance(0, endOfPathInstruction);
            }
            rb = GetComponent<Rigidbody>();
        }

        void Update()
        {
            if (pathCreator == null)
                return;


            distanceTravelled += speed * GetComponent<Timeline>().deltaTime;
            distanceTravelled = Mathf.Clamp(distanceTravelled, 0, Mathf.Infinity);

            distanceBeforeRotating += speed * GetComponent<Timeline>().deltaTime;

            if (distanceTravelled < pathCreator.path.length && GetComponent<Timeline>().timeScale > 0) //avance quand le temps est positif
            {
                distanceBeforeRotating = 0f;
                transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
                transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction) - transform.right*0.25f;

                if (isEjected)
                    ReturnOnPath();
            }
            else if (distanceTravelled >= pathCreator.path.length && !isEjected) //arrivée au bout du chemin
            {
                EjectFromPath();
            }
            else if(distanceTravelled <= 0 && GetComponent<Timeline>().timeScale < 0) //retour et touche le point de spawn
            {
                Destroy(this.gameObject);
            }
        }

        private void EjectFromPath()
        {
            rb.useGravity = true;
            isEjected = true;
            rb.AddForce(transform.forward, ForceMode.Impulse);
            //GetComponent<Collider>().isTrigger = false;
        }

        private void ReturnOnPath()
        {
            rb.useGravity = false;
            isEjected = false;
            //GetComponent<Collider>().isTrigger = true;
        }

        // If the path changes during the game, update the distance travelled so that the follower's position on the new path
        // is as close as possible to its position on the old path
        void OnPathChanged() {
            distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
        }
    }
}