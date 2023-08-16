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
            while (!stopped)
            {
                foreach(var entity in level.Entities)
                {
                    Debug.Log("turn: " + entity.Name);
                    var ability = await entity.NextAction();
                    ability.Invoke();
                }
            }
        }

        public void Stop()
        {
            stopped = true;
        }
    }
}

