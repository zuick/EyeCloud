using System.Collections;
using UnityEngine;
using Game.Data;

namespace Game.Messages
{
    public class StartLevelMessage
    {
        public LevelData LevelData;

        public StartLevelMessage(LevelData levelData)
        {
            LevelData = levelData;
        }
    }
}