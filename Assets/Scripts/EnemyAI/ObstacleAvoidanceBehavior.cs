using UnityEngine;

namespace EnemyAI
{
    public class ObstacleAvoidanceBehavior : Steering
    {
        private float avoidDistance = 10f;
        private float lookahead = 0.4f;
        private float sideViewAngle = 36f;//"Enemy", "Ground", "Character", "Player", "Default", "Water", "Ignore Raycast", "UI", "TransparentFX");

        private Vector2[] rayVector = new Vector2[3];
        public static LayerMask CollisionLayer = LayerMask.GetMask("Obstacles");

        public ObstacleAvoidanceBehavior(int weight) : base(weight) { }

        public override SteeringData GetSteering(SteeringBase steeringBase)
        {
            var steering = new SteeringData();
            var velocity = steeringBase.rigidBody.velocity;
            rayVector[0] = velocity;
            rayVector[0].Normalize();
            rayVector[0] *= lookahead;
            var rayOrientation = Mathf.Atan2(velocity.y, velocity.x);
            var rightRayOrientation = rayOrientation + (sideViewAngle * Mathf.Deg2Rad);
            var leftRayOrientation = rayOrientation - (sideViewAngle * Mathf.Deg2Rad);
            rayVector[1] = new Vector2(Mathf.Cos(rightRayOrientation), Mathf.Sin(rightRayOrientation));
            rayVector[1].Normalize();
            rayVector[1] *= lookahead;
            rayVector[2] = new Vector2(Mathf.Cos(leftRayOrientation), Mathf.Sin(leftRayOrientation));
            rayVector[2].Normalize();
            rayVector[2] *= lookahead;
            // rightRayOrientation += (sideViewAngle * Mathf.Deg2Rad);
            // leftRayOrientation -= (sideViewAngle * Mathf.Deg2Rad);
            // rayVector[3] = new Vector2(Mathf.Cos(rightRayOrientation), Mathf.Sin(rightRayOrientation));
            // rayVector[3].Normalize();
            // rayVector[3] *= lookahead;
            // rayVector[4] = new Vector2(Mathf.Cos(leftRayOrientation), Mathf.Sin(leftRayOrientation));
            // rayVector[4].Normalize();
            // rayVector[4] *= lookahead;
            // rightRayOrientation += (sideViewAngle * Mathf.Deg2Rad);
            // leftRayOrientation -= (sideViewAngle * Mathf.Deg2Rad);
            // rayVector[5] = new Vector2(Mathf.Cos(rightRayOrientation), Mathf.Sin(rightRayOrientation));
            // rayVector[5].Normalize();
            // rayVector[5] *= lookahead;
            // rayVector[6] = new Vector2(Mathf.Cos(leftRayOrientation), Mathf.Sin(leftRayOrientation));
            // rayVector[6].Normalize();
            // rayVector[6] *= lookahead;
            // rightRayOrientation += (sideViewAngle * Mathf.Deg2Rad);
            // leftRayOrientation -= (sideViewAngle * Mathf.Deg2Rad);
            // rayVector[7] = new Vector2(Mathf.Cos(rightRayOrientation), Mathf.Sin(rightRayOrientation));
            // rayVector[7].Normalize();
            // rayVector[7] *= lookahead;
            // rayVector[8] = new Vector2(Mathf.Cos(leftRayOrientation), Mathf.Sin(leftRayOrientation));
            // rayVector[8].Normalize();
            // rayVector[8] *= lookahead;   
            // rightRayOrientation += (sideViewAngle * Mathf.Deg2Rad);
            // rayVector[9] = new Vector2(Mathf.Cos(rightRayOrientation), Mathf.Sin(rightRayOrientation));
            // rayVector[9].Normalize();
            // rayVector[9] *= lookahead;

            var position = (Vector2)steeringBase.transform.position;
            foreach (var ray in rayVector)
            {
                var hit = Physics2D.Linecast(position,  position + ray, CollisionLayer);
                if (hit)
                {
                    Debug.DrawRay(steeringBase.transform.position, ray);
                    var target = hit.point + (hit.normal * avoidDistance);
                    steering.linear = target - position;
                    steering.linear.Normalize();
                    steering.linear *= steeringBase.maxAcceleration;
                    break;
                }
            }

            return steering;
        }
    }
}
