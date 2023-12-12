using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace Attack
{
    public class FloatingNumber : MonoBehaviour, IPoolable
    {
        [SerializeField] private TextMeshProUGUI numberText;

        private RectTransform rectTransform;

        public RectTransform RectTransform => rectTransform;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        public void New()
        {
            transform.localScale = Vector3.zero;
            gameObject.SetActive(true);
        }

        public void Free()
        {
            gameObject.SetActive(false);
        }

        public int Number
        {
            set => numberText.text = $"{value}";
        }
    }
}
