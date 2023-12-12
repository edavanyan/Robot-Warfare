using System;
using DG.Tweening;
using Manager;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GameCanvas : MonoBehaviour
    {
        [SerializeField] private RectTransform canvasRect;
        [SerializeField] private Image gameOverScreen;
        [SerializeField] private Image gameOverBackground;
        [SerializeField] private Image quitButton;
        [SerializeField] private InputForJoystick joystickManager;
        [SerializeField] private FPSLogger timer;
        [SerializeField] private HpBar hpBar;
        [SerializeField] private XpBar xpBar;
        
        [SerializeField] private Vector3 hpBarOffset = new Vector3(0, -0.1f, 0);

        private void Start()
        {
            canvasRect = GetComponent<RectTransform>();
            API.PlayerCharacter.OnHealthChanged += ChangeHpBar;
            hpBar.MaxValue = hpBar.Value = API.PlayerCharacter.MaxHealth;

            API.PlayerCharacter.OnXpChanged += (amount, onComplete) => xpBar.Change(amount, onComplete);
            API.PlayerCharacter.OnXpReset += maxValue =>
            {
                xpBar.MaxValue = maxValue;
                xpBar.Value = 0;
                xpBar.ChangeImmediate(int.MinValue);
            };
        }

        private void Update()
        {
            hpBar.transform.position = API.PlayerCharacter.transform.position + hpBarOffset;
        }

        private void ChangeHpBar(int amount)
        {
            hpBar.Change(amount);
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
            joystickManager.gameObject.SetActive(false);
            hpBar.gameObject.SetActive(false);
            xpBar.gameObject.SetActive(false);
            timer.enabled = false;
            gameOverBackground.gameObject.SetActive(true);
            gameOverBackground.DOFade(1, 0.75f);
            gameOverScreen.DOFade(1, 0.75f).OnComplete(() =>
            {
                onComplete?.Invoke();
                quitButton.gameObject.SetActive(true);
                quitButton.GetComponent<MMScaleShaker>().enabled = true;
            });
        }
    }
}
