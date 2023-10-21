using UnityEngine;

namespace EnemyAI
{
    public class SeekBehavior : Steering
    {
        public SeekBehavior(int weight) : base(weight) { }
        
        public override SteeringData GetSteering(SteeringBase steeringBase)
        {
            var steering = new SteeringData
            {
                linear = steeringBase.target.transform.position - steeringBase.transform.position
            };
            steering.linear.Normalize();
            steering.linear *= steeringBase.maxAcceleration;
            return steering;
        }
    }
}
