using System;
using UnityEngine;

namespace PlayerController
{
    public class PlayerCharacterProvider : MonoBehaviour
    {
        public static PlayerController.CharacterController PlayerCharacter => Instance.playerCharacter;
        private PlayerController.CharacterController playerCharacter;
        private static PlayerCharacterProvider Instance = null;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                playerCharacter = FindObjectOfType<CharacterController>();
            }
        }
    }
}
