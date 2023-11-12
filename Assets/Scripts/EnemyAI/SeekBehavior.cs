using PlayerController;
using UnityEngine;

namespace EnemyAI
{
    public class SeekBehavior : Steering
    {
        public SeekBehavior(int weight) : base(weight) { }
        
        public override SteeringData GetSteering(Enemy enemy)
        {
            var target = API.PlayerCharacter.transform;
            var steering = new SteeringData
            {
                linear = target.transform.position - enemy.transform.position
            };
            steering.linear.Normalize();
            steering.linear *= enemy.maxAcceleration;
            return steering;
        }
    }
}
