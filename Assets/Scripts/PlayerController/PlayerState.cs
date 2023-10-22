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
            rigidBody = parent.rigidBody;
            animation = parent.CharacterAnimation;
            
            PlayStateAnimation();
            
            GameInput.InputActions.Instance.Game.Movement.performed += CaptureInput;
            GameInput.InputActions.Instance.Game.Movement.canceled += CaptureInput;
            
        }

        public override void CaptureInput(InputAction.CallbackContext context)
        {
            runner.Input = context.ReadValue<Vector2>();
        }

        public abstract void PlayStateAnimation();

        public override void Exit()
        {
            GameInput.InputActions.Instance.Game.Movement.performed -= CaptureInput;
            GameInput.InputActions.Instance.Game.Movement.canceled -= CaptureInput;
        }

        public abstract Vector2 GetPlayerMoveDirection();
    }
}
