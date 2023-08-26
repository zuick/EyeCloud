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
            var targetPosition = GetRandomPointAround(entity.Position);

            if (nearestTarget != null)
            {
                var targetDirection = (nearestTarget.Position - entity.Position).UnitDirection();
                var targetPositionCandidate = entity.Position + targetDirection;
                if (level.IsFree(targetPositionCandidate))
                {
                    targetPosition = targetPositionCandidate;
                }
            }

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

        private IntPoint GetRandomPointAround(IntPoint position)
        {
            var freePositions = level.GetFreePositionsAround(position);
            return freePositions[Random.Range(0, freePositions.Length)];
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