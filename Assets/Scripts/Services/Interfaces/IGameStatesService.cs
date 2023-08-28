using Game.State;

namespace Game.Services
{
    public interface IGameStatesService
    {
        GameState Current { get; }
        void Init(params GameState[] gameStates);
        void Start(GameState gameState, object data = null);
    }
}