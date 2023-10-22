using System;
using System.Collections.Generic;
using Attack;
using StateMachine;
using Unity.VisualScripting;
using UnityEngine;

namespace PlayerController
{
    public class CharacterController : StateRunner<CharacterController>, IHittable
    {
        public LayerMask ObstaclesLayer;
        public CharacterAnimation CharacterAnimation;
        public Vector2 Input = Vector2.zero;
        [SerializeField] private Animator animator;
        public CircleCollider2D CircleCollider2D;
        public Rigidbody2D rigidBody;
        public Dictionary<string, Sprite> shadowSprites;
        [SerializeField] private Sprite[] shadows;
        [SerializeField] private Attacker attackerAgent;
        [SerializeField] private int maxHealth;

        private HitPoints hitPoints;

        protected override void Awake()
        {
            hitPoints = new HitPoints(maxHealth);
            hitPoints.OnDie += () => Debug.Log("Player Die");
            rigidBody = GetComponent<Rigidbody2D>();
            CharacterAnimation = new CharacterAnimation(animator, transform);
            shadowSprites = new Dictionary<string, Sprite>();
            foreach (var shadow in shadows)
            {
                var name = shadow.name;
                var shadowSize = "shadow_".Length + 1;
                var key = string.Concat(name.Substring(0, name.Length - shadowSize), shadow.name.Substring(name.Length - 1, 1));
                shadowSprites.Add(key, shadow);
            }
            base.Awake();
        }

        public Vector2 GetCurrentSpeed()
        {
            return activeState.GetPlayerMoveDirection();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, 0.25f);
        }

        public void Hit(int damageAmount)
        {
            hitPoints.Hit(damageAmount);
        }
    }
}
