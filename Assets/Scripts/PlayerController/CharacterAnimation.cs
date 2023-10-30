using UnityEngine;

namespace PlayerController
{
    public class CharacterAnimation
    {
        private readonly Animator animator;
        private readonly Transform transform;
        private static readonly int IsWalkingHorizontal = Animator.StringToHash("IsWalkingHorizontal");
        private static readonly int IsWalkingVertical = Animator.StringToHash("IsWalkingVertical");
        private static readonly int IsIdle = Animator.StringToHash("IsIdle");
        private static readonly int IsRun = Animator.StringToHash("IsRun");
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
        public void WalkingHorizontalAnimation()
        {
            if (currentTrigger == IsWalkingHorizontal)
            {
                return;
            }
            animator.SetTrigger(IsWalkingHorizontal);
            currentTrigger = IsWalkingHorizontal;
        }
        public void WalkingVerticalAnimation()
        {
            if (currentTrigger == IsWalkingVertical)
            {
                return;
            }
            animator.SetTrigger(IsWalkingVertical);
            currentTrigger = IsWalkingVertical;
        }
        public void RunAnimation()
        {
            if (currentTrigger == IsRun)
            {
                return;
            }
            animator.SetTrigger(IsRun);
            currentTrigger = IsRun;
        }

        public void IdleAnimation()
        {
            if (currentTrigger == IsIdle)
            {
                return;
            }
            animator.SetTrigger(IsIdle);
            currentTrigger = IsIdle;
        }

        public void AttackAnimation()
        {
            animator.ResetTrigger(currentTrigger);
            animator.SetTrigger(IsAttacking);
        }

        public void PreviousAnimation()
        {
            animator.SetTrigger(currentTrigger);
        }
    }
}
