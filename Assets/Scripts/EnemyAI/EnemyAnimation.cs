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

        private int currentTrigger;
        

        public EnemyAnimation(Animator animator, Transform transform)
        {
            this.animator = animator;
            this.transform = transform;
        }
        
        public void AdjustSpriteRotation(float x)
        {
            if (x != 0)
            {
                transform.localScale = new Vector3(Mathf.Sign(x), 1, 1);
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
    }
}