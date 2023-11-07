using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Attack
{
    public class RangedAttacker : Attacker
    {
        private float startArea = 0f;
        public Transform circularTransform;
        public Transform circularTransformParent;

        private void Awake()
        {
            var scaleTime = 1f;
            circularTransformParent = circularTransform.parent;
            DOTween.Sequence()
                .Append(circularTransformParent.DORotateQuaternion(
                    Quaternion.Euler(0, 0, 179),
                    scaleTime).SetRelative(true))
                .Append(circularTransformParent.DORotateQuaternion(
                    Quaternion.Euler(0, 0, 179),
                    scaleTime).SetRelative(true))
                .SetLoops(int.MaxValue);
        }

        protected override IEnumerator FindTargetAndAttack()
        {
            //TODO cinematic staff
            while (canAttack)
            {
            //     while (startArea < attackRange)
            //     {
            //         startArea += 0.5f;
            //         if (startArea > attackRange)
            //         {
            //             startArea = attackRange;
            //         }
            //
            //         var target = Physics2D.OverlapCircle(transform.position, startArea,targetLayer);
            //         if (target)
            //         {
            //             Attack(target.transform);
            //             yield return new WaitForSeconds(InverseSpeed);
            //
            //             break;
            //         }
            //     }
            //     
            //     startArea = 0f;
            //     yield return null;
            
                Attack(circularTransform);
                yield return new WaitForSeconds(InverseSpeed);
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

        public override void OnLevelUp(int level)
        {
            AttackSpeed += 0.1f;
        }
    }
}
