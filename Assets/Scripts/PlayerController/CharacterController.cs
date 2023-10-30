using System;
using System.Collections.Generic;
using Attack;
using Loots;
using StateMachine;
using UI;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

namespace PlayerController
{
    public class CharacterController : StateRunner<CharacterController>, IHittable
    {
        private HpBar hpBar;
        public CharacterAnimation CharacterAnimation;
        public Vector2 Input { get; set; }

        [SerializeField] private Animator animator;
        public Rigidbody2D rigidBody;
        public Dictionary<string, Sprite> ShadowSprites;
        [SerializeField] private Sprite[] shadows;
        [SerializeField] private Attacker attackerAgent;
        [SerializeField] private int maxHealth;
        [SerializeField] private Vector3 barOffset = new Vector3(0, -0.1f, 0);
        [SerializeField] private Transform directionIndicator;

        private HitPoints hitPoints;
        private XPoints xPoints;

        protected override void Awake()
        {
            xPoints = new XPoints
            {
                Level = 1
            };
            xPoints.OnLevelUp += level =>
            {
                attackerAgent.damage += 1;
                attackerAgent.attackRange += 0.1f;
                attackerAgent.AttackSpeed += 0.1f;
            };
            
            hitPoints = new HitPoints(maxHealth);
            hitPoints.OnDie += () => Debug.Log("Player Die");

            hpBar = FindObjectOfType<HpBar>();
            hpBar.MaxValue = hpBar.Value = maxHealth;
            
            rigidBody = GetComponent<Rigidbody2D>();
            CharacterAnimation = new CharacterAnimation(animator, transform);
            ShadowSprites = new Dictionary<string, Sprite>();
            foreach (var shadow in shadows)
            {
                var name = shadow.name;
                var shadowSize = "shadow_".Length + 1;
                var key = string.Concat(name.Substring(0, name.Length - shadowSize), shadow.name.Substring(name.Length - 1, 1));
                ShadowSprites.Add(key, shadow);
            }
            base.Awake();
        }

        public Vector2 GetMoveDirection()
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
            hpBar.Change(damageAmount);
        }

        private void LateUpdate()
        {
            var transform1 = transform;
            hpBar.transform.position = transform1.position + barOffset;
            
            var scale = transform1.localScale;
            directionIndicator.localScale = new Vector3(scale.x, scale.y, scale.z);
            var moveDirection = GetMoveDirection();
            if (moveDirection.sqrMagnitude > 0)
            {
                directionIndicator.gameObject.SetActive(true);
                directionIndicator.rotation =
                    Quaternion.Euler(0, 0, Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg);
            }
            else
            {
                directionIndicator.gameObject.SetActive(false);
            }
        }

        public void LootCollected(Loot loot)
        {
            if (loot.lootType == LootType.Xp)
            {
                xPoints.Xp += loot.amount;
            }
        }
    }
}
