using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace GameInput
{
    public class MouseUser : MonoBehaviour
    {
        private InputActions inputActions;

        [SerializeField] private UnityEngine.Camera mainCamera;

        private Vector2 MousePosition { get; set; }
        public Vector2 MouseInWorldPos => mainCamera.ScreenToWorldPoint(MousePosition);

        private bool isLeftMouseButtonPressed;
        private bool isRightMouseButtonPressed;

        private void OnEnable()
        {
            inputActions = InputActions.Instance;
            inputActions.Game.PerformAction.performed += OnPerformActionPerformed;
            inputActions.Game.PerformAction.canceled += OnPerformActionCancelled;
            inputActions.Game.CancelAction.performed += OnCancelActionPerformed;
            inputActions.Game.CancelAction.canceled += OnCancelActionCancelled;
            inputActions.Game.MousePosition.performed += OnMousePositionPerformed;
        }

        private void OnMousePositionPerformed(InputAction.CallbackContext ctx)
        {
            MousePosition = ctx.ReadValue<Vector2>();
        }

        private void OnPerformActionPerformed(InputAction.CallbackContext ctx)
        {
            isLeftMouseButtonPressed = true;
        }

        private void OnPerformActionCancelled(InputAction.CallbackContext ctx)
        {
            isLeftMouseButtonPressed = false;
        }

        private void OnCancelActionPerformed(InputAction.CallbackContext ctx)
        {
            isRightMouseButtonPressed = true;
        }

        private void OnCancelActionCancelled(InputAction.CallbackContext ctx)
        {
            isRightMouseButtonPressed = false;
        }
 
        public bool IsMouseButtonPressed(MouseButton button)
        {
            return button == MouseButton.LeftMouse ? isLeftMouseButtonPressed : isRightMouseButtonPressed;
        }

        private void OnDisable()
        {
            inputActions.Game.PerformAction.performed -= OnPerformActionPerformed;
            inputActions.Game.PerformAction.canceled -= OnPerformActionCancelled;
            inputActions.Game.CancelAction.performed -= OnCancelActionPerformed;
            inputActions.Game.CancelAction.canceled -= OnCancelActionCancelled;
            inputActions.Game.MousePosition.performed -= OnMousePositionPerformed;
        }
    }
}
