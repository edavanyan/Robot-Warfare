using DG.Tweening;
using MoreMountains.Feedbacks;
using PlayerController;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.Menu
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private CharacterSelection characterSelection;
        [SerializeField] private Button startButton;
        [SerializeField] private TextMeshProUGUI startButtonText;
        [SerializeField] private Image backButton;
        [SerializeField] private Image background;
        private State state = State.Menu;
        
        public void OnPlayButtonPress()
        {
            if (state == State.HeroSelection)
            {
                API.CharacterName = characterSelection.SelectedCharacter().name;
                SceneManager.LoadScene("Siege");
            }
            else
            {
                TransitionToHeroSelection();
            }
        }

        public void OnBackButtonPress()
        {
            TransitionToMainMenu();
        }

        public void TransitionToHeroSelection()
        {
            startButton.transform.parent.GetComponent<MMScaleShaker>().enabled = true;
            startButton.interactable = false;
            backButton.gameObject.SetActive(true);
            backButton.GetComponent<MMScaleShaker>().enabled = true;
            background.DOFade(0.025f, 0.3f).OnComplete(() =>
            {
                startButton.interactable = true;
                startButtonText.text = "PLAY";
                characterSelection.gameObject.SetActive(true);
            });
            state = State.HeroSelection;
        }

        public void TransitionToMainMenu()
        {
            startButtonText.text = "START";
            backButton.gameObject.SetActive(false);
            characterSelection.gameObject.SetActive(false);
            background.DOFade(1f, 0.3f);
            state = State.Menu;
        }

        private enum State
        {
            Menu,
            HeroSelection
        }
    }
}
