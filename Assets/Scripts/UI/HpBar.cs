using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HpBar : MonoBehaviour
    {
        [field: SerializeField]
        public int MaxValue { get; set; }
        [field: SerializeField]
        public int Value { get; set; }

        [SerializeField] private RectMask2D hpMask;
        [SerializeField] private Image bar;
        [SerializeField] private Transform hpBar;

        private bool isTweening;

        public void Change(int amount)
        {
            Value = Mathf.Clamp(Value - amount, 0, MaxValue);
            var t = (float)Value / MaxValue;
            bar.color = Color.Lerp(Color.red, Color.green, t);
            
            var hpMaskPadding = hpMask.padding;
            hpMaskPadding.z = 0.8f * (1f - t);
            hpMask.padding = hpMaskPadding;

            if (t <= 0.3f)
            {
                if (!isTweening)
                {
                    hpBar.DOScale(new Vector3(0.012f, 0.012f, 1f), 0.5f).SetLoops(-1, LoopType.Yoyo);
                    isTweening = true;
                }
            }
            else
            {
                if (isTweening)
                {
                    isTweening = false;
                    hpBar.DOKill();
                    hpBar.localScale = new Vector3(0.01f, 0.01f, 1f);
                }
            }
        }
    }
}
