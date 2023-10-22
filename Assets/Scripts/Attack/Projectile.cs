using Unity.VisualScripting;
using UnityEngine;

namespace Attack
{
    public abstract class Projectile : MonoBehaviour, IPoolable
    {
        protected Transform target;
        public float speed;

        public virtual void Init(Transform target)
        {
            this.target = target;
        }

        public abstract void New();

        public abstract void Free();
    }
}
