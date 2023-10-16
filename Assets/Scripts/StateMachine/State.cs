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
        public abstract void Update();
        public abstract void FixedUpdate();
        public abstract void ChangeState();
        public abstract void Exit();
    }
}
