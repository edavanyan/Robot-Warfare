using System;
using Cameras;
using DG.Tweening;
using PlayerController;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Loots
{
    public class Loot : MonoBehaviour, IPoolable
    {
        public CircleCollider2D circleCollider2D;
        private Animator animator;

        public event Action onCollected;
        private bool isCollecting;
        private Vector3 headOffset = new Vector3(0, 0.25f, 0);
        private new SpriteRenderer renderer;
        private static string UILayer = "UI";
        private static string GroundLayer = "Ground";

        public LootType lootType;
        public int amount;

        private new SmoothCamera2D camera;
        private float horizontalDistance;
        private float verticalDistance;

        private bool isEnabled = true;
        
        private void Awake()
        {
            renderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
            circleCollider2D = GetComponent<CircleCollider2D>();
            
            camera = API.Camera2D; 
            horizontalDistance = camera.CameraBounds.x + 2;
            verticalDistance = camera.CameraBounds.y + 2;
        }

        // private void Update()
        // {
        //     var targetPosition = camera.transform.position;
        //     var position = transform.position;
        //     if (Mathf.Abs(targetPosition.x - position.x) > horizontalDistance ||
        //         Mathf.Abs(targetPosition.y - position.y) > verticalDistance)
        //     {
        //         if (isEnabled)
        //         {
        //             Disable();
        //         }
        //     }
        //     else
        //     {
        //         if (!isEnabled)
        //         {
        //             Enable();
        //         }
        //     }
        // }

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
            move.onComplete = OnCollected;
            move.SetEase(Ease.InBack);
            move.easePeriod = 0.2f;
            move.easeOvershootOrAmplitude = 7.5f;
        }

        private void OnCollected()
        {
            isCollecting = false;
            onCollected?.Invoke();
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
            gameObject.SetActive(false);
        }
    }

    public enum LootType
    {
        Xp, Gold, Sword
    }
}
