using System;
using System.Collections;
using System.Collections.Generic;
using Cameras;
using PlayerController;
using UnityEngine;
using Utils.Pool;

namespace Attack
{
    public class Attacker : MonoBehaviour
    {
        [SerializeField] private SmoothCamera2D camera2D;
        private ComponentPool<Projectile> projectilePool;
        public Projectile projectile;
        public float damage;
        public float attackSpeed;
        protected float inverseSpeed;
        public float AttackSpeed
        {
            get => attackSpeed;
            set
            {
                attackSpeed = value;
                inverseSpeed = 1f / attackSpeed;
            }
        }
        public float attackRange;
        public LayerMask targetLayer;
        protected bool canAttack = true;
        public event Action OnAttack;
        private readonly List<Projectile> activeProjectiles = new();

        [SerializeField] private AttackType type;

        private void Start()
        {
            projectilePool = new ComponentPool<Projectile>(projectile);
            inverseSpeed = 1f / attackSpeed;
            StartCoroutine(nameof(FindTargetAndAttack));
        }

        protected virtual IEnumerator FindTargetAndAttack()
        {
            while (canAttack)
            {
                var possibleTarget = Physics2D.OverlapCircle(transform.position, attackRange, targetLayer);
                if (possibleTarget)
                {
                    Attack(possibleTarget.transform);
                    yield return new WaitForSeconds(inverseSpeed);
                }

                yield return null;
            }
        }

        // ReSharper disable once ParameterHidesMember
        protected void Attack(Transform target)
        {
            var projToFire = projectilePool.NewItem();
            projToFire.transform.position = transform.position;
            projToFire.Init(target, hit =>
            {
                if (hit.gameObject.activeSelf)
                {
                    projectilePool.DestroyItem(projToFire);
                    hit.SendMessage(nameof(IHittable.Hit), damage);
                    tempForRemoveProj.Add(projToFire);
                }
            });
            activeProjectiles.Add(projToFire);
            OnAttack?.Invoke();
        }

        private readonly List<Projectile> tempForRemoveProj = new();
        private void FixedUpdate()
        {
            if (type == AttackType.Ranged)
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
            }

            foreach (var projToRemove in tempForRemoveProj)
            {
                projectilePool.DestroyItem(projToRemove);
                activeProjectiles.Remove(projToRemove);
            }
            tempForRemoveProj.Clear();
        }
    }

    public enum AttackType
    {
        Melee, Ranged
    }
}
