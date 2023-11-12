using System;
using Cinemachine;
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
        private int interval = 5;
        
        public static string CharacterName { get; set; }

        private void Awake()
        {
            volume = API.GlobalVolume;
            globalLight = API.GlobalLight;
            virtualCamera = API.VirtualCamera;
            virtualCamera.Follow = API.PlayerCharacter.transform;
            enemyManager = API.EnemyManager;

            API.PlayerCharacter.OnLevelUp += level => levelText.text = $"{level}";
        }

        void Update()
        {
            currentTime += Time.deltaTime;
        
            if (currentTime > interval)
            {
                interval += 30;
                enemyManager.limit = Math.Clamp(enemyManager.limit * 2, 0, 700);
            }
        }
    }
}
