using System;
using UnityEngine;

namespace Attack
{
    public class Bullet : Projectile
    {
        private Vector2 direction;
        [SerializeField]
        private new Rigidbody2D rigidbody;
        private TrailRenderer trail;

        private void Awake()
        {
            trail = GetComponentInChildren<TrailRenderer>();
        }

        // ReSharper disable once ParameterHidesMember
        public override void Init(Transform target, Action<Transform, Projectile> hitCallback)
        {
            base.Init(target, hitCallback);
            direction = target.position - transform.position;
            direction.Normalize();
            
            rigidbody.AddForce(direction * speed, ForceMode2D.Impulse);
        }

        private void FixedUpdate()
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
