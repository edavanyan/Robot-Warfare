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

        public void Change(int amount)
        {
            Value = Mathf.Clamp(Value - amount, 0, MaxValue);
            var hpMaskPadding = hpMask.padding;
            hpMaskPadding.z = 0.489f * (1f - (float)Value / MaxValue);
            hpMask.padding = hpMaskPadding;
        }
    }
}
