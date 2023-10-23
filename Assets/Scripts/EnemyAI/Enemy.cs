using System;
using Attack;
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

        private void Awake()
        {
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
            hitPoints.Hit(amount);
            animationController.HitAnimation();
            rigidBody.AddForce((rigidBody.position - (Vector2)target.transform.position).normalized * 1.5f, ForceMode2D.Impulse);
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

        public void New()
        {
            gameObject.SetActive(true);
            hitPoints.OnDie += () => OnDie?.Invoke();
        }

        public void Free()
        {
            hitPoints.Reset();
            gameObject.SetActive(false);
        }
    }
}
