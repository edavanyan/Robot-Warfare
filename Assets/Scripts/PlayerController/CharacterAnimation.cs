using UnityEngine;

namespace PlayerController
{
    public class CharacterAnimation
    {
        private readonly Animator animator;
        private readonly Transform transform;
        private static readonly int IsWalking = Animator.StringToHash("IsWalking");
        private static readonly int IsIdle = Animator.StringToHash("IsIdle");
        private static readonly int IsAttacking = Animator.StringToHash("IsAttacking");

        private int currentTrigger;
        

        public CharacterAnimation(Animator animator, Transform transform)
        {
            this.animator = animator;
            this.transform = transform;
        }
        public void AdjustSpriteRotation(float xInput)
        {
            if (xInput != 0)
            {
                transform.localScale = new Vector3(Mathf.Sign(xInput), 1, 1);
            }
        }
        public void WalkingAnimation()
        {
            // animator.ResetTrigger(IsIdle);
            animator.SetTrigger(IsWalking);
            currentTrigger = IsWalking;
        }

        public void IdleAnimation()
        {
            // animator.ResetTrigger(IsWalking);
            animator.SetTrigger(IsIdle);
            currentTrigger = IsIdle;
        }

        public void AttackAnimation()
        {
            animator.SetTrigger(IsAttacking);
            
        }
    }
}
