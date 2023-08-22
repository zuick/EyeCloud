using System;

namespace Game.Core
{
    [Serializable]
    public struct EntityStats
    {
        public int HP;
        public int MaxHP;
        public float AttackMultiplier;
        public int SightDistance;
    }
}
