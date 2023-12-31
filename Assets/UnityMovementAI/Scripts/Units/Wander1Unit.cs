﻿using UnityEngine;
using UnityMovementAI.Scripts.Units.Movement;

namespace UnityMovementAI.Scripts.Units
{
    public class Wander1Unit : MonoBehaviour
    {
        SteeringBasics steeringBasics;
        Wander1 wander;

        void Start()
        {
            steeringBasics = GetComponent<SteeringBasics>();
            wander = GetComponent<Wander1>();
        }

        void FixedUpdate()
        {
            Vector3 accel = wander.GetSteering();

            steeringBasics.Steer(accel);
            steeringBasics.LookWhereYoureGoing();
        }
    }
}