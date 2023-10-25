using System;
using GameInput;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

namespace UI
{
    public class InputForJoystick : MonoBehaviour
    {
        [SerializeField] private Canvas canvas;
        private Vector2 mousePosition;

        private void Awake()
        {
            gameObject.SetActive(false);
            var canvasRect = canvas.GetComponent<RectTransform>();
            InputActions.Instance.Game.TouchInput.performed += context =>
            {
                var touch = context.ReadValue<TouchState>();
                if (touch.phase == TouchPhase.Began)
                {
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
                else if (touch.phase == TouchPhase.Ended ||
                         touch.phase == TouchPhase.Canceled)
                {
                    gameObject.SetActive(false);
                }
            };
        }
    }
}
