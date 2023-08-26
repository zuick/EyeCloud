using UnityEngine;

namespace Game.Core
{
    public class Game
    {
        public Level level;
        private bool stopped;

        public Game(Level level)
        {
            this.level = level;
        }

        public async void Start()
        {
            Entity currentEntity = null;
            while (level.Entities.Count > 0 && !stopped)
            {
                currentEntity = level.GetNext(currentEntity);
                Debug.Log($"{currentEntity.Name}({currentEntity.Id}/{currentEntity.FractionId})?");
                await currentEntity.NextAction();
                level.RemoveDestroyed();
            }
        }

        public void Stop()
        {
            stopped = true;
        }
    }
}

