using System;
using System.Collections;
using UnityEngine;

namespace Attack
{
    public class RangedAttacker : Attacker
    {
        private float startArea = 0f;

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
                    var target = Physics2D.OverlapCircle(transform.position, startArea,targetLayer);
                    if (target)
                    {
                        Attack(target.transform);
                        yield return new WaitForSeconds(inverseSpeed);

                        break;
                    }
                }
                
                startArea = 0f;
                yield return null;
            }
        }

        private int ClosestComparison(Collider2D first, Collider2D second)
        {
            if (!first || !second)
            {
                return 0;
            }

            var position = transform.position;
            var firstDist = (position - first.transform.position).sqrMagnitude;
            var secondDist = (position - second.transform.position).sqrMagnitude;
            if (firstDist > secondDist) return 1;
            if (secondDist > firstDist) return -1;
            return 0;
        }
    }
}
