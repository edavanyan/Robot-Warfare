using UnityEngine;

namespace Attack
{
    public abstract class Projectile : MonoBehaviour
    {
        protected Transform target;
        public float speed;

        public virtual void Init(Transform target)
        {
            this.target = target;
        }
    }
}
