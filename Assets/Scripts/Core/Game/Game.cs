using UnityEngine;
using System;

namespace Game.Core
{
    public class Game
    {
        public Level level;
        public Action<Entity> CurrentEntityChanged;
        public Action PlayerWin;
        public Action PlayerLose;

        private bool stopped;
        private int playerFraction;
        private int enemyFraction;
        private int spawnerFraction;

        public Game(Level level, int playerFraction, int enemyFraction, int spawnerFraction)
        {
            this.level = level;
            this.playerFraction = playerFraction;
            this.enemyFraction = enemyFraction;
            this.spawnerFraction = spawnerFraction;
        }

        public async void Start()
        {
            Entity currentEntity = null;
            while (level.Entities.Count > 0 && !stopped)
            {
                currentEntity = level.GetNext(currentEntity);
                CurrentEntityChanged?.Invoke(currentEntity);
                Debug.Log($"{currentEntity.Name}({currentEntity.Id}/{currentEntity.FractionId})?");
                await currentEntity.NextAction();
                level.RemoveDestroyed();

                if (!level.HasEntities(enemyFraction) && !level.HasEntities(spawnerFraction))
                {
                    PlayerWin?.Invoke();
                    break;
                }

                if (!level.HasEntities(playerFraction))
                {
                    PlayerLose?.Invoke();
                    break;
                }
            }
        }

        public void Stop()
        {
            stopped = true;
        }
    }
}

