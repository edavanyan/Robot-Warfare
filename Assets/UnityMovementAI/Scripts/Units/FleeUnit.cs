﻿using UnityEngine;
using UnityMovementAI.Scripts.Units.Movement;

namespace UnityMovementAI.Scripts.Units
{
    public class FleeUnit : MonoBehaviour
    {
        public Transform target;

        SteeringBasics steeringBasics;
        Flee flee;

        void Start()
        {
            steeringBasics = GetComponent<SteeringBasics>();
            flee = GetComponent<Flee>();
        }

        void FixedUpdate()
        {
            Vector3 accel = flee.GetSteering(target.position);

            steeringBasics.Steer(accel);
            steeringBasics.LookWhereYoureGoing();
        }
    }
}