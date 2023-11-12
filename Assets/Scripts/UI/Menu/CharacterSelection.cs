using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI.Menu
{
    public class CharacterSelection : MonoBehaviour
    {
        [SerializeField] private List<CharacterItem> characters;
        private int selectedIndex = 0;

        private void Start()
        {
            GameInput.InputActions.Instance.Game.Movement.performed += CaptureInput;
            GameInput.InputActions.Instance.Game.Movement.canceled += CaptureInput;
        }

        private void OnDestroy()
        {
            GameInput.InputActions.Instance.Game.Movement.performed -= CaptureInput;
            GameInput.InputActions.Instance.Game.Movement.canceled -= CaptureInput;
        }

        private void CaptureInput(InputAction.CallbackContext context)
        {
            var input = context.ReadValue<Vector2>();
            var nextIndex = Math.Clamp(Math.Sign(input.x) + selectedIndex, 0, characters.Count - 1);
            SelectCharacter(characters[nextIndex]);
        }

        public void SelectCharacter(CharacterItem characterItem)
        {
            characters[selectedIndex].DeselectedView();
            selectedIndex = characters.IndexOf(characterItem);
            characters[selectedIndex].SelectedView();
        }

        public CharacterItem SelectedCharacter()
        {
            return characters[selectedIndex];
        }
    }
}
