using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Attack
{
    public abstract class Projectile : MonoBehaviour, IPoolable
    {
        private Transform target;
        public float speed;
        public event Action<Transform, Projectile> OnTargetHit;
        private string hitTag;

        // ReSharper disable once ParameterHidesMember
        public virtual void Init(Transform target, Action<Transform, Projectile> hitCallback)
        {
            this.target = target;
            hitTag = this.target.tag;
            OnTargetHit += hitCallback;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(hitTag))
            {
                OnTargetHit?.Invoke(other.transform.parent, this);
            }
        }

        public abstract void New();

        public virtual void Free()
        {
            OnTargetHit = null;
        }
    }
}
