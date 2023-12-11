using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class XpBar : MonoBehaviour
    {

        [field: SerializeField]
        public int MaxValue { get; set; }
        [field: SerializeField]
        public int Value { get; set; }
        [SerializeField] private RectMask2D xpMask;
        private readonly List<TweenData> queuedAnimations = new();
        private bool animating;
        private RectTransform parentTransform;

        private void Awake()
        {
            parentTransform = transform.parent as RectTransform;
        }

        public void ChangeImmediate(int amount)
        {
            Value = Mathf.Clamp(Value + amount, 0, MaxValue);
            var t = (float)Value / MaxValue;
            var xpMaskPadding = xpMask.padding;
            xpMaskPadding.z = parentTransform.sizeDelta.x * (1f - t);
            xpMask.padding = xpMaskPadding;
        }

        public void Change(int amount, TweenCallback onComplete)
        {
            if (!animating)
            {
                AnimateBar(amount, onComplete);
            }
            else
            {
                queuedAnimations.Add(new TweenData()
                {
                    amount = amount,
                    callback = onComplete
                });
            }
        }

        private void AnimateBar(int amount, TweenCallback onComplete)
        {
            animating = true;
            Value = Mathf.Clamp(Value + amount, 0, MaxValue);
            var t = (float)Value / MaxValue;
            var xpMaskPadding = xpMask.padding;
            var z = parentTransform.sizeDelta.x * (1f - t);
            var duration = 0.2f / (Mathf.Clamp(queuedAnimations.Count, 2, 96) / 2f);
            DOTween.To(() => xpMaskPadding.z, value => xpMaskPadding.z = value, z, duration)
                .SetEase(Ease.Linear)
                .OnUpdate(() => xpMask.padding = xpMaskPadding)
                .OnComplete(() =>
                {
                    animating = false;
                    onComplete?.Invoke();
                    if (queuedAnimations.Count > 0)
                    {
                        var queuedAnimation = queuedAnimations[0];
                        queuedAnimations.RemoveAt(0);
                        Change(queuedAnimation.amount, queuedAnimation.callback);
                    }
                });
        }

        struct TweenData
        {
            public int amount;
            public TweenCallback callback;
        }
    }
}
