using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Manager
{
    public class VfxManager : MonoBehaviour
    {
        [SerializeField]private ParticleSystem levelUpEffect;
        private Bloom bloom;
        [SerializeField]private Volume globalVolume;
        private static TweenerCore<float, float, FloatOptions> redScreenPulse;

        private void Start()
        {
            globalVolume.profile.TryGet(out bloom);
            var effectTransform = levelUpEffect.transform;
            effectTransform.SetParent(API.PlayerCharacter.transform, false);
            effectTransform.position = API.PlayerCharacter.transform.position;
        }

        public void LevelUpEffect()
        {
            levelUpEffect.gameObject.SetActive(true);
            levelUpEffect.Play();
        }

        public void ShowRedScreen(float intensity)
        {
            if (!bloom.active)
            {
                bloom.active = true;
            }
            bloom.intensity.value = 0;
            redScreenPulse?.Kill();
            redScreenPulse = DOTween.To(() => bloom.intensity.value, i => bloom.intensity.value = i, intensity, 1f).SetLoops(-1, LoopType.Restart).SetEase(Ease.InOutSine);
        }

        public void RemoveRedScreen()
        {
            redScreenPulse.Kill();
            bloom.active = false;
        }
    }
}
