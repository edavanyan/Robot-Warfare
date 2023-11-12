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
                        yield return new WaitForSeconds(InverseSpeed);

                        break;
                    }
                }

                startArea = 0f;
                yield return null;
            }
        }
    }
}
