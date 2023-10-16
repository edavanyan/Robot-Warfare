using StateMachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerController
{
    [CreateAssetMenu(menuName = "States/Character/Idle")]
    public class IdleState : PlayerState
    {

        protected override void PlayStateAnimation()
        {
            animation.IdleAnimation();
        }

        public override Vector2 GetPlayerMoveDirection()
        {
            return Vector2.zero;
        }

        public override void Update()
        {
        }

        public override void FixedUpdate()
        {
        }

        public override void ChangeState()
        {
            if (runner.Input != Vector2.zero)
            {
                runner.SetState(typeof(WalkState));
            }
        }
    }
}
