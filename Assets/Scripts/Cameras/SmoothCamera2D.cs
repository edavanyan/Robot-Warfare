using System;
using Cinemachine;
using DG.Tweening;
using PlayerController;
using UnityEngine;


namespace Cameras
{
    public class SmoothCamera2D : MonoBehaviour
    {
        private Vector2 destination;
        public Vector2 CameraBoundsMax => CameraBounds + (Vector2)transform.position;
        public Vector2 CameraBoundsMin => (Vector2)transform.position - CameraBounds;

        public CinemachineVirtualCamera virtualCamera;


        public Vector2 CameraBounds
        {
            get
            {
                var vertical = virtualCamera.m_Lens.OrthographicSize;
                var horizontal = vertical * virtualCamera.m_Lens.Aspect;
                return new Vector2(horizontal, vertical);
            }
        }

        private void Shake()
        {
            virtualCamera.enabled = false;
            transform
                .DOShakePosition(0.05f, Vector3.one * 0.1f, 1, 0, false, false, ShakeRandomnessMode.Harmonic)
                .OnComplete(() => virtualCamera.enabled = true);
        }
    }
}