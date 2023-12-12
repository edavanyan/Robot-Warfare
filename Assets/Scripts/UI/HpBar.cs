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
        [SerializeField] private RectTransform hpBar;

        public void Change(int amount)
        {
            Value = Mathf.Clamp(Value - amount, 0, MaxValue);
            var t = (float)Value / MaxValue;
            bar.color = Color.Lerp(Color.red, Color.green, t);
            
            var hpMaskPadding = hpMask.padding;
            hpMaskPadding.z = hpBar.sizeDelta.x * hpBar.localScale.x * (1f - t);
            hpMask.padding = hpMaskPadding;
        }
    }
}
