using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Core
{
    public class SpawnerAbilityResolver : AbilityResolver
    {
        private Level level;
        private int stateTick = -1;
        private const int spawnOnTick = 2;
        private const int moveOnTick = 0;
        private const int maxTick = 3;

        public SpawnerAbilityResolver(Level level)
        {
            this.level = level;
        }

        public override async Task<AbilityApplyData> GetAbility(Entity entity)
        {
            stateTick++;
            if (stateTick >= maxTick)
            {
                stateTick = 0;
            }

            if (stateTick == moveOnTick)
            {
                if (entity.TryGetAbility<MoveTo>(out var moveToAbility))
                {
                    return new AbilityApplyData(moveToAbility, level.GetRandomPosition());
                }
            }
            else if (stateTick == spawnOnTick)
            {
                if (entity.TryGetAbility<SpawnEntity>(out var spawnAbility))
                {
                    return new AbilityApplyData(spawnAbility, entity.Position);
                }
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