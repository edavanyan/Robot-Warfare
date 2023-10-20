using System;
using System.Collections.Generic;
using System.Linq;
using PlayerController;
using UnityEngine;
using CharacterController = UnityEngine.CharacterController;

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
            character = GetComponent<PlayerController.CharacterController>();
            SetState(states[0].GetType());
        }

        public void SetState(Type newStateType)
        {
            activeState?.Exit();

            activeState = states.First(s => s.GetType() == newStateType);
            activeState.Init(character);
        }

        private void Update()
        {
            activeState.Update();
            activeState.ChangeState();
        }

        private void FixedUpdate()
        {
            activeState.FixedUpdate();
        }
    }
}
