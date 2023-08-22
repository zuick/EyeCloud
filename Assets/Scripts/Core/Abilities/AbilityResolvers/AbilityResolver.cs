using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Core
{
    public abstract class AbilityResolver : IAbilityResolver
    {
        public virtual async Task<AbilityApplyData> GetAbility(Entity entity)
        {
            return GetPassAbility(entity);
        }

        protected AbilityApplyData GetPassAbility(Entity entity)
        {
            if (entity.TryGetAbility<Pass>(out var passAbility))
            {
                return new AbilityApplyData(passAbility);
            }

            return AbilityApplyData.Empty;
        }
    }
}