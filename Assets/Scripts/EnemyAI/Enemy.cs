using System;
using Attack;
using DG.Tweening;
using PlayerController;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace EnemyAI
{
    public class Enemy : MonoBehaviour, IHittable, IPoolable
    {
        internal Rigidbody2D rigidBody;
        internal Transform target;
        private readonly Steering[] steeringList = new Steering[2];
        public float maxAcceleration = 3f;
        public float maxSpeed = 1f;
        public float drag = 1f;
        [SerializeField] private int maxHealth;

        [SerializeField] private Animator animator;

        [SerializeField] internal float targetRadius;
        private Attacker attacker;

        private EnemyAnimation animationController;

        private HitPoints hitPoints;
        public event Action OnDie;

        private bool dead;

        private CircleCollider2D circleCollider2D;

        public int xpAmount; 

        private void Awake()
        {
            circleCollider2D = GetComponentInChildren<CircleCollider2D>();
            animationController = new EnemyAnimation(animator, transform);
            hitPoints = new HitPoints(maxHealth);
            
            rigidBody = GetComponent<Rigidbody2D>();
            
            target = PlayerCharacterProvider.PlayerCharacter.transform;
            InitializeSteeringList();
            
            InvokeRepeating(nameof(AdjustRotation), Random.Range(0f, 0.2f), Random.Range(0.1f, 0.3f));
            
            attacker = GetComponentInChildren<Attacker>();
            attacker.OnAttack += animationController.AttackAnimation;
        }

        public void Hit(int amount)
        {
            animationController.HitAnimation();
            if (!hitPoints.Hit(amount))
            {
                rigidBody.AddForce((rigidBody.position - (Vector2)target.transform.position).normalized * 1.5f,
                    ForceMode2D.Impulse);
            }
        }

        private void AdjustRotation()
        {
            animationController.AdjustSpriteRotation(target.position.x - transform.position.x);
        }

        private void InitializeSteeringList()
        {
            steeringList[0] = new SeekBehavior(3);
            steeringList[1] = new ArriveBehavior(1);
        }

        private void FixedUpdate()
        {
            if (dead)
            {
                return;
            }
            var acceleration = Vector2.zero;
            foreach (var steering in steeringList)
            {
                var steeringData = steering.GetSteering(this);
                acceleration += steeringData.linear * steering.GetWeight();
            }
            if (acceleration.magnitude > maxSpeed)
            {
                acceleration.Normalize();
                acceleration *= maxSpeed;
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
        }

        private void Die()
        {
            circleCollider2D.enabled = false;
            rigidBody.velocity = Vector2.zero;
            dead = true;
            transform.DOScale(Vector3.zero, 0.1f).OnComplete(() =>
            {
                transform.localScale = Vector3.one;
                animationController.DieAnimation();
                Invoke(nameof(DieInvoke), 0.15f);
            });
        }

        private void DieInvoke()
        {
            OnDie?.Invoke();
        }

        public void New()
        {
            transform.localScale = Vector3.one;
            dead = false;
            circleCollider2D.enabled = true;
            gameObject.SetActive(true);
            hitPoints.OnDie += Die;
            hitPoints.Reset();
        }

        public void Free()
        {
            transform.localScale = Vector3.one;
            gameObject.SetActive(false);
        }
    }
}
