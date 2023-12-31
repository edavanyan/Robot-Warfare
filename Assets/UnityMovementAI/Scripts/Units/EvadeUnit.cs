﻿using UnityEngine;
using UnityMovementAI.Scripts.Units.Movement;

namespace UnityMovementAI.Scripts.Units
{
    public class EvadeUnit : MonoBehaviour
    {
        public MovementAIRigidbody target;

        SteeringBasics steeringBasics;
        Evade evade;

        void Start()
        {
            steeringBasics = GetComponent<SteeringBasics>();
            evade = GetComponent<Evade>();
        }

        void FixedUpdate()
        {
            Vector3 accel = evade.GetSteering(target);

            steeringBasics.Steer(accel);
            steeringBasics.LookWhereYoureGoing();
        }
    }
}