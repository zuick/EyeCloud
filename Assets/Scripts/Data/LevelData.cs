using System;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;

namespace Game.Data
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "Data/LevelData", order = 1)]
    public class LevelData : ScriptableObject, ISerializationCallbackReceiver
    {
        public IntPoint MapSize;
        public MapCellType[,] Map;
        public List<EnityPositionData> Entities = new();
        public string Description;
        public List<MapCellList> mapSerialized;

        public void OnAfterDeserialize()
        {
            Map = new MapCellType[MapSize.Y, MapSize.X];

            if (mapSerialized == null)
                return;

            for (var i = 0; i < MapSize.Y; i++)
            {
                for (var j = 0; j < MapSize.X; j++)
                {
                    Map[i, j] = mapSerialized[i].Data[j];
                }
            }
        }

        public void OnBeforeSerialize()
        {
            if (Map == null)
                return;

            mapSerialized = new List<MapCellList>();
            for (var i = 0; i < MapSize.Y; i++)
            {
                var row = new MapCellList();
                for (var j = 0; j < MapSize.X; j++)
                {
                    row.Data.Add(Map[i, j]);
                }
                mapSerialized.Add(row);
            }
        }
    }

    [Serializable]
    public class MapCellList
    {
        public List<MapCellType> Data = new();
    }

    [Serializable]
    public class EnityPositionData
    {
        public EntityData entityData;
        public IntPoint position;
    }
}
