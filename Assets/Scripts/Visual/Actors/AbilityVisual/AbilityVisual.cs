using System;
using UnityEngine;
using Game.Core;

namespace Game.Visual
{
    [CreateAssetMenu(fileName = "AbilityVisual", menuName = "Data/Abilities Visual/AbilityVisual", order = 1)]
    public class AbilityVisual : ScriptableObject
    {
        public virtual void Perform(EntityActor actor, AbilityApplyData applyData) { }
    }
}
