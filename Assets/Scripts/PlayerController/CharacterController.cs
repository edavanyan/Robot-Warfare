using System;
using System.Collections.Generic;
using Attack;
using DG.Tweening;
using Loots;
using Manager;
using Player;
using StateMachine;
using UnityEngine;

namespace PlayerController
{
    public class CharacterController : StateRunner<CharacterController>, IHittable
    {
        public CharacterAnimation CharacterAnimation { get; private set; }
        public Vector2 Input { get; set; }

        [SerializeField] private Animator animator;
        [field:SerializeField] public Rigidbody2D RigidBody { get; private set; }
        [field:SerializeField] public Transform DirectionIndicator { get; private set; }
        public int MaxHealth => maxHealth;

        [SerializeField] private int maxHealth;
        private HitPoints hitPoints;
        private XPoints xPoints;
        public event Action<int> OnLevelUp;
        public event Action<float> OnLowHealth;
        public event Action<int> OnHealthChanged;
        public event Action<int> OnXpReset;
        public event Action<int, TweenCallback> OnXpChanged;
        public List<Attacker> attackers;

        protected override void Awake()
        {
            xPoints = new XPoints
            {
                Level = 1
            };
            xPoints.OnLevelUp += level =>
            {
                
                OnXpReset?.Invoke(xPoints.NextLevelXp - xPoints.Xp);
                foreach (var attacker in attackers)
                {
                    attacker.OnLevelUp(level);
                }
                CharacterAnimation.LevelUpAnimation();

                OnLevelUp?.Invoke(level);
                var hp = hitPoints.CurrentHitPoints;
                hitPoints.IncreaseMaxHp(4);
                OnHealthChanged?.Invoke(hp - hitPoints.CurrentHitPoints);
            };
            foreach (var attacker in attackers)
            {
                attacker.OnAttack += (showAnim) =>
                {
                    if (showAnim)
                    {
                        CharacterAnimation.AttackAnimation();
                    }
                };
            }

            hitPoints = new HitPoints(maxHealth);
            hitPoints.OnDie += () =>
            {
                if (activeState.GetType() != typeof(DeadState))
                {
                    SetState(typeof(DeadState));
                    foreach (var attacker in attackers)
                    {
                        attacker.gameObject.SetActive(false);
                    }

                    DOTween.Sequence().SetDelay(1f).AppendCallback(API.GameManager.GameOver);
                }
            };

            CharacterAnimation = new CharacterAnimation(animator, transform);

            base.Awake();
        }

        private void Start()
        {
            OnXpReset?.Invoke(xPoints.NextLevelXp);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, 0.25f);
        }

        public void Hit(Projectile projectile)
        {
            CharacterAnimation.HitAnimation();
            hitPoints.Hit(projectile.Damage);
            OnHealthChanged?.Invoke(projectile.Damage);
            if (hitPoints.CurrentHitPoints <= hitPoints.MaxHitPoints * 0.3f)
            {
                OnLowHealth?.Invoke((float)hitPoints.CurrentHitPoints / hitPoints.MaxHitPoints);
            }
        }

        public void LootCollected(Loot loot)
        {
            if (loot.lootType == LootType.Xp)
            {
                OnXpChanged?.Invoke(loot.amount, () => xPoints.Xp += loot.amount);
            }
            else if (loot.lootType == LootType.Projectile)
            {
                UpgradeWeapon();
            }
            else if (loot.lootType == LootType.Magnet)
            {
                API.LootManager.CollectAll();
            }
        }

        private void UpgradeWeapon()
        {
            foreach (var attacker in attackers)
            {
                attacker.Upgrade();
                if (attacker.type == AttackType.Melee)
                {
                    attacker.ScheduleUpgrade();
                }
            }
        }
    }
}
