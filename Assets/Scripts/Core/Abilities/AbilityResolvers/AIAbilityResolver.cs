using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Core
{
    public class AIAbilityResolver : AbilityResolver
    {
        private Level level;

        public AIAbilityResolver(Level level)
        {
            this.level = level;
        }

        public override async Task<AbilityApplyData> GetAbility(Entity entity)
        {
            var nearestTarget = GetNearestTarget(entity);
            IntPoint delta;
            if (nearestTarget != null)
            {
                delta = (nearestTarget.Position - entity.Position).UnitDirection();
            }
            else
            {
                delta = GetRandomDirectionPoint();
            }

            var targetPosition = entity.Position + delta;
            var entityAtTargetPosition = level.GetAt(targetPosition);

            if (entityAtTargetPosition != null)
            {
                if (entity.FractionId != entityAtTargetPosition.FractionId &&
                    entity.TryGetAbility<MeleeAttack>(out var attackAbility))
                {
                    return new AbilityApplyData(attackAbility, entityAtTargetPosition);
                }
            }
            else if (entity.TryGetAbility<MoveTo>(out var moveToAbility))
            {
                return new AbilityApplyData(moveToAbility, targetPosition);
            }

            return GetPassAbility(entity);
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

        private Entity GetNearestTarget(Entity searcher)
        {
            return level.Get(
                e => e.FractionId != searcher.FractionId &&
                searcher.Position.MaxDistanceTo(e.Position) <= searcher.Stats.SightDistance
            );
        }
    }
}