using System;
using UnityEngine;

namespace EnemyAI
{
    public class ArriveBehavior : Steering
    {
        private float slowRadius = 5f;

        private float inverseRadius;

        public ArriveBehavior(int weight) : base(weight)
        {
            inverseRadius = 1f / 1000f;
        }

        private Color color;
        private Transform target;
        public override SteeringData GetSteering(Enemy enemy)
        {
            target = enemy.target;
            var steering = new SteeringData();
            var direction = (Vector2)(enemy.target.position - enemy.transform.position);
            var distance = direction.magnitude;
            color = Color.green;
            if (distance < enemy.targetRadius)
            {
                var velocity = enemy.rigidBody.velocity;
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                if (Mathf.Sign(direction.x) == Mathf.Sign(velocity.x) &&
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                    Mathf.Sign(direction.y) == Mathf.Sign(velocity.y))
                {
                    enemy.rigidBody.velocity = Vector2.zero;
                }
                color = Color.red;
                return steering;
            }

            float targetSpeed;
            if (distance > slowRadius)
            {
                
                targetSpeed = enemy.maxAcceleration;
            }
            else
            {
                color = Color.yellow;
                targetSpeed = distance * inverseRadius;
            }

            var targetVelocity = direction;
            targetVelocity.Normalize();
            targetVelocity *= targetSpeed;

            steering.linear = targetVelocity - enemy.rigidBody.velocity;
            if (steering.linear.magnitude > enemy.maxAcceleration)
            {
                color = Color.cyan;
                steering.linear.Normalize();
                steering.linear *= enemy.maxAcceleration;
            }

            return steering;
        }
    }
}
