using UnityEngine;

namespace EnemyAI.SteeringBehaviors
{
    public class SeparationBehavior : Steering
    {
        private float threshold = 2f;
        private Collider2D[] targets = new Collider2D[10];
        public static LayerMask CollisionLayer = LayerMask.GetMask("Enemy");
        
        public SeparationBehavior(int weight) : base(weight)
        {
        }
        
        public override SteeringData GetSteering(Enemy enemy)
        {
            SteeringData steering = new SteeringData();
            var targetCount = Physics2D.OverlapCircleNonAlloc((Vector2)enemy.transform.position, 0.5f, targets, CollisionLayer);
            for (var i = 0; i < targetCount; i++)
            {
                var target = targets[i].transform;
                Vector2 direction = (Vector2)target.transform.position - (Vector2)enemy.transform.position;
                float distance = direction.magnitude;
                if (distance < threshold)
                {
                    float strength = -enemy.maxAcceleration;
                    direction.Normalize();
                    steering.linear += strength * direction;
                }
            }

            return steering;
        }
    }
}
