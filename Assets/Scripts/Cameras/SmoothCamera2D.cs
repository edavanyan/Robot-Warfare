using System;
using UnityEngine;

namespace Cameras
{
    public class SmoothCamera2D : MonoBehaviour
    {
        public Camera mainCamera;

        [SerializeField] private float smoothSpeed = 002f;
        [SerializeField] private float offset;
        private Vector2 destination;
        [SerializeField] private Transform target;
        private PlayerController.CharacterController character;
        [SerializeField] private float cameraAcceleration = 2f;
        private Vector2 moveDirection = Vector2.zero;

        private void Start()
        {
            character = target.gameObject.GetComponent<PlayerController.CharacterController>();
        }

        private void Move(Vector2 position)
        {
            destination = position + moveDirection * offset;
            var smoothPosition = Vector3.Lerp(transform.position, destination, smoothSpeed * Time.fixedDeltaTime);
            smoothPosition.z = -10f;
            transform.position = smoothPosition;
        }

        private void FixedUpdate()
        {
            var direction = character.GetCurrentSpeed().normalized;
            Move(target.position);
            moveDirection = Vector2.Lerp(moveDirection, direction, Time.deltaTime * cameraAcceleration);
        }
    }
}