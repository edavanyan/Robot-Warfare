using System;
using UnityEngine;

namespace EnemyAI
{
    public class SteeringBase : MonoBehaviour
    {
        public Rigidbody2D rigidBody;
        public Transform target;
        private readonly Steering[] steeringList = new Steering[2];
        public float maxAcceleration = 3f;
        public float maxSpeed = 1f;
        public float drag = 1f;

        private void Start()
        {
            rigidBody = GetComponent<Rigidbody2D>();
            
            InitializeSteeringList();
        }

        private void InitializeSteeringList()
        {
            steeringList[0] = new SeekBehavior(3);
            // steeringList[1] = new ObstacleAvoidanceBehavior(3);
            steeringList[1] = new ArriveBehavior(1);
            
        }

        private void FixedUpdate()
        {
            var acceleration = Vector2.zero;
            foreach (var steering in steeringList)
            {
                var steeringData = steering.GetSteering(this);
                acceleration += steeringData.linear * steering.GetWeight();
            }
            if (acceleration.magnitude > maxSpeed)
            {
                acceleration.Normalize();
                acceleration *= maxSpeed;
            }
            rigidBody.AddForce(acceleration);
            transform.localScale = new Vector3(Mathf.Sign(target.position.x - transform.position.x), 1, 1);
        }
    }
}
