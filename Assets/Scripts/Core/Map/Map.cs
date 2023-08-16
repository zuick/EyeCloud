using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core
{
    public class Map
    {
        private MapCell[][] data;

        public Map(int sizeX, int sizeY)
        {
            for (int i = 0; i < sizeX; i++)
            {
                for (int j = 0; j < sizeY; j++)
                {
                    data[i][j] = new MapCell();
                }
            }
        }

        public bool TryGet(IntPoint pos, out MapCell result)
        {
            result = null;
            if (pos.X < data.Length && pos.Y < data[pos.X].Length)
            {
                result = data[pos.X][pos.Y];
                return true;
            }

            return false;
        }
    }
}
