using System;
using System.Collections;
using System.Collections.Generic;
using Cameras;
using UnityEngine;
using Utils.Pool;

namespace Attack
{
    public class Attacker : MonoBehaviour
    {
        private SmoothCamera2D camera2D;
        protected ComponentPool<Projectile> ProjectilePool;
        public Projectile projectile;
        public float damage;
        public float attackSpeed;
        protected float InverseSpeed;
        [SerializeField] private bool showAnimation;
        [SerializeField] protected float knockBackForce = 1.5f;
        [SerializeField] protected float projectileHealth;
        protected int level = 1;
        public int maxLevel;
        public float AttackSpeed
        {
            get => attackSpeed;
            set
            {
                attackSpeed = value;
                InverseSpeed = 1f / attackSpeed;
                attackWaitTime = new WaitForSeconds(InverseSpeed);
            }
        }
        public float attackRange;
        public LayerMask targetLayer;
        public bool canAttack = true;
        public event Action<bool> OnAttack;
        public event Action OnHitEvent;
        private readonly List<Projectile> activeProjectiles = new();
        public AttackType type;

        [SerializeField] private float damagePerLevel;
        [SerializeField] private float attackSpeedPerLevel;
        [SerializeField] private float projectileHealthPerLevel;

        private void Start()
        {
            camera2D = FindObjectOfType<SmoothCamera2D>();
            ProjectilePool = new ComponentPool<Projectile>(projectile);
            InverseSpeed = 1f / attackSpeed;
            attackWaitTime = new WaitForSeconds(InverseSpeed);
        }

        private void OnEnable()
        {
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
                    yield return attackWaitTime;
                }

                yield return null;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }

        // ReSharper disable once ParameterHidesMember
        protected void Attack(Transform target)
        {
            var projToFire = CreateProjectile();
            projToFire.transform.position = transform.position;
            projToFire.Init(target, OnHit);
            activeProjectiles.Add(projToFire);
            OnAttack?.Invoke(showAnimation);
        }

        // ReSharper disable once ParameterHidesMember
        protected virtual void OnHit(Transform hitTarget, Projectile projectile)
        {
            if (hitTarget.gameObject.activeSelf)
            {
                projectile.Damage = Mathf.FloorToInt(damage);
                projectile.KnockBackForce = knockBackForce;
                hitTarget.parent.SendMessage(nameof(IHittable.Hit), projectile);
                if (projectile.IsBroken)
                {
                    DestroyProjectile(projectile);
                }
                OnHitEvent?.Invoke();
            }
        }

        protected Projectile CreateProjectile()
        {
            var item = ProjectilePool.NewItem();
            item.health = (int)projectileHealth;
            return item;
        }

        // ReSharper disable once ParameterHidesMember
        private void DestroyProjectile(Projectile projectile)
        {
            ProjectilePool.DestroyItem(projectile);
            TempForRemoveProj.Add(projectile);
        }

        protected readonly List<Projectile> TempForRemoveProj = new();
        protected WaitForSeconds attackWaitTime;

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
                        TempForRemoveProj.Add(proj);
                    }
                    else
                    {
                        proj.Act();
                    }
                }
            }

            foreach (var projToRemove in TempForRemoveProj)
            {
                ProjectilePool.DestroyItem(projToRemove);
                activeProjectiles.Remove(projToRemove);
            }
            TempForRemoveProj.Clear();
        }

        public virtual void OnLevelUp(int level)
        {
            damage += damagePerLevel;
            AttackSpeed += attackSpeedPerLevel;
            projectileHealth += projectileHealthPerLevel;
        }

        public virtual void Upgrade()
        {
            level++;
        }

        public virtual void ScheduleUpgrade()
        {
            
        }
    }

    public enum AttackType
    {
        Melee, Ranged
    }
}
