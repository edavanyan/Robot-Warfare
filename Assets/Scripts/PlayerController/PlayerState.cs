using StateMachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerController
{
    public abstract class PlayerState : State<CharacterController>
    {
        protected Rigidbody2D rigidBody;
        protected CharacterAnimation animation;

        public override void Init(CharacterController parent)
        {
            base.Init(parent); 
            rigidBody = parent.RigidBody;
            animation = parent.CharacterAnimation;
            
            PlayStateAnimation();
        }

        public override void CaptureInput(InputAction.CallbackContext context)
        {
            runner.Input = context.ReadValue<Vector2>();
        }

        protected abstract void PlayStateAnimation();
    }
}
