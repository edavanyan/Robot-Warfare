using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Manager
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private RectTransform canvasRect;
        [SerializeField] private Image gameOverScreen;
        [SerializeField] private Button quitButton;

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

        public void ShowGameOverScreen(Action onComplete = null)
        {
            gameOverScreen.gameObject.SetActive(true);
            gameOverScreen.DOFade(1, 0.75f).OnComplete(() =>
            {
                onComplete?.Invoke();
                quitButton.gameObject.SetActive(true);
            });
        }
    }
}
