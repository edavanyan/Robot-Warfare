using UnityEngine;

namespace Attack
{
    public class Bullet : Projectile
    {
        private Vector2 direction;
        [SerializeField]
        private Rigidbody2D rigidbody;

        public override void Init(Transform target)
        {
            base.Init(target);
            direction = target.position - transform.position;
            direction.Normalize();
            
            rigidbody.AddForce(direction * speed, ForceMode2D.Impulse);
        }
    }
}
