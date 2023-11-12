using PlayerController;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.Menu
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField]private CharacterSelection characterSelection;
        
        public void OnPlayButtonPress()
        {
            API.CharacterName = characterSelection.SelectedCharacter().name;
            SceneManager.LoadScene("Siege");
        }
    }
}
