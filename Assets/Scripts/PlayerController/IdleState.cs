using System;
using StateMachine;
using UnityEngine;

namespace PlayerController
{
    [CreateAssetMenu(menuName = "States/Character/Idle")]
    public class IdleState : PlayerState
    {
        private Optional<Action> changeState;

        public override void Init(CharacterController parent)
        {
            base.Init(parent);
            changeState ??= new(() =>
            {
                if (runner.Input != Vector2.zero)
                {
                    runner.SetState(typeof(WalkState));
                }
            });
        }

        protected override void PlayStateAnimation()
        {
            animation.IdleAnimation();
        }

        public override Optional<Action> Update()
        {
            return Optional<Action>.Empty;
        }

        public override Optional<Action> FixedUpdate()
        {
            return Optional<Action>.Empty;
        }

        public override Optional<Action> Exit()
        {
            return Optional<Action>.Empty;
        }

        public override Optional<Action> ChangeState()
        {
            return changeState;
        }
    }
}
