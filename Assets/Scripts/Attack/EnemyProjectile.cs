using System;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

namespace Attack
{
    public class EnemyProjectile : Projectile, IPoolable
    {

        private SpriteRenderer spriteRenderer;
        public Vector2 Direction { set; private get; }
        public event Action<EnemyProjectile> OnComplete; 

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public override void New()
        {
            gameObject.SetActive(true);
        }

        public override void Free()
        {
            print("free");
            CompleteDestroy();
        }

        private void CompleteDestroy()
        {
            base.Free();
            gameObject.SetActive(false);
        }

        public override void Init(Transform target, Action<Transform, Projectile> hitCallback)
        {
            spriteRenderer.sprite = target.GetComponent<SpriteRenderer>().sprite;
            var scale = target.parent.localScale;
            transform.localScale = new Vector3(scale.x * -1, scale.y, scale.z);
            base.Init(target, hitCallback);
            transform.position = target.position;
            transform.DOScale(Vector3.zero, 0.1f).SetDelay(0.3f);
            rigidbody.AddForce(Direction.normalized * speed, ForceMode2D.Impulse);
            // DOTween.Sequence()
            //     .Append(
            //         transform.DOMove(Direction.normalized * speed, 1)
            //             .SetRelative(true)
            //             .SetEase(Ease.OutQuart)
            //             .OnComplete(() =>
            //             {
            //                 Health = 0;
            //                 OnComplete?.Invoke(this);
            //             }))
            //     .Insert(0.3f, );
        }
    }
}
