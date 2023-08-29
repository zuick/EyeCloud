using UnityEngine;
using Game.UI;
using Game.Data;
using Game.Services;
using Game.Messages;
using UnityEngine.InputSystem;
using System;

namespace Game.State
{
    [CreateAssetMenu(fileName = "GameLevelState", menuName = "Data/GameState/GameLevelState", order = 1)]
    public class GameLevelState : GameState
    {
        [SerializeField] private GameState MainMenuGameState;
        [SerializeField] private LevelsData levelsData;

        private GameHUDWindow gameHUD;
        private ConfirmWindow exitWindow;
        private LevelData levelData;

        public override void OnStartEnter(object data)
        {
            gameHUD = uiSystem.WindowsService.Open<GameHUDWindow>();
            levelData = data as LevelData;
        }

        public override void OnEntered()
        {
            input.Player.Cancel.performed += OnExitGameIntention;

            disposables.Add(MessagesService.Subscribe<PlayerFinishLevel>(OnPlayerFinishLevel));

            MessagesService.Publish(new StartLevelMessage(levelData));
        }

        private void OnPlayerFinishLevel(PlayerFinishLevel e)
        {
            if (e.IsWin)
            {
                if (levelsData.TryGetNext(levelData, out var nextLevelData))
                {
                    gameStatesService.Start(this, nextLevelData, true);
                }
                else
                {
                    gameStatesService.Start(MainMenuGameState);
                }
            }
            else
            {
                gameStatesService.Start(this, levelData, true);
            }
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