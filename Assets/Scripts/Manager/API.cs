using System.Collections.Generic;
using Cameras;
using Cinemachine;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using CharacterController = PlayerController.CharacterController;

namespace Manager
{
    public class API : MonoBehaviour
    {
        public static CharacterController PlayerCharacter => instance.playerCharacter;
        public static SmoothCamera2D Camera2D => instance.camera2D;
        public static EnemyManager EnemyManager => instance.enemyManager;
        public static CinemachineVirtualCamera VirtualCamera => instance.virtualCamera;
        public static GameCanvas GameCanvas => instance.gameCanvas;
        public static LootManager LootManager => instance.lootManager;
        public static VfxManager VfxManager => instance.vfxManager;
        public static GameManager GameManager => instance.gameManager;

        private CharacterController playerCharacter;
        private SmoothCamera2D camera2D;
        private CinemachineVirtualCamera virtualCamera;
        private GameCanvas gameCanvas;
        private EnemyManager enemyManager;
        private LootManager lootManager;
        private VfxManager vfxManager;
        private GameManager gameManager;
        
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
                virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
                gameCanvas = FindObjectOfType<GameCanvas>();
                lootManager = FindObjectOfType<LootManager>();
                vfxManager = FindObjectOfType<VfxManager>();
                gameManager = FindObjectOfType<GameManager>();
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
            character.OnLowHealth += percentage => VfxManager.ShowRedScreen(1f - percentage);
        }

        public void Quit()
        {
            SceneManager.LoadScene("Menu");
        }
    }
}
