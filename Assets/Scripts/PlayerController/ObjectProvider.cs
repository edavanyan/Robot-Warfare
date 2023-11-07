using System;
using Cameras;
using UnityEngine;

namespace PlayerController
{
    public class ObjectProvider : MonoBehaviour
    {
        public static CharacterController PlayerCharacter => instance.playerCharacter;
        public static SmoothCamera2D Camera2D => instance.camera2D;
        private CharacterController playerCharacter;
        private SmoothCamera2D camera2D;
        private static ObjectProvider instance;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                playerCharacter = FindObjectOfType<CharacterController>();
                camera2D = FindObjectOfType<SmoothCamera2D>();
            }
        }

        public static void SetCharacter(CharacterController character)
        {
            instance.playerCharacter = character;
        }
    }
}
