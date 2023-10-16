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

        protected virtual void Awake()
        {
            SetState(states[0].GetType());
        }

        public void SetState(Type newStateType)
        {
            if (activeState != null)
            {
                activeState.Exit();
            }

            activeState = states.First(s => s.GetType() == newStateType);
            activeState.Init(GetComponent<PlayerController.CharacterController>());
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
