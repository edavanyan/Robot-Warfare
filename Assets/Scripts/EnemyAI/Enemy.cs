using System;
using Attack;
using Cameras;
using DG.Tweening;
using Loots;
using PlayerController;
using Unity.VisualScripting;
using UnityEngine;
using Utils;
using Utils.Pool;
using Sequence = DG.Tweening.Sequence;

namespace EnemyAI
{
    public class Enemy : MonoBehaviour, IHittable, IPoolable
    {
        internal Rigidbody2D rigidBody;
        // private readonly Steering[] steeringList = new Steering[2];
        public float maxAcceleration = 3f;
        public float targetRadiusAcceleration = 0.5f;
        public float maxSpeed = 1f;
        public float drag = 1f;
        [SerializeField] private int maxHealth;
        public LootType LootType { get; set; }
        public ComponentPool<Enemy> Pool { get; set; }

        [SerializeField] private Animator animator;

        [SerializeField] internal float targetRadius;
        private Attacker attacker;

        private EnemyAnimation animationController;

        private HitPoints hitPoints;
        private event Action OnFaraway;
        private event Action OnDie;
        private event Action<int> OnHit;

        private bool dead;

        private CircleCollider2D circleCollider2D;
        private new SmoothCamera2D camera;
        private float horizontalDistance;
        private float verticalDistance;

        private SpriteRenderer spriteRenderer;

        public int xpAmount;

        private static int awakeCount;
        
        private void Awake()
        {
            circleCollider2D = GetComponentInChildren<CircleCollider2D>();
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            animationController = new EnemyAnimation(animator, transform);
            hitPoints = new HitPoints(maxHealth);
            hitPoints.OnDie += Die;
            print(awakeCount++);
            
            rigidBody = GetComponent<Rigidbody2D>();
            
            InitializeSteeringList();
            
            InvokeRepeating(nameof(AdjustRotation), 0.2f, 0.2f);
            
            attacker = GetComponentInChildren<Attacker>();
            attacker.OnAttack += show =>
            {
                animationController.AttackAnimation();
            };

            camera = API.Camera2D; 
            horizontalDistance = camera.CameraBounds.x + 4;
            verticalDistance = camera.CameraBounds.y + 4;
        }

        public void Hit(Projectile projectile)
        {
            if (dead)
            {
                return;
            }
            spriteRenderer.DOColor(Color.red, 0.05f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.OutSine);
            animationController.HitAnimation();
            if (!hitPoints.Hit(projectile.Damage))
            {
                var target = API.PlayerCharacter.transform;
                rigidBody.AddForce((rigidBody.position - (Vector2)target.transform.position).normalized * projectile.KnockBackForce,
                    ForceMode2D.Impulse);
                if (rigidBody.velocity.sqrMagnitude > projectile.KnockBackForce * projectile.KnockBackForce)
                {
                    rigidBody.velocity = rigidBody.velocity.normalized * projectile.KnockBackForce;
                }
            }
            OnHit?.Invoke(projectile.Damage);
        }

        private void AdjustRotation()
        {
            var target = API.PlayerCharacter.transform;
            animationController.AdjustSpriteRotation(target.position.x - transform.position.x);
        }

        private void InitializeSteeringList()
        {
            // steeringList[0] = new SeekBehavior(1);
            // steeringList[1] = new ArriveBehavior(1);
        }

        public void Act()
        {
            if (dead)
            {
                return;
            }

            var target = API.PlayerCharacter.transform;
            var selfTransform = transform;
            var acceleration = target.position - selfTransform.position;
            // foreach (var steering in steeringList)
            // {
            //     var steeringData = steering.GetSteering(this);
            //     acceleration += steeringData.linear * steering.GetWeight();
            // }
            if (acceleration.sqrMagnitude > 3 * maxSpeed * maxSpeed)
            {
                acceleration.Normalize();
                acceleration *= maxSpeed;
            }
            else if (acceleration.sqrMagnitude > 0.25f)
            {
                acceleration.Normalize();
                acceleration *= targetRadiusAcceleration;
            }
            else
            {
                acceleration = Vector2.zero;
            }
            rigidBody.AddForce(acceleration);
            // if (rigidBody.velocity != Vector2.zero)
            // {
            //     animationController.WalkingAnimation();
            // }
            // else
            // {
            //     animationController.IdleAnimation();
            // }

            var targetPosition = camera.transform.position;
            var position = selfTransform.position;
            if (Mathf.Abs(targetPosition.x - position.x) > horizontalDistance ||
                Mathf.Abs(targetPosition.y - position.y) > verticalDistance)
            {
                OnFaraway?.Invoke();
            }
        }

        private void Die()
        {
            attacker.canAttack = false;
            circleCollider2D.enabled = false;
            rigidBody.velocity = Vector2.zero;
            DOTween.Sequence().SetDelay(0.1f)
                .OnStepComplete(() =>
                {
                    dead = true;
                    animationController.DieAnimation();
                })
                .Append(
            transform.DOScale(Vector3.zero, 0.15f).OnComplete(DieInvoke));
        }

        private void DieInvoke()
        {
            OnDie?.Invoke();
        }

        public bool OnHitSubscribed()
        {
            return OnHit != null;
        }

        public void SubscribeOnHit(Action<int> onHit)
        {
            OnHit += onHit;
        }

        public bool OnDieSubscribed()
        {
            return OnDie != null;
        }

        public void SubscribeOnDie(Action onDie)
        {
            OnDie += onDie;
        }

        public bool OnFarAwaySubscribed()
        {
            return OnFaraway != null;
        }
        public void SubscribeOnFarAway(Action onFarAway)
        {
            OnFaraway += onFarAway;
        }

        public void New()
        {
            spriteRenderer.color = Color.white;
            attacker.canAttack = true;
            transform.localScale = Vector3.one;
            dead = false;
            circleCollider2D.enabled = true;
            gameObject.SetActive(true);
            hitPoints.Reset();
        }

        public void Free()
        {
            transform.position = Constants.Nan;
            gameObject.SetActive(false);
        }
    }
}
