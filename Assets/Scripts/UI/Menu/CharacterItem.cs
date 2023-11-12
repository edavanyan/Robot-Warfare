using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Menu
{
    public class CharacterItem : MonoBehaviour
    {
        private Image selectionFrame;
        [SerializeField]private Transform avatar;

        private void Awake()
        {
            selectionFrame = GetComponent<Image>();
        }

        public void SelectedView()
        {
            avatar.DOScale(new Vector3(2f, 2f, 1), 0.2f).SetEase(Ease.OutBack);
            avatar.GetComponent<RectTransform>().DOAnchorPosX(15, 0.1f).SetEase(Ease.OutSine);
            selectionFrame.enabled = true;
            
        }
        
        public void DeselectedView()
        {
            avatar.DOScale(new Vector3(1f, 1f, 1), 0.2f).SetEase(Ease.InSine);
            avatar.GetComponent<RectTransform>().DOAnchorPosX(0, 0.1f).SetEase(Ease.InSine);
            selectionFrame.enabled = false;
        }
    }
}
