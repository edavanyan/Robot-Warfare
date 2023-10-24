using System;
using System.Collections;
using UnityEngine;

namespace Attack
{
    public class RangedAttacker : Attacker
    {
        private readonly Collider2D[] targets = new Collider2D[20];
        protected override IEnumerator FindTargetAndAttack()
        {
            while (canAttack)
            {
                var count = Physics2D.OverlapCircleNonAlloc(transform.position, attackRange, targets,targetLayer);
                if (count > 0)
                {
                    if (targets.Length > 1)
                    {
                        Array.Sort(targets, ClosestComparison);
                    } 
                    Attack(targets[0].transform);
                    Array.Clear(targets, 0, targets.Length);
                    yield return new WaitForSeconds(attackSpeed);
                }

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
