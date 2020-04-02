using Chronos;
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

        void Start() {
            if (pathCreator != null)
            {
                // Subscribed to the pathUpdated event so that we're notified if the path changes during the game
                pathCreator.pathUpdated += OnPathChanged;
                transform.position = pathCreator.path.GetPointAtDistance(0, endOfPathInstruction);
                transform.rotation = pathCreator.path.GetRotationAtDistance(0, endOfPathInstruction);
            }
        }

        void Update()
        {
            if (pathCreator == null)
                return;


            distanceTravelled += speed * GetComponent<Timeline>().deltaTime;
            distanceTravelled = Mathf.Clamp(distanceTravelled, 0, Mathf.Infinity);

            distanceBeforeRotating += speed * GetComponent<Timeline>().deltaTime;

            if (distanceTravelled < pathCreator.path.length && GetComponent<Timeline>().timeScale > 0)// && distanceBeforeRotating >= 0.5f)
            {
                distanceBeforeRotating = 0f;
                transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
                transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction) - transform.right*0.5f;
                isEjected = false;
                //GetComponent<Rigidbody>().AddTorque(pathCreator.path.GetNormalAtDistance(distanceTravelled + 0.25f), ForceMode.Impulse);
            }
            if (distanceTravelled >= pathCreator.path.length && !isEjected)
            {
                Debug.Log("ejection");
                isEjected = true;
                GetComponent<Rigidbody>().useGravity = true;
                GetComponent<Rigidbody>().AddForce(transform.forward * 5f, ForceMode.Impulse);
            }
        }


        // If the path changes during the game, update the distance travelled so that the follower's position on the new path
        // is as close as possible to its position on the old path
        void OnPathChanged() {
            distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
        }
    }
}