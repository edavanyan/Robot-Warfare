using System;
using Attack;
using Cameras;
using DG.Tweening;
using Loots;
using PlayerController;
using Unity.VisualScripting;
using UnityEngine;
using Utils;
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

        [SerializeField] private Animator animator;

        [SerializeField] internal float targetRadius;
        private Attacker attacker;

        private EnemyAnimation animationController;

        private HitPoints hitPoints;
        public event Action OnFaraway;
        public event Action OnDie;

        private bool dead;

        private CircleCollider2D circleCollider2D;
        private new SmoothCamera2D camera;
        private float horizontalDistance;
        private float verticalDistance;

        private SpriteRenderer spriteRenderer;

        public int xpAmount;


        private void Awake()
        {
            circleCollider2D = GetComponentInChildren<CircleCollider2D>();
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            animationController = new EnemyAnimation(animator, transform);
            hitPoints = new HitPoints(maxHealth);
            
            rigidBody = GetComponent<Rigidbody2D>();
            
            InitializeSteeringList();
            
            InvokeRepeating(nameof(AdjustRotation), 0.2f, 0.05f);
            
            attacker = GetComponentInChildren<Attacker>();
            attacker.OnAttack += show =>
            {
                animationController.AttackAnimation();
            };

            camera = ObjectProvider.Camera2D; 
            horizontalDistance = camera.cameraBounds.x + 4;
            verticalDistance = camera.cameraBounds.y + 4;
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
                var target = ObjectProvider.PlayerCharacter.transform;
                rigidBody.AddForce((rigidBody.position - (Vector2)target.transform.position).normalized * projectile.KnockBackForce,
                    ForceMode2D.Impulse);
                if (rigidBody.velocity.sqrMagnitude > projectile.KnockBackForce * projectile.KnockBackForce)
                {
                    rigidBody.velocity = rigidBody.velocity.normalized * projectile.KnockBackForce;
                }
            }
        }

        private void AdjustRotation()
        {
            var target = ObjectProvider.PlayerCharacter.transform;
            animationController.AdjustSpriteRotation(target.position.x - transform.position.x);
        }

        private void InitializeSteeringList()
        {
            // steeringList[0] = new SeekBehavior(1);
            // steeringList[1] = new ArriveBehavior(1);
        }

        private void FixedUpdate()
        {
            if (dead)
            {
                return;
            }

            var target = ObjectProvider.PlayerCharacter.transform;
            var acceleration = target.position - transform.position;
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
            if (rigidBody.velocity != Vector2.zero)
            {
                animationController.WalkingAnimation();
            }
            else
            {
                animationController.IdleAnimation();
            }

            var targetPosition = camera.transform.position;
            var position = transform.position;
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

        public void New()
        {
            spriteRenderer.color = Color.white;
            attacker.canAttack = true;
            transform.localScale = Vector3.one;
            dead = false;
            circleCollider2D.enabled = true;
            gameObject.SetActive(true);
            hitPoints.OnDie += Die;
            hitPoints.Reset();
        }

        public void Free()
        {
            transform.position = Constants.Nan;
            gameObject.SetActive(false);
        }
    }
}
