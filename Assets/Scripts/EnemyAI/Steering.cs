using UnityEngine;

namespace EnemyAI
{
    public abstract class Steering
    {
        private float weight;

        protected Steering(float weight = 1)
        {
            this.weight = weight;
        }
        
        public abstract SteeringData GetSteering(SteeringBase steeringBase);

        public float GetWeight()
        {
            return weight;
        }
    }
}
