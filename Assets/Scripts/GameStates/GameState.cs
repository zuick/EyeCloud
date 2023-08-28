using UnityEngine;
using UnityEngine.SceneManagement;
using Game.Services;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using PlayerInput = Game.Input.PlayerInput;

namespace Game.State
{
    [CreateAssetMenu(fileName = "GameState", menuName = "Data/GameState/Simple", order = 1)]
    public class GameState : ScriptableObject
    {
        public string SceneName;
        public LoadSceneMode SceneLoadMode;
        public bool isCursorVisible;

        protected IGameStatesService gameStatesService;
        protected IUISystem uiSystem;
        protected PlayerInput input;

        protected List<IDisposable> disposables = new();
        public virtual void Init(IGameStatesService gameStatesService, IUISystem uiSystem, PlayerInput input)
        {
            this.gameStatesService = gameStatesService;
            this.uiSystem = uiSystem;
            this.input = input;
        }

        public virtual void OnStartEnter(object data) { }

        public virtual async Task ProcessEnter() { }

        public virtual void OnEntered() { }

        public virtual void OnStartExit() { }

        public virtual async Task ProcessExit() { }

        public virtual void OnExited()
        {
            disposables.ForEach(d => d.Dispose());
        }
    }
}