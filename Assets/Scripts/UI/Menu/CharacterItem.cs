using DG.Tweening;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Menu
{
    public class CharacterItem : MonoBehaviour
    {
        private Image selectionFrame;
        [SerializeField]private Transform avatar;
        [SerializeField]private MMScaleShaker scaleShaker;

        private void Awake()
        {
            selectionFrame = GetComponent<Image>();
        }

        public void SelectedView()
        {
            var rectTransform = avatar.GetComponent<RectTransform>();
            rectTransform.DOSizeDelta(new Vector2(400, 400), 0.2f).SetEase(Ease.OutBack);
            // avatar.DOScale(new Vector3(2f, 2f, 1), 0.2f).SetEase(Ease.OutBack);
            rectTransform.DOAnchorPosX(15, 0.1f).SetEase(Ease.OutSine);
            selectionFrame.enabled = true;
            scaleShaker.enabled = true;
        }
        
        public void DeselectedView()
        {
            var rectTransform = avatar.GetComponent<RectTransform>();
            // avatar.DOScale(new Vector3(1f, 1f, 1), 0.2f).SetEase(Ease.InSine);
            rectTransform.DOSizeDelta(new Vector2(0, 0), 0.2f).SetEase(Ease.InSine);
            avatar.GetComponent<RectTransform>().DOAnchorPosX(0, 0.1f).SetEase(Ease.InSine);
            selectionFrame.enabled = false;
        }
    }
}
