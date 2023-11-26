using System;
using System.Collections.Generic;
using Cameras;
using Cinemachine;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Manager;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

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
        public static LootManager LootManager => instance.lootManager;

        private CharacterController playerCharacter;
        private SmoothCamera2D camera2D;
        private Volume globalVolume;
        private CinemachineVirtualCamera virtualCamera;
        private UIManager uiManager;
        [SerializeField]private Light2D light2d;
        private EnemyManager enemyManager;
        private LootManager lootManager;
        
        private static API instance;

        [SerializeField]private List<CharacterController> characters;
        private Bloom bloom;
        private static TweenerCore<float, float, FloatOptions> redScreenPulse;

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
                lootManager = FindObjectOfType<LootManager>();
                GlobalVolume.profile.TryGet(out bloom);
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

        public static void ShowRedScreen(float intensity)
        {
            if (!instance.bloom.active)
            {
                instance.bloom.active = true;
            }
            instance.bloom.intensity.value = 0;
            redScreenPulse?.Kill();
            redScreenPulse = DOTween.To(() => instance.bloom.intensity.value, i => instance.bloom.intensity.value = i, intensity, 1).SetLoops(-1, LoopType.Restart).SetEase(Ease.InOutSine);
        }

        public static void RemoveRedScreen()
        {
            redScreenPulse.Kill();
            instance.bloom.active = false;
        }

        public static void GameOver()
        {
            UIManager.ShowGameOverScreen(RemoveRedScreen);
        }

        public void Quit()
        {
            SceneManager.LoadScene("Menu");
        }
    }
}
