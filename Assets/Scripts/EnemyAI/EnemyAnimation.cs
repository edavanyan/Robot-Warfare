using DG.Tweening;
using UnityEngine;

namespace EnemyAI
{
    public class EnemyAnimation
    {
        private readonly Animator animator;
        private readonly Transform transform;
        private static readonly int IsWalking = Animator.StringToHash("Walk");
        private static readonly int IsIdle = Animator.StringToHash("Idle");
        private static readonly int IsAttacking = Animator.StringToHash("Attack");
        private static readonly int IsHit = Animator.StringToHash("Hit");
        private static readonly int IsDie = Animator.StringToHash("Die");

        private int currentTrigger;
        

        public EnemyAnimation(Animator animator, Transform transform)
        {
            this.animator = animator;
            this.transform = transform;
        }

        private static int k = 0;
        private Tweener cachedScaleTween = DOTween.To(() => k, t => k = t, 1, 0);
        public void AdjustSpriteRotation(float x)
        {
            if (x != 0)
            {
                cachedScaleTween.Kill(true);
                cachedScaleTween = transform.DOScaleX(Mathf.Sign(x), 0.05f);
            }
        }
        
        public void WalkingAnimation()
        {
            animator.SetBool(IsIdle, false);
            animator.SetTrigger(IsWalking);
            currentTrigger = IsWalking;
        }

        public void IdleAnimation()
        {
            animator.SetBool(IsIdle, true);
            currentTrigger = IsIdle;
        }

        public void AttackAnimation()
        {
            animator.SetTrigger(IsAttacking);
        }

        public void HitAnimation()
        {
            animator.SetTrigger(IsHit);
        }

        public void DieAnimation()
        {
            animator.SetTrigger(IsDie);
        }
    }
}