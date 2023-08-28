using UnityEngine;
using Zenject;
using Game.Services;
using Game.State;

namespace Game.Startup
{
    public class Startup : MonoBehaviour
    {
        [SerializeField]
        private GameState[] GameStates;

        private IGameStatesService gameStatesService;

        [Inject]
        public void Constract(IGameStatesService gameStatesService)
        {
            this.gameStatesService = gameStatesService;
        }

        private void Awake()
        {
            gameStatesService.Init(GameStates);
            gameStatesService.Start(GameStates[0]);
        }
    }
}