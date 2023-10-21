using System;
using System.Collections;
using UnityEngine;

namespace Attack
{
    public class Attacker : MonoBehaviour
    {
        public Projectile projectile;
        public float attackSpeed;
        public float attackRange;
        public LayerMask targetLayer;
        private bool canAttack = true;
        private Transform target;

        private void Start()
        {
            attackSpeed = 1 / attackSpeed;
            StartCoroutine(nameof(FindTargetAndAttack));
        }

        private IEnumerator FindTargetAndAttack()
        {
            while (canAttack)
            {
                var possibleTarget = Physics2D.OverlapCircle(transform.position, attackRange, targetLayer);
                if (possibleTarget)
                {
                    Attack(possibleTarget.transform);
                    yield return new WaitForSeconds(attackSpeed);
                }

                yield return null;
            }
        }

        private void Attack(Transform target)
        {
            var position = transform.position;
            var projToFire = Instantiate(projectile, position, Quaternion.identity);
            projToFire.Init(target);
        }
    }
}
