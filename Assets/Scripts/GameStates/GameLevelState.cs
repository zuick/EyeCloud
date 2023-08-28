using UnityEngine;
using Game.UI;
using UnityEngine.InputSystem;

namespace Game.State
{
    [CreateAssetMenu(fileName = "GameLevelState", menuName = "Data/GameState/GameLevelState", order = 1)]
    public class GameLevelState : GameState
    {
        [SerializeField]
        private GameState MainMenuGameState;

        private GameHUDWindow gameHUD;
        private ConfirmWindow exitWindow;

        public override void OnStartEnter(object data)
        {
            gameHUD = uiSystem.WindowsService.Open<GameHUDWindow>();

        }

        public override void OnEntered()
        {
            input.Player.Cancel.performed += OnExitGameIntention;
        }

        private void OnExitGameIntention(InputAction.CallbackContext ctx)
        {
            if (exitWindow == null)
            {
                exitWindow = uiSystem.WindowsService.Open<ConfirmWindow>("Выйти в меню?");
                exitWindow.OnSubmit += () =>
                {
                    exitWindow.CloseSelf();
                    exitWindow = null;

                    gameStatesService.Start(MainMenuGameState);
                };

                exitWindow.OnCancel += () =>
                {
                    exitWindow.CloseSelf();
                    exitWindow = null;
                };
            }
        }

        public override void OnExited()
        {
            base.OnExited();
            uiSystem.WindowsService.Close(gameHUD);
            input.Player.Cancel.performed -= OnExitGameIntention;
        }
    }
}