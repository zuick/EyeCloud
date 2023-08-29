using System.Collections;
using UnityEngine;
using Game.Data;

namespace Game.Messages
{
    public class PlayerFinishLevel
    {
        public readonly bool IsWin;

        public PlayerFinishLevel(bool isWin)
        {
            IsWin = isWin;
        }
    }
}