using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Loots
{
    public class Loot : MonoBehaviour, IPoolable
    {
        private CircleCollider2D circleCollider2D;
        private Animator animator;

        public event Action onCollected;
        private bool isCollecting;
        private Vector3 headOffset = new Vector3(0, 0.25f, 0);

        private void Awake()
        {
            animator = GetComponent<Animator>();
            circleCollider2D = GetComponent<CircleCollider2D>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                StartCollecting(other.transform);
            }
        }

        public void Set(RuntimeAnimatorController animatorController)
        {
            animator.runtimeAnimatorController = animatorController;
        }

        void StartCollecting(Transform collector)
        {
            if (isCollecting) return;
            isCollecting = true;
            circleCollider2D.enabled = false;
            headOffset.y = Random.Range(0, 0.5f);
            var move = transform.DOMove(collector.position + headOffset, 0.3f);
            move.onComplete = OnCollected;
            move.SetEase(Ease.InBack);
            move.easePeriod = 0.2f;
            move.easeOvershootOrAmplitude = 10;
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
            isCollecting = false;
            gameObject.SetActive(false);
        }
    }
}
