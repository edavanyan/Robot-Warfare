using PlayerController;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Cameras
{
    public class SmoothCamera2D : MonoBehaviour
    {
        public Camera mainCamera;

        [SerializeField] private float smoothSpeed = 002f;
        [SerializeField] private float offset;
        private Vector2 destination;
        private PlayerController.CharacterController character;
        [SerializeField] private float cameraAcceleration = 2f;
        [SerializeField] private Tilemap map;
        private Vector2 moveDirection = Vector2.zero;
        private Vector2 mapBoundsMax = new Vector2(28.5f, 16f);
        private Vector2 mapBoundsMin = new Vector2(28.5f, 16f);
        public Vector2 CameraBoundsMax => cameraBounds + (Vector2)transform.position;
        public Vector2 CameraBoundsMin => (Vector2)transform.position - cameraBounds;
        private Vector2 cameraBounds;

        private void Start()
        {
            var cellBounds = map.cellBounds;
            var boundsMax = map.CellToWorld(cellBounds.max);
            var boundsMin = map.CellToWorld(cellBounds.min);

            // ReSharper disable once LocalVariableHidesMember
            var camera = mainCamera;
            var vertical = camera.orthographicSize;
            var horizontal = vertical * camera.aspect;
            cameraBounds = new Vector2(horizontal, vertical);

            mapBoundsMax = new Vector2(boundsMax.x - horizontal, boundsMax.y - vertical);
            mapBoundsMin = new Vector2(boundsMin.x + horizontal, boundsMin.y + vertical);
            
            character = PlayerCharacterProvider.PlayerCharacter;
        }

        private void Move(Vector2 position)
        {
            destination = position + moveDirection * offset;
            var smoothPosition = Vector3.Lerp(transform.position, destination, smoothSpeed * Time.deltaTime);
            // smoothPosition.x = Mathf.Clamp(smoothPosition.x, mapBoundsMin.x, mapBoundsMax.x);
            // smoothPosition.y = Mathf.Clamp(smoothPosition.y, mapBoundsMin.y, mapBoundsMax.y);
            smoothPosition.z = -10f;
            transform.position = smoothPosition;
        }

        private void LateUpdate()
        {
            var direction = character.GetMoveDirection().normalized;
            Move(character.transform.position);
            moveDirection = Vector2.Lerp(moveDirection, direction, Time.deltaTime * cameraAcceleration);
        }
    }
}