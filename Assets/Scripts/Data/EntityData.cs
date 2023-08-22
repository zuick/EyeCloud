using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;

namespace Game.Data
{
    [CreateAssetMenu(fileName = "EntityData", menuName = "Data/Entities/EntityData", order = 1)]
    public class EntityData : ScriptableObject
    {
        public string Name;
        public List<Ability> Abilities;
        public EntityStats Stats;

        public Entity Create(int id, int fractionId, int x, int y, IAbilityResolver abilityResolver)
        {
            var entity = new Entity(
                id,
                fractionId,
                Name,
                Abilities.Select(a => (IAbility)a).ToList(),
                abilityResolver
            );

            entity.SetPosition(x, y);
            entity.SetStats(Stats);
            return entity;
        }
    }
}
