using UnityEngine;

namespace EnemyAI
{
    public class SeekBehavior : Steering
    {
        public SeekBehavior(int weight) : base(weight) { }
        
        public override SteeringData GetSteering(Enemy enemy)
        {
            var steering = new SteeringData
            {
                linear = enemy.target.transform.position - enemy.transform.position
            };
            steering.linear.Normalize();
            steering.linear *= enemy.maxAcceleration;
            return steering;
        }
    }
}
