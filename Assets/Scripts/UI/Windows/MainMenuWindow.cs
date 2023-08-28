using UnityEngine;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.InputSystem;
using System;

namespace Game.UI
{
    public class MainMenuWindow : Window
    {
        public Action OnStartGame;

        public override void Init(object data)
        {
            input.Player.Submit.performed += OnStartGameIntention;
        }

        private void OnStartGameIntention(InputAction.CallbackContext ctx)
        {
            if (IsActive)
            {
                OnStartGame?.Invoke();
            }
        }

        private void OnDestroy()
        {
            input.Player.Submit.performed -= OnStartGameIntention;
        }
    }
}