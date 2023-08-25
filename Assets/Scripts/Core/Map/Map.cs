using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core
{
    public class Map
    {
        private MapCellType[][] data;

        public Map(int sizeX, int sizeY)
        {
            for (int i = 0; i < sizeX; i++)
            {
                for (int j = 0; j < sizeY; j++)
                {
                    data[i][j] = MapCellType.Empty;
                }
            }
        }
    }
}
