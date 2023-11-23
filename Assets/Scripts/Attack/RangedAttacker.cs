using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Attack
{
    public class RangedAttacker : Attacker
    {
        private float startArea = 0f;
        
        //cinematic staff
        public Transform circularTransform;
        public Transform circularTransformParent;

        protected override IEnumerator FindTargetAndAttack()
        {
            while (canAttack)
            {
                while (startArea < attackRange)
                {
                    startArea += 0.5f;
                    if (startArea > attackRange)
                    {
                        startArea = attackRange;
                    }

                    var target = Physics2D.OverlapCircle(transform.position, startArea, targetLayer);
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

                        break;
                    }
                }

                startArea = 0f;
                yield return null;
            }
        }

        public override void OnLevelUp(int level)
        {
            damage += 1;
            AttackSpeed += 0.075f;
            projectileHealth += 0.1f;
        }
    }
}
