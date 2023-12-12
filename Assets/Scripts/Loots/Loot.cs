using System;
using Cameras;
using DG.Tweening;
using Manager;
using Unity.VisualScripting;
using UnityEngine;

namespace Loots
{
    public class Loot : MonoBehaviour, IPoolable
    {
        public CircleCollider2D circleCollider2D;
        private Animator animator;

        private event Action<Loot> OnCollected;
        private bool isCollecting;
        private readonly Vector3 headOffset = new Vector3(0, 0.25f, 0);
        private new SpriteRenderer renderer;
        private static readonly string UILayer = "UI";
        private static readonly string GroundLayer = "Ground";

        public LootType lootType;
        public int amount;

        private new SmoothCamera2D camera;
        private float horizontalDistance;
        private float verticalDistance;

        private bool isEnabled = true;

        private bool movingToTarget;
        
        private void Awake()
        {
            renderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
            circleCollider2D = GetComponent<CircleCollider2D>();
            
            camera = API.Camera2D; 
            horizontalDistance = camera.CameraBounds.x + 2;
            verticalDistance = camera.CameraBounds.y + 2;
        }

        public void Act()
        {
            if (movingToTarget)
            {
                var translation = (API.PlayerCharacter.transform.position - transform.position);
                if (translation.sqrMagnitude > 0.01f)
                {
                    transform.Translate(Time.deltaTime * 10f * translation.normalized, Space.World);
                }
                else
                {
                    movingToTarget = false;
                    OnCollected?.Invoke(this);
                }
            }
        }

        private void Enable()
        {
            isEnabled = true;
            renderer.enabled = isEnabled;
            // animator.enabled = isEnabled;
            circleCollider2D.enabled = isEnabled;
        }

        private void Disable()
        {
            isEnabled = false;
            renderer.enabled = isEnabled;
            // animator.enabled = isEnabled;
            circleCollider2D.enabled = isEnabled;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                StartCollecting(other.transform);
            }
        }

        public void Set(LootType type, int count)
        {
            amount = count;
            lootType = type;
        }

        void StartCollecting(Transform collector)
        {
            if (isCollecting) return;
            renderer.sortingLayerName = UILayer;
            circleCollider2D.enabled = false;
            var move = transform.DOMove(collector.position + headOffset, 0.3f);
            move.onComplete = Collected;
            move.SetEase(Ease.InBack);
            move.easePeriod = 0.2f;
            move.easeOvershootOrAmplitude = 7.5f;
        }

        private void Collected()
        {
            isCollecting = false;
            OnCollected?.Invoke(this);
            OnCollected = null;
        }

        public void RegisterOnCollected(Action<Loot> collectedCallback)
        {
            OnCollected += collectedCallback;
        }

        public bool OnCollectedRegistered()
        {
            return OnCollected != null;
        }

        public void New()
        {
            gameObject.SetActive(true);
            circleCollider2D.enabled = true;
        }

        public void Free()
        {
            renderer.sortingLayerName = GroundLayer;
            isCollecting = false;
            movingToTarget = false;
            gameObject.SetActive(false);
        }

        public void MoveToPlayer()
        {
            movingToTarget = true;
            isCollecting = true;
        }
    }

    public enum LootType
    {
        Xp, Gold, Sword, Projectile, Magnet
    }
}
