using UnityEngine;
using UnityEngine.SceneManagement;
using Game.State;
using PlayerInput = Game.Input.PlayerInput;

namespace Game.Services
{
    public class GameStatesService : IGameStatesService
    {
        public GameState Current { private set; get; }

        private IScenesService scenesService;
        private IUISystem uiSystem;
        private PlayerInput input;

        public GameStatesService(IScenesService scenesService, IUISystem uiSystem, PlayerInput input)
        {
            this.scenesService = scenesService;
            this.uiSystem = uiSystem;
            this.input = input;
        }

        public void Init(params GameState[] gameStates)
        {
            foreach (var state in gameStates)
            {
                state.Init(this, uiSystem, input);
            }
        }

        public async void Start(GameState gameState, object data = null)
        {
            if (Current != null)
            {
                Current.OnStartExit();
                await uiSystem.FaderService.FadeIn();
                await Current.ProcessExit();
                if (Current.SceneLoadMode == LoadSceneMode.Additive)
                {
                    await scenesService.UnloadScene(Current.SceneName);
                }
                Current.OnExited();
            }

            Current = gameState;

            Cursor.visible = gameState.isCursorVisible;
            gameState.OnStartEnter(data);
            await scenesService.LoadScene(gameState.SceneName, gameState.SceneLoadMode);
            await gameState.ProcessEnter();
            gameState.OnEntered();
            await uiSystem.FaderService.FadeOut();
            return;
        }
    }
}