using System;

namespace Game.Core
{
    public struct AbilityApplyData
    {
        public readonly IAbility Ability;
        public readonly object Data;

        public AbilityApplyData(IAbility ability, object data = null)
        {
            Ability = ability;
            Data = data;
        }

        public static AbilityApplyData Empty => new AbilityApplyData(null, null);
    }
}
