using System;
using UnityEngine;

namespace Attack
{
    public class Bullet : Projectile
    {
        private Vector2 direction;
        [SerializeField]
        private Rigidbody2D rigidbody;
        private TrailRenderer trail;

        private void Awake()
        {
            trail = GetComponentInChildren<TrailRenderer>();
        }

        // ReSharper disable once ParameterHidesMember
        public override void Init(Transform target, Action<Transform> hitCallback)
        {
            base.Init(target, hitCallback);
            direction = target.position - transform.position;
            direction.Normalize();
            
            rigidbody.AddForce(direction * speed, ForceMode2D.Impulse);
        }

        public override void New()
        {
            gameObject.SetActive(true);
        }

        public override void Free()
        {
            base.Free();
            rigidbody.velocity = Vector2.zero;
            if (trail)
            {
                trail.Clear();
            }

            gameObject.SetActive(false);
        }
    }
}
