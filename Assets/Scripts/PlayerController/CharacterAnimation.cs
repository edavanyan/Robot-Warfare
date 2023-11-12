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
        private static readonly int IsAttacking = Animator.StringToHash("IsAttack");
        private static readonly int IsHit = Animator.StringToHash("IsHit");

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
                var scale = transform.localScale;
                animator.transform.localScale = new Vector3(-Mathf.Sign(xInput) * scale.y, scale.y, scale.z);
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
            animator.SetTrigger(IsAttacking);
        }

        public void HitAnimation()
        {
            animator.SetTrigger(IsHit);
        }

        public void PreviousAnimation()
        {
            animator.SetTrigger(currentTrigger);
        }
    }
}
