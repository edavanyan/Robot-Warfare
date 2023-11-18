using System;
using UnityEngine;

namespace Attack
{
    public class Bullet : Projectile
    {
        private TrailRenderer trail;

        private void Awake()
        {
            trail = GetComponentInChildren<TrailRenderer>();
        }

        // ReSharper disable once ParameterHidesMember
        public override void Init(Transform target, Action<Transform, Projectile> hitCallback)
        {
            base.Init(target, hitCallback);
            FireOnTarget(speed);
        }

        public override void Act()
        {
            var velocity = rigidbody.velocity;
            rigidbody.rotation = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg - 45f;
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
