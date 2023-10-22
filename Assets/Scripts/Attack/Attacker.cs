using System;
using System.Collections;
using System.Collections.Generic;
using Cameras;
using PlayerController;
using UnityEngine;

namespace Attack
{
    public class Attacker : MonoBehaviour
    {
        [SerializeField] private SmoothCamera2D camera2D;
        private ComponentPool<Projectile> projectilePool;
        public Projectile projectile;
        public float damage;
        public float attackSpeed;
        public float attackRange;
        public LayerMask targetLayer;
        private bool canAttack = true;
        private Transform target;
        public event Action OnAttack;
        private List<Projectile> activeProjectiles = new List<Projectile>();

        private void Start()
        {
            projectilePool = new ComponentPool<Projectile>(projectile);
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

        // ReSharper disable once ParameterHidesMember
        private void Attack(Transform target)
        {
            var projToFire = projectilePool.NewItem();
            projToFire.transform.position = transform.position;
            projToFire.Init(target);
            activeProjectiles.Add(projToFire);
            OnAttack?.Invoke();
        }

        private List<Projectile> tempForRemoveProj = new List<Projectile>();
        private void FixedUpdate()
        {
            foreach (var proj in activeProjectiles)
            {
                var boundsMax = camera2D.CameraBoundsMax;
                var boundsMin = camera2D.CameraBoundsMin;
                var projPosition = proj.transform.position;
                if (projPosition.x > boundsMax.x ||
                    projPosition.y > boundsMax.y ||
                    projPosition.x < boundsMin.x ||
                    projPosition.y < boundsMin.y)
                {
                    tempForRemoveProj.Add(proj);
                }
            }

            foreach (var projToRemove in tempForRemoveProj)
            {
                projectilePool.DestroyItem(projToRemove);
                activeProjectiles.Remove(projToRemove);
            }
            tempForRemoveProj.Clear();
        }
    }
}
