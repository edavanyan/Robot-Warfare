using System;
using StateMachine;
using UnityEngine;

namespace PlayerController
{
    [CreateAssetMenu(menuName = "States/Character/Walk")]
    public class WalkState : PlayerState
    {
        [SerializeField]
        private float speed = 5f;

        private float currentSpeed = 0;
        private Transform directionIndicator;
        [SerializeField]
        private float acceleration = 50f;
        [SerializeField]
        private float deAcceleration = 100f;

        private Optional<Action> update;
        private Optional<Action> fixedUpdate;
        private Optional<Action> changeState;
        private Optional<Action> exit;

        public override void Init(CharacterController parent)
        {
            base.Init(parent);
            currentSpeed = 0;
            directionIndicator = parent.DirectionIndicator;
            directionIndicator.gameObject.SetActive(true);

            update ??= new(() => {
                var scale = runner.transform.localScale;
                directionIndicator.localScale = new Vector3(scale.x, scale.y, scale.z);
                var moveDirection = runner.Input;
                if (moveDirection.sqrMagnitude > 0 && currentSpeed >= 0)
                {
                    currentSpeed += acceleration * speed * Time.deltaTime;
                    directionIndicator.rotation =
                        Quaternion.Euler(0, 0, Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg);
                }
                else
                {
                    currentSpeed -= deAcceleration * speed * Time.deltaTime;
                }
                currentSpeed = Mathf.Clamp(currentSpeed, 0, speed); });

            fixedUpdate ??= new(() => {
                var moveDirection = runner.Input;
                var position = rigidBody.position;
                var destination = position + currentSpeed * Time.fixedDeltaTime * moveDirection;

                rigidBody.MovePosition(destination);
                animation.AdjustSpriteRotation(moveDirection.x); });

            changeState ??= new(() =>
            {
                if (currentSpeed == 0)
                {
                    runner.SetState(typeof(IdleState));
                }
            });

            exit ??= new(() => directionIndicator.gameObject.SetActive(false));
        }

        protected override void PlayStateAnimation()
        {
            animation.WalkingHorizontalAnimation();
        }

        public override Optional<Action> Exit()
        {
            return exit;
        }

        public override Optional<Action> Update()
        {
            return update;
        }

        public override Optional<Action> FixedUpdate()
        {
            return fixedUpdate;
        }

        public override Optional<Action> ChangeState()
        {
            return changeState;
        }
    }
}
