using System;
using Cinemachine;
using DG.Tweening;
using PlayerController;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

namespace Manager
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI levelText;
        private Volume volume;
        private Light2D globalLight;
        private CinemachineVirtualCamera virtualCamera;
        private EnemyManager enemyManager;

        private float currentTime = 0f;
        private int interval = 1;
        private int difficultyInterval = 60;

        private int difficulty = 10;
        
        public static string CharacterName { get; set; }

        private void Awake()
        {
            volume = API.GlobalVolume;
            globalLight = API.GlobalLight;
            virtualCamera = API.VirtualCamera;
            virtualCamera.Follow = API.PlayerCharacter.transform;
            enemyManager = API.EnemyManager;

            API.PlayerCharacter.OnLevelUp += level => levelText.text = $"{level}";

            DOTween.SetTweensCapacity(5000, 500);
        }

        void Update()
        {
            currentTime += Time.deltaTime;
        
            if (currentTime > interval)
            {
                interval += 30;
                enemyManager.limit = Math.Clamp(Mathf.CeilToInt(enemyManager.limit * 1.5f), 0, 700);
                if (difficulty > enemyManager.EnemyDifficultyIndex)
                {
                    enemyManager.EnemyDifficultyIndex++;
                }
            }

            if (currentTime > difficultyInterval)
            {
                difficultyInterval += 60;
                difficulty++;
            }
        }
    }
}
