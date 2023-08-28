using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;

namespace Game.Data
{
    [CreateAssetMenu(fileName = "EntityData", menuName = "Data/EntityData", order = 1)]
    public class EntityData : ScriptableObject
    {
        public string Name;
        public EntityFraction Fraction;
        public List<Ability> Abilities;
        public EntityStats Stats;
        public bool IsPlayable;

        public Entity Create(int id, IntPoint position, IAbilityResolver abilityResolver)
        {
            var entity = new Entity(
                id,
                (int)Fraction,
                Name,
                Abilities.Select(a => (IAbility)a).ToList(),
                abilityResolver
            );

            entity.SetPosition(position);
            entity.SetStats(Stats);
            return entity;
        }
    }
}
