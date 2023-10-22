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

        public override void Init(Transform target)
        {
            base.Init(target);
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
            rigidbody.velocity = Vector2.zero;
            trail.Clear();
            gameObject.SetActive(false);
        }
    }
}
