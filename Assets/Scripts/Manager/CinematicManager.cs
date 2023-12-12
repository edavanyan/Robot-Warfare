using Cinemachine;
using DG.Tweening;
using Loots;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using CharacterController = PlayerController.CharacterController;

namespace Manager
{
    public class CinematicManager : MonoBehaviour
    {
        public Volume volume;
        public Light2D globalLight;
        public new CinemachineVirtualCamera camera;
        public EnemyManager enemyManager;

        public CharacterController Verdan;
        public CharacterController Luna;
        public CharacterController Alden;

        public Transform verdanPosition; 
        public Transform lunaPosition; 
        public Transform cameraPosition; 

        public static CinematicManager instance;
        
        private void Start()
        {
            instance = this;
            DOTween.Sequence()
                .AppendCallback(() => camera.gameObject.SetActive(true))
                .Append(DOTween.To(() => camera.m_Lens.OrthographicSize, x => camera.m_Lens.OrthographicSize = x,
                -4, 0.5f).SetRelative(true).SetEase(Ease.OutQuint)).SetDelay(2);
        }

        public void CameraZoomOut(float amount)
        {
            DOTween.To(() => camera.m_Lens.OrthographicSize, x => camera.m_Lens.OrthographicSize = x,
                amount, 2).SetRelative(true).SetEase(Ease.OutQuint);
        }

        public void SetToDropSword()
        {
            enemyManager.limit = 10;
            enemyManager.currentLootType = LootType.Sword;
        }

        public void SetToDropXp()
        {
            enemyManager.currentLootType = LootType.Xp;
        }

        public void SetEnemyCountHuge()
        {
            enemyManager.limit = 700;
        }

        public void LensEffect(TweenCallback callback)
        {
            volume.profile.TryGet<LensDistortion>(out var lensDistortion);
            DOTween.Sequence()
                .Append(
                    DOTween.To(() => lensDistortion.intensity.value, value => lensDistortion.intensity.value = value,
                            0.2f, 0.3f)
                        .SetRelative(true)) //.SetEase(Ease.OutSine))
                .Join(DOTween.To(() => globalLight.intensity, x => globalLight.intensity = x,
                        5, 0.6f))
                .Join(DOTween.To(() => camera.m_Lens.OrthographicSize, x => camera.m_Lens.OrthographicSize = x,
                    -3, 0.6f).SetRelative(true))
                .Append(
                    DOTween.To(() => lensDistortion.intensity.value, value => lensDistortion.intensity.value = value,
                            -1f, 0.3f)
                        .SetRelative(true)) //.SetEase(Ease.OutSine))
                .Append(
                    DOTween.To(() => lensDistortion.intensity.value, value => lensDistortion.intensity.value = value,
                        0, 0.3f).SetEase(Ease.OutBounce))
                .Join(DOTween.To(() => globalLight.intensity, x => globalLight.intensity = x,
                    1, 0.3f))
                .InsertCallback(0.8f, callback)
                .Append(DOTween.To(() => camera.m_Lens.OrthographicSize, x => camera.m_Lens.OrthographicSize = x,
                    3, 5).SetRelative(true));
        }

        public void SetLuna()
        {
            LensEffect(() =>
            {
                // camera.Follow.gameObject.SetActive(false);
                Luna.transform.position = camera.Follow.position;
                camera.Follow = Luna.transform;
                camera.Follow.gameObject.SetActive(true);
                API.SetCharacter(Luna);
                Verdan.transform.position = verdanPosition.position;
                Verdan.transform.localScale = new Vector3(1, 1, 1);
            });
        }

        public void SetAlden()
        {
            LensEffect(() =>
            {
                // camera.Follow.gameObject.SetActive(false);
                Alden.transform.position = camera.Follow.position;
                camera.Follow = Alden.transform;
                camera.Follow.gameObject.SetActive(true);
                API.SetCharacter(Alden);
                Luna.transform.position = lunaPosition.position;
                Luna.transform.localScale = new Vector3(-1, 1, 1);
            });
        }

        private bool cameraFixed = false;
        private void Update()
        {
            if (!cameraFixed)
            {
                if (camera.transform.position.x >= cameraPosition.position.x - 2)
                {
                    cameraFixed = true;
                    camera.Follow = cameraPosition;
                    DOTween.Sequence()
                        .Append(DOTween.To(() => camera.m_Lens.OrthographicSize,
                            x => camera.m_Lens.OrthographicSize = x,
                            -2, 0.6f).SetRelative(true))
                        .Append(DOTween.To(() => camera.m_Lens.OrthographicSize,
                            x => camera.m_Lens.OrthographicSize = x,
                            2, 2f).SetRelative(true));

                }
            }
        }
    }
}
