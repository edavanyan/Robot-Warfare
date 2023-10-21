using UnityEngine;
using UnityEngine.Rendering.Universal;
using CharacterController = PlayerController.CharacterController;

namespace Utils
{
    [RequireComponent(typeof(Light2D))]
    public class LightAnimation : MonoBehaviour
    {
        public Light2D light2D;
        public SpriteRenderer spriteRenderer;
        public CharacterController character;

        private void Start()
        {
            light2D = GetComponent<Light2D>();
        }

        void Update()
        {
            if (light2D.lightCookieSprite != spriteRenderer.sprite)
            {
                light2D.lightCookieSprite = character.shadowSprites[spriteRenderer.sprite.name];
            }
        }
    }
}
