using System.Collections;
using UnityEngine;

namespace Attack
{
    public class RangedSplashAttacker : RangedAttacker
    {
        public float splashRadius;
        private Collider2D[] splashTargets = new Collider2D[100];

        // ReSharper disable once ParameterHidesMember
        protected override void OnHit(Transform hitTarget, Projectile projectile)
        {
            var count = Physics2D.OverlapCircleNonAlloc(hitTarget.transform.position, splashRadius, splashTargets, targetLayer);
            for (var i = 0; i < count; i++)
            {
                var splashTarget = splashTargets[i].transform.parent;
                if (splashTarget.gameObject.activeSelf)
                {
                    projectile.Damage = damage;
                    projectile.KnockBackForce = 0;
                    splashTarget.SendMessage(nameof(IHittable.Hit), projectile);
                }
            }
            ProjectilePool.DestroyItem(projectile);
            TempForRemoveProj.Add(projectile);
        }
        
        //TODO cinematic staff
        // protected override IEnumerator FindTargetAndAttack()
        // {
        //     while (canAttack)
        //     {
        //         var target = Physics2D.OverlapCircle(transform.position, 5f,targetLayer);
        //         if (target)
        //         {
        //             Attack(target.transform);
        //             yield return new WaitForSeconds(InverseSpeed);
        //         }
        //         yield return null;
        //     }
        // }

        public override void OnLevelUp(int level)
        {
            base.OnLevelUp(level);
            splashRadius += 0.1f;
        }
    }
}
