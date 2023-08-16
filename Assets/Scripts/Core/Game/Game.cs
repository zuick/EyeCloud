using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core
{
    public class Game
    {
        public Level level;
        public Entity currentEntity;

        public async void Start()
        {
            foreach(var entity in level.Entities)
            {
                Debug.Log("turn: " + entity.Name);
                var ability = await entity.NextAction();
                ability.Invoke();
            }
        }
    }
}

