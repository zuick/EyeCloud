using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;
using Game.Data;

namespace Game.Visual
{
    [CreateAssetMenu(fileName = "VisualResolver", menuName = "Data/VisualResolver", order = 1)]
    public class VisualResolver : ScriptableObject
    {
        [Serializable]
        public class AbilityKeyPair
        {
            public Ability Ability;
            public AbilityVisual Visual;
        }

        [Serializable]
        public class EntityActorKeyPair
        {
            public EntityData Data;
            public EntityActor Actor;
        }

        [SerializeField] private List<AbilityKeyPair> abilityToVisual;
        [SerializeField] private List<EntityActorKeyPair> entityToActor;

        public bool TryGet(Ability ability, out AbilityVisual visual)
        {
            visual = null;
            var candidate = abilityToVisual.FirstOrDefault(p => p.Ability == ability);
            if (candidate != null)
            {
                visual = candidate.Visual;
                return true;
            }

            return false;
        }

        public bool TryGet(EntityData entityData, out EntityActor actor)
        {
            actor = null;
            var candidate = entityToActor.FirstOrDefault(p => p.Data == entityData);
            if (candidate != null)
            {
                actor = candidate.Actor;
                return true;
            }

            return false;
        }
    }
}
