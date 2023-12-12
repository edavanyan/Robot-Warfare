using System;
using StateMachine;
using UnityEngine;

namespace PlayerController
{
    [CreateAssetMenu(menuName = "States/Character/Dead")]
    public class DeadState : PlayerState
    {
        public override Optional<Action> Update()
        {
            return Optional<Action>.Empty;
        }

        public override Optional<Action> FixedUpdate()
        {
            return Optional<Action>.Empty;
        }

        public override Optional<Action> ChangeState()
        {
            return Optional<Action>.Empty;
        }

        protected override void PlayStateAnimation()
        {
            animation.DieAnimation();
        }

        public override Optional<Action> Exit()
        {
            return Optional<Action>.Empty;
        }
    }
}
