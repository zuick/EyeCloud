using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core
{
    public class Game
    {
        public Level level;
        public Entity currentEntity;
        private bool stopped;

        public Game(Level level, Entity currentEntity)
        {
            this.level = level;
            this.currentEntity = currentEntity;
        }

        public async void Start()
        {
            Entity currentEntity = null;
            while (level.Entities.Count > 0 && !stopped)
            {
                currentEntity = level.GetNext(currentEntity);
                Debug.Log(currentEntity.Name + "?");
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

