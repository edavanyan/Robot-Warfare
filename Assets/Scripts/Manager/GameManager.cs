using System;
using Cinemachine;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Manager
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI levelText;
        private CinemachineVirtualCamera virtualCamera;
        private EnemyManager enemyManager;

        private float currentTime = 0f;
        private int interval = 15;
        private int difficultyInterval = 60;

        private int difficulty = 0;

        private void Awake()
        {
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
        
        public void GameOver()
        {
            API.GameCanvas.ShowGameOverScreen(API.VfxManager.RemoveRedScreen);
        }
    }
}
