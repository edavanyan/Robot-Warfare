using System;
using StateMachine;
using Unity.VisualScripting;
using UnityEngine;

namespace PlayerController
{
    public class CharacterController : StateRunner<CharacterController>
    {
        public LayerMask ObstaclesLayer;
        public CharacterAnimation CharacterAnimation;
        public Vector2 Input = Vector2.zero;
        [SerializeField] private Animator animator;
        public CircleCollider2D CircleCollider2D;

        protected override void Awake()
        {
            CharacterAnimation = new CharacterAnimation(animator, transform);
            base.Awake();
        }

        public Vector2 GetCurrentSpeed()
        {
            return activeState.GetPlayerMoveDirection();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            // Gizmos.DrawWireSphere(GetComponent<Rigidbody2D>().position + CircleCollider2D.offset, CircleCollider2D.radius);
        }
    }
}
