using System;
using System.Collections.Generic;
using System.Linq;
using PlayerController;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachine
{
    public abstract class StateRunner<T> : MonoBehaviour where T : MonoBehaviour
    {
        [SerializeField]
        private List<PlayerState> states;
        protected PlayerState activeState;
        private PlayerController.CharacterController character;

        protected virtual void Awake()
        {
            GameInput.InputActions.Instance.Game.Movement.performed += CaptureInput;
            GameInput.InputActions.Instance.Game.Movement.canceled += CaptureInput;
            character = GetComponent<PlayerController.CharacterController>();
            SetState(states[0].GetType());
        }

        private void OnDestroy()
        {
            GameInput.InputActions.Instance.Game.Movement.performed -= CaptureInput;
            GameInput.InputActions.Instance.Game.Movement.canceled -= CaptureInput;
        }

        public void SetState(Type newStateType)
        {
            Invoke(activeState?.Exit());

            activeState = states.First(s => s.GetType() == newStateType);
            activeState.Init(character);
        }

        private void Update()
        {
            Invoke(activeState.Update());
            Invoke(activeState.ChangeState());
        }

        private void FixedUpdate()
        {
            Invoke(activeState.FixedUpdate());
        }

        private void CaptureInput(InputAction.CallbackContext context)
        {
            activeState.CaptureInput(context);
        }

        private void Invoke(Optional<Action> method)
        {
            method?.Get()?.Invoke();
        }
    }
}
