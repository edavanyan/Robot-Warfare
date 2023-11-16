using System;
using System.Collections.Generic;
using Cameras;
using Cinemachine;
using Manager;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace PlayerController
{
    public class API : MonoBehaviour
    {
        public static CharacterController PlayerCharacter => instance.playerCharacter;
        public static SmoothCamera2D Camera2D => instance.camera2D;
        public static EnemyManager EnemyManager => instance.enemyManager;
        public static Volume GlobalVolume => instance.globalVolume;
        public static Light2D GlobalLight => instance.light2d;
        public static CinemachineVirtualCamera VirtualCamera => instance.virtualCamera;
        public static UIManager UIManager => instance.uiManager;

        private CharacterController playerCharacter;
        private SmoothCamera2D camera2D;
        private Volume globalVolume;
        private CinemachineVirtualCamera virtualCamera;
        private UIManager uiManager;
        [SerializeField]private Light2D light2d;
        private EnemyManager enemyManager;
        
        private static API instance;

        [SerializeField]private List<CharacterController> characters;
        
        public static string CharacterName { get; set; }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                
                SpawnMainCharacter();
                camera2D = FindObjectOfType<SmoothCamera2D>();
                enemyManager = FindObjectOfType<EnemyManager>();
                globalVolume = FindObjectOfType<Volume>();
                virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
                uiManager = FindObjectOfType<UIManager>();
            }
            else
            {
                Destroy(instance);
            }
        }

        private void SpawnMainCharacter()
        {
            CharacterName ??= "Luna";
            foreach (var character in characters)
            {
                if (character.name == CharacterName)
                {
                    SetCharacter(character);
                    break;
                }
            }
        }

        public static void SetCharacter(CharacterController character)
        {
            character.gameObject.SetActive(true);
            instance.playerCharacter = character;
        }
    }
}
