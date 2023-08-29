using UnityEngine;
using Game.UI;
using UnityEngine.InputSystem;
using Game.Data;

namespace Game.State
{
    [CreateAssetMenu(fileName = "MainMenu", menuName = "Data/GameState/MainMenu", order = 1)]
    public class MainMenuGameState : GameState
    {
        [SerializeField]
        private GameState NextState;
        [SerializeField]
        private LevelsData levelsData;

        private MainMenuWindow mainMenuWindow;
        private ConfirmWindow exitWindow;

        public override void OnEntered()
        {
            base.OnEntered();
            mainMenuWindow = uiSystem.WindowsService.Open<MainMenuWindow>();
            mainMenuWindow.OnStartGame += OnStartGame;
            input.Player.Cancel.performed += OnExitGameIntention;
        }

        private void OnExitGameIntention(InputAction.CallbackContext ctx)
        {
            if (exitWindow == null)
            {
                exitWindow = uiSystem.WindowsService.Open<ConfirmWindow>("Выйти из игры?");
                exitWindow.OnSubmit += () => Application.Quit();
                exitWindow.OnCancel += () =>
                {
                    exitWindow.CloseSelf();
                    exitWindow = null;
                };
            }
        }

        public override void OnExited()
        {
            uiSystem.WindowsService.Close(mainMenuWindow);
            input.Player.Cancel.performed -= OnExitGameIntention;
        }

        private void OnStartGame()
        {
            gameStatesService.Start(NextState, levelsData.First);
        }
    }
}