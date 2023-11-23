using System;
using System.Collections;
using EnemyAI;
using UnityEngine;

namespace Attack
{
    public class RangedSplashAttacker : RangedAttacker
    {
        public float splashRadius;
        private Collider2D[] splashTargets = new Collider2D[100];
        [SerializeField] private float splashRadiusPerLevel;

        // ReSharper disable once ParameterHidesMember
        protected override void OnHit(Transform hitTarget, Projectile projectile)
        {
            var count = Physics2D.OverlapCircleNonAlloc(hitTarget.transform.position, splashRadius, splashTargets, targetLayer);
            for (var i = 0; i < count; i++)
            {
                var splashTarget = splashTargets[i].transform.parent;
                if (splashTarget.gameObject.activeSelf)
                {
                    projectile.Damage = Mathf.FloorToInt(damage);
                    projectile.KnockBackForce = 0;
                    splashTarget.SendMessage(nameof(IHittable.Hit), projectile);
                }
            }
            ProjectilePool.DestroyItem(projectile);
            TempForRemoveProj.Add(projectile);
        }
        
        protected override IEnumerator FindTargetAndAttack()
        {
            while (canAttack)
            {
                var target = Physics2D.OverlapCircle(transform.position, 5f,targetLayer);
                if (target)
                {
                    Attack(target.transform);
                    var direction = target.transform.position - transform.position;
                    var angle1 = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    var angle2 = angle1;
                    var extraLevel = level - maxLevel;
                    for (var i = 1; i < Math.Min(level, maxLevel); i++)
                    {
                        yield return new WaitForSeconds(0.05f);
                        angle1 -= 360f / maxLevel;
                        circularTransformParent.rotation = Quaternion.Euler(0, 0, angle1);
                        Attack(circularTransform);
                        if (extraLevel > 0)
                        {
                            extraLevel--;
                            angle2 += 360f / maxLevel;
                            circularTransformParent.rotation = Quaternion.Euler(0, 0, angle2);
                            Attack(circularTransform);
                        }
                    }
                    yield return new WaitForSeconds(InverseSpeed);
                }
                yield return null;
            }
        }

        public override void OnLevelUp(int level)
        {
            splashRadius += splashRadiusPerLevel;
        }
    }
}
