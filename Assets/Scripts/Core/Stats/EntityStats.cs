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

        public static EntityStats operator +(EntityStats a, EntityStats b) => new EntityStats()
        {
            HP = a.HP + b.HP,
            MaxHP = a.MaxHP + b.MaxHP,
            AttackMultiplier = a.AttackMultiplier + b.AttackMultiplier,
            SightDistance = a.SightDistance + b.SightDistance,
        };

        public static EntityStats operator -(EntityStats a, EntityStats b) => new EntityStats()
        {
            HP = a.HP - b.HP,
            MaxHP = a.MaxHP - b.MaxHP,
            AttackMultiplier = a.AttackMultiplier - b.AttackMultiplier,
            SightDistance = a.SightDistance - b.SightDistance,
        };

        public static EntityStats Empty => new EntityStats()
        {
            HP = 0,
            MaxHP = 0,
            AttackMultiplier = 0,
            SightDistance = 0,
        };
    }
}
