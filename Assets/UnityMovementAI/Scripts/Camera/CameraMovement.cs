using UnityEngine;

namespace UnityMovementAI.Scripts.Camera
{
    public class CameraMovement : MonoBehaviour
    {
        public Transform target;

        Vector3 displacement;

        void Start()
        {
            displacement = transform.position - target.position;
        }

        void LateUpdate()
        {
            if (target != null)
            {
                transform.position = target.position + displacement;
            }
        }
    }
}