using DG.Tweening;
using Manager;
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
        private static readonly int IsDie = Animator.StringToHash("IsDie");
        private readonly SpriteRenderer spriteRenderer;

        public CharacterAnimation(Animator animator, Transform transform)
        {
            this.animator = animator;
            this.transform = transform;
            this.spriteRenderer = animator.GetComponent<SpriteRenderer>();
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
            spriteRenderer.DOComplete();
            spriteRenderer.DOColor(Color.red, 0.1f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.OutSine).OnComplete(() =>
            {
                spriteRenderer.color = Color.white;
            });
        }

        public void DieAnimation()
        {
            animator.SetTrigger(IsDie);
        }

        public void LevelUpAnimation()
        {
            API.VfxManager.LevelUpEffect();
            spriteRenderer.DOComplete();
            DOTween.Sequence()
                .Append(spriteRenderer.DOColor(Color.cyan, 0.2f).SetLoops(6, LoopType.Yoyo)
                    .SetEase(Ease.OutSine))
                .Join(transform.DOScale(new Vector3(1.05f, 1.05f, 1), 0.2f).SetLoops(6, LoopType.Yoyo))
                .OnComplete(() =>
                {
                    spriteRenderer.color = Color.white;
                    transform.localScale = Vector3.one;
                });
            
        }
    }
}
