using UnityEngine;
using UnityMovementAI.Scripts.Camera;
using UnityMovementAI.Scripts.Units.Movement;

namespace UnityMovementAI.Scripts
{
    [RequireComponent(typeof(MovementAIRigidbody))]
    [RequireComponent(typeof(UnityEngine.Camera))]
    public class ThirdPersonUnit : MonoBehaviour
    {
        public float speed = 5f;

        public float facingSpeed = 720f;

        public float jumpSpeed = 7f;

        public bool autoAttachToCamera = true;

        MovementAIRigidbody rb;

        Transform cam;

        float horAxis = 0f;
        float vertAxis = 0f;

        void Start()
        {
            rb = GetComponent<MovementAIRigidbody>();
            cam = UnityEngine.Camera.main.transform;

            if (autoAttachToCamera)
            {
                cam.GetComponent<ThirdPersonCamera>().target = transform;
            }
        }

        void Update()
        {
            horAxis = Input.GetAxisRaw("Horizontal");
            vertAxis = Input.GetAxisRaw("Vertical");

            if (Input.GetButtonDown("Jump"))
            {
                rb.Jump(jumpSpeed);
            }
        }

        void FixedUpdate()
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                rb.Velocity = GetMovementDir() * speed;
            }
            else
            {
                rb.Velocity = Vector3.zero;
            }
        }

        void LateUpdate()
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Vector3 dir = GetMovementDir();

                if (dir.magnitude > 0)
                {
                    float curFacing = transform.eulerAngles.y;
                    float facing = Mathf.Atan2(-dir.z, dir.x) * Mathf.Rad2Deg;
                    rb.Rotation = Quaternion.Euler(0, Mathf.MoveTowardsAngle(curFacing, facing, facingSpeed * Time.deltaTime), 0);
                }
            }
        }

        Vector3 GetMovementDir()
        {
            return ((cam.forward * vertAxis) + (cam.right * horAxis)).normalized;
        }
    }
}