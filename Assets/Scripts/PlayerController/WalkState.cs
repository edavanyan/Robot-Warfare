using StateMachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerController
{
    [CreateAssetMenu(menuName = "States/Character/Walk")]
    public class WalkState : PlayerState
    {

        [SerializeField]
        private float speed = 5f;

        private float currentSpeed = 0;
        private Vector2 moveDirection = Vector2.zero;
        [SerializeField]
        private float acceleration = 50f;
        [SerializeField]
        private float deAcceleration = 100f;

        public override void Init(CharacterController parent)
        {
            base.Init(parent);
            currentSpeed = 0;
        }

        protected override void PlayStateAnimation()
        {
            animation.WalkingAnimation();
        }

        public override Vector2 GetPlayerMoveDirection()
        {
            return moveDirection * currentSpeed;
        }

        public override void Update()
        {
            if (runner.Input.sqrMagnitude > 0 && currentSpeed >= 0)
            {
                moveDirection = runner.Input;
                currentSpeed += acceleration * speed * Time.deltaTime;
            }
            else
            {
                currentSpeed -= deAcceleration * speed * Time.deltaTime;
            }
            currentSpeed = Mathf.Clamp(currentSpeed, 0, speed);
        }

        public override void FixedUpdate()
        {
            var position = rigidbody.position;
            var destination = position + currentSpeed * Time.fixedDeltaTime * moveDirection;

            // var collision = Physics2D.OverlapCircle(position + runner.CircleCollider2D.offset, runner.CircleCollider2D.radius, runner.ObstaclesLayer);
            // Debug.Log(collision);
            rigidbody.MovePosition(destination);
            animation.AdjustSpriteRotation(moveDirection.x);
        }

        public override void ChangeState()
        {
            if (currentSpeed == 0)
            {
                runner.SetState(typeof(IdleState));
            }
        }
    }
}
