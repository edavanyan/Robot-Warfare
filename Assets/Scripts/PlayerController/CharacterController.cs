using System;
using System.Collections;
using System.Collections.Generic;
using Attack;
using DG.Tweening;
using Loots;
using StateMachine;
using UI;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace PlayerController
{
    public class CharacterController : StateRunner<CharacterController>, IHittable
    {
        private HpBar hpBar;
        private XpBar xpBar;
        public CharacterAnimation CharacterAnimation;
        public Vector2 Input { get; set; }

        [SerializeField] private Animator animator;
        public Rigidbody2D rigidBody;
        public Dictionary<string, Sprite> ShadowSprites;
        [SerializeField] private Sprite[] shadows;
        public List<Attacker> attackers;
        [SerializeField] private int maxHealth;
        [SerializeField] private Vector3 barOffset = new Vector3(0, -0.1f, 0);
        [SerializeField] private Transform directionIndicator;

        private HitPoints hitPoints;
        private XPoints xPoints;

        [SerializeField] private ParticleSystem levelUpEffect;

        private SpriteRenderer spriteRenderer;
        public event Action OnAttack;
        public event Action<int> OnLevelUp;

        public bool IsLevelingUp { get; set; }

        private bool isAlive = true;

        protected override void Awake()
        {
            xPoints = new XPoints
            {
                Level = 1
            };
            xpBar = FindObjectOfType<XpBar>();
            xpBar.MaxValue = xPoints.NextLevelXp;
            xpBar.Value = 0;
            xpBar.ChangeImmediate(int.MinValue);
            xPoints.OnLevelUp += level =>
            {
                xpBar.MaxValue = xPoints.NextLevelXp - xPoints.Xp;
                xpBar.Value = 0;
                xpBar.ChangeImmediate(int.MinValue);
                foreach (var attacker in attackers)
                {
                    attacker.OnLevelUp(level);
                }

                OnLevelUp?.Invoke(level);
                hitPoints.IncreaseMaxHp(4);

                IsLevelingUp = true;
                levelUpEffect.gameObject.SetActive(true);
                levelUpEffect.Play();

                spriteRenderer.DOComplete();
                DOTween.Sequence()
                    .Append(spriteRenderer.DOColor(Color.cyan, 0.2f).SetLoops(6, LoopType.Yoyo)
                        .SetEase(Ease.OutSine))
                    .Join(transform.DOScale(new Vector3(1.05f, 1.05f, 1), 0.2f).SetLoops(6, LoopType.Yoyo))
                    .OnComplete(() =>
                    {
                        spriteRenderer.color = Color.white;
                        transform.localScale = Vector3.one;
                        IsLevelingUp = false;
                    });
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
                attacker.OnHitEvent += () => OnAttack?.Invoke();
            }

            hitPoints = new HitPoints(maxHealth);
            hitPoints.OnDie += () =>
            {
                if (isAlive)
                {
                    isAlive = false;
                    foreach (var attacker in attackers)
                    {
                        attacker.gameObject.SetActive(false);
                    }
                    xpBar.gameObject.SetActive(false);
                    CharacterAnimation.DieAnimation();
                    DOTween.Sequence().SetDelay(1f).AppendCallback(API.GameOver);
                }
            };

            hpBar = FindObjectOfType<HpBar>();
            hpBar.MaxValue = hpBar.Value = maxHealth;

            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            CharacterAnimation = new CharacterAnimation(animator, transform);
            ShadowSprites = new Dictionary<string, Sprite>();
            foreach (var shadow in shadows)
            {
                var name = shadow.name;
                var shadowSize = "shadow_".Length + 1;
                var key = string.Concat(name.Substring(0, name.Length - shadowSize),
                    shadow.name.Substring(name.Length - 1, 1));
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

        public void Hit(Projectile projectile)
        {
            if (IsLevelingUp)
            {
                return;
            }
            spriteRenderer.DOComplete();
            spriteRenderer.DOColor(Color.red, 0.1f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.OutSine).OnComplete(() =>
            {
                spriteRenderer.color = Color.white;
            });
            CharacterAnimation.HitAnimation();
            hitPoints.Hit(projectile.Damage);
            hpBar.Change(projectile.Damage);
            if (hitPoints.CurrentHitPoints <= hitPoints.MaxHitPoints * 0.3f)
            {
                API.ShowRedScreen(1 - (float)hitPoints.CurrentHitPoints / hitPoints.MaxHitPoints);
            }
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
                xpBar.Change(loot.amount, () => xPoints.Xp += loot.amount);
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
