using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Attack
{
    public abstract class Projectile : MonoBehaviour, IPoolable
    {
        [SerializeField]
        protected new Rigidbody2D rigidbody;
        private Transform target;
        public float speed;
        public event Action<Transform, Projectile> OnTargetHit;
        protected string HitTag;
        public int Damage { get; set; }
        public float KnockBackForce { get; set; }
        [FormerlySerializedAs("Health")] [SerializeField] protected int health;
        public bool IsBroken => currentHealth <= 0;
        private int currentHealth = 0;

        private void Awake()
        {
            currentHealth = health;
        }

        // ReSharper disable once ParameterHidesMember
        public virtual void Init(Transform target, Action<Transform, Projectile> hitCallback)
        {
            this.target = target;
            HitTag = this.target.tag;
            OnTargetHit += hitCallback;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(HitTag))
            {
                TargetHit(other);
            }
        }

        protected void TargetHit(Collider2D other)
        {
            currentHealth--;
            OnTargetHit?.Invoke(other.transform, this);
        }

        protected void FireOnTarget(float force)
        {
            var direction = target.position - transform.position;
            direction.Normalize();
            
            rigidbody.AddForce(direction * force, ForceMode2D.Impulse);
        }

        public abstract void New();

        public virtual void Free()
        {
            currentHealth = health;
            OnTargetHit = null;
        }
    }
}
