using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachine
{
    public abstract class State<T> : ScriptableObject where T : MonoBehaviour
    {
        protected T runner;
        public virtual void Init(T parent)
        {
            runner = parent;
        }

        public abstract void CaptureInput(InputAction.CallbackContext context);
        public abstract Optional<Action> Update();
        public abstract Optional<Action> FixedUpdate();
        public abstract Optional<Action> ChangeState();
        public abstract Optional<Action> Exit();
    }
}
