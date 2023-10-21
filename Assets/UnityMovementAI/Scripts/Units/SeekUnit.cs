using UnityEngine;
using UnityMovementAI.Scripts.Units.Movement;

namespace UnityMovementAI.Scripts.Units
{
    public class SeekUnit : MonoBehaviour
    {
        public Transform target;

        SteeringBasics steeringBasics;

        void Start()
        {
            steeringBasics = GetComponent<SteeringBasics>();
        }

        void FixedUpdate()
        {
            Vector3 accel = steeringBasics.Seek(target.position);

            steeringBasics.Steer(accel);
            steeringBasics.LookWhereYoureGoing();
        }
    }
}