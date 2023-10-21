using System;
using UnityEngine;

namespace EnemyAI
{
    public class ArriveBehavior : Steering
    {
        private float targetRadius = 0.5f;
        private float slowRadius = 5f;

        private float inverseRadius;

        public ArriveBehavior(int weight) : base(weight)
        {
            inverseRadius = 1f / 1000f;
        }

        private Color color;
        private Transform target;
        public override SteeringData GetSteering(SteeringBase steeringBase)
        {
            target = steeringBase.target;
            var steering = new SteeringData();
            var direction = (Vector2)(steeringBase.target.position - steeringBase.transform.position);
            var distance = direction.magnitude;
            color = Color.green;
            if (distance < targetRadius)
            {
                
                color = Color.red;
                steeringBase.rigidBody.velocity = Vector2.zero;
                return steering;
            }

            float targetSpeed;
            if (distance > slowRadius)
            {
                
                targetSpeed = steeringBase.maxAcceleration;
            }
            else
            {
                color = Color.yellow;
                targetSpeed = distance * inverseRadius;
            }

            var targetVelocity = direction;
            targetVelocity.Normalize();
            targetVelocity *= targetSpeed;

            steering.linear = targetVelocity - steeringBase.rigidBody.velocity;
            if (steering.linear.magnitude > steeringBase.maxAcceleration)
            {
                color = Color.cyan;
                steering.linear.Normalize();
                steering.linear *= steeringBase.maxAcceleration;
            }

            return steering;
        }
    }
}
