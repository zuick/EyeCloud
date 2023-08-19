using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Core
{
    public class AIAbilityResolver : IAbilityResolver
    {
        private Level level;

        public AIAbilityResolver(Level level)
        {
            this.level = level;
        }

        public async Task<AbilityApplyData> GetAbility(Entity entity)
        {
            var delta = GetRandomDirectionPoint();
            var targetPosition = entity.Position + delta;
            var entityAtTargetPosition = level.GetAt(targetPosition);

            if (entityAtTargetPosition != null)
            {
                if (entity.TryGetAbility<Pass>(out var passAbility))
                {
                    return new AbilityApplyData(passAbility);
                }
            }
            else if (entity.TryGetAbility<MoveTo>(out var moveToAbility))
            {
                return new AbilityApplyData(moveToAbility, targetPosition);
            }

            return AbilityApplyData.Empty;
        }

        private IntPoint GetRandomDirectionPoint()
        {
            var r = Random.Range(0, 5);
            if (r == 0)
                return new IntPoint(-1, 0);
            else if (r == 1)
                return new IntPoint(1, 0);
            else if (r == 2)
                return new IntPoint(0, 1);

            return new IntPoint(0, -1);
        }
    }
}