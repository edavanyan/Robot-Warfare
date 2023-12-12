using UnityEngine;

namespace EnemyAI.SteeringBehaviors
{
    public class ObstacleAvoidanceBehavior : Steering
    {
        private float avoidDistance = 10f;
        private float lookahead = 0.4f;
        private float sideViewAngle = 45f;

        private Vector2[] rayVector = new Vector2[3];
        private static LayerMask CollisionLayer = LayerMask.GetMask("Obstacles");

        public ObstacleAvoidanceBehavior(int weight) : base(weight) { }

        public override SteeringData GetSteering(Enemy enemy)
        {
            var steering = new SteeringData();
            var velocity = enemy.rigidBody.velocity;
            rayVector[0] = velocity;
            rayVector[0].Normalize();
            rayVector[0] *= lookahead;
            
            var rayOrientation = Mathf.Atan2(velocity.y, velocity.x);
            var rightRayOrientation = rayOrientation + (sideViewAngle * Mathf.Deg2Rad);
            rayVector[1] = new Vector2(Mathf.Cos(rightRayOrientation), Mathf.Sin(rightRayOrientation));
            rayVector[1].Normalize();
            rayVector[1] *= lookahead;
            
            var leftRayOrientation = rayOrientation - (sideViewAngle * Mathf.Deg2Rad);
            rayVector[2] = new Vector2(Mathf.Cos(leftRayOrientation), Mathf.Sin(leftRayOrientation));
            rayVector[2].Normalize();
            rayVector[2] *= lookahead;

            var position = (Vector2)enemy.transform.position;
            foreach (var ray in rayVector)
            {
                var hit = Physics2D.Linecast(position,  position + ray, CollisionLayer);
                if (hit)
                {
                    Debug.DrawRay(enemy.transform.position, ray);
                    var target = hit.point + (hit.normal * avoidDistance);
                    steering.linear = target - position;
                    steering.linear.Normalize();
                    steering.linear *= enemy.maxAcceleration;
                    break;
                }
            }

            return steering;
        }
    }
}
