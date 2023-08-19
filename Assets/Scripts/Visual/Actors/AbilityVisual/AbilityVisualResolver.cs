using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;

namespace Game.Visual
{
    public class AbilityVisualResolver : MonoBehaviour
    {
        [Serializable]
        public class AbilityKeyPair
        {
            public Ability Ability;
            public AbilityVisual Visual;
        }

        [SerializeField] private List<AbilityKeyPair> abilityToVisual;

        public static AbilityVisualResolver Instance;

        private void Awake()
        {
            Instance = this;
        }

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

        public List<IAbility> GetAllAbilities()
        {
            return abilityToVisual.Select(p => (IAbility)p.Ability).ToList();
        }
    }
}
