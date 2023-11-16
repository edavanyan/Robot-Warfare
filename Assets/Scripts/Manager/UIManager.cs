using System;
using UnityEngine;

namespace Manager
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField]private RectTransform canvasRect;

        private void Awake()
        {
            canvasRect = GetComponent<RectTransform>();
        }

        public Vector2 ScreenToCanvasPosition(Vector2 screenPosition)
        {
            var viewportPoint = Camera.main.ScreenToViewportPoint(screenPosition);
            var scale = canvasRect.sizeDelta;
            return new Vector2(
                viewportPoint.x * scale.x - scale.x / 2,
                viewportPoint.y * scale.y - scale.y / 2);
        }
    }
}
