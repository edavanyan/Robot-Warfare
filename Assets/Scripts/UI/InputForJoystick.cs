using System;
using DG.Tweening;
using GameInput;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.UI;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

namespace UI
{
    public class InputForJoystick : MonoBehaviour
    {
        [SerializeField] private Canvas canvas;
        private Vector2 mousePosition;
        [SerializeField] private OnScreenStick onScreenStick;
        private RectTransform canvasRect;
        [SerializeField] private Image outer;
        [SerializeField] private Image inner;

        private Color outerColor;
        private Color innerColor;

        private void Awake()
        {
            outerColor = outer.color;
            innerColor = inner.color;

            var color = outerColor;
            color.a = 0;
            outer.color = color;
            color = innerColor;
            color.a = 0;
            inner.color = color;
            
            canvasRect = canvas.GetComponent<RectTransform>();
            InputActions.Instance.Game.TouchInput.performed += AdjustJoystickPosition;
        }
        
        private void AdjustJoystickPosition(InputAction.CallbackContext context)
        {
            var touch = context.ReadValue<TouchState>();
            if (touch.phase == TouchPhase.Began)
            {
                var color = outerColor;
                outer.color = color;
                color = innerColor;
                inner.color = color;
                mousePosition = touch.position;
                gameObject.SetActive(true);
                var viewportPosition = new Vector3(mousePosition.x / Screen.width,
                    mousePosition.y / Screen.height,
                    0);
            
                var rectTransform = (RectTransform)transform;
                var centerBasedViewPortPosition = viewportPosition - new Vector3(0.5f, 0.5f, 0);
                var scale = canvasRect.sizeDelta;
                rectTransform.anchoredPosition = Vector3.Scale(centerBasedViewPortPosition, scale);
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                var color = outerColor;
                color.a = 0;
                outer.color = color;
                color = innerColor;
                color.a = 0;
                inner.color = color;
            }
        }

        private void OnDestroy()
        {
            InputActions.Instance.Game.TouchInput.performed -= AdjustJoystickPosition;
        }
    }
}
