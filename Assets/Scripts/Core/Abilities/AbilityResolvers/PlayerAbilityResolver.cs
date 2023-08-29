﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Game.Core
{
    public class PlayerAbilityResolver : AbilityResolver
    {
        private Level level;
        private IInputHandler inputHandler;
        private bool inputPressed =>
            inputHandler.Left ||
            inputHandler.Right ||
            inputHandler.Up ||
            inputHandler.Down ||
            inputHandler.PassTurn;

        public PlayerAbilityResolver(Level level, IInputHandler inputHandler)
        {
            this.level = level;
            this.inputHandler = inputHandler;
        }

        public override async Task<AbilityApplyData> GetAbility(Entity entity)
        {
            while (!inputPressed)
            {
                await Task.Yield();
            }

            if (!inputHandler.PassTurn)
            {
                var delta = GetPointFromInput();
                var targetPosition = entity.Position + delta;

                if (!level.IsFree(targetPosition))
                {
                    return GetPassAbility(entity); // TODO: wait for other input
                }

                var entityAtTargetPosition = level.GetAt(targetPosition);

                if (entityAtTargetPosition != null)
                {
                    if (entityAtTargetPosition.HasAbility<ProcessInteractionAbility>())
                    {
                        if (entity.TryGetAbility<Interact>(out var interactAbility))
                        {
                            return new AbilityApplyData(interactAbility, entityAtTargetPosition);
                        }
                    }
                    else if (entity.FractionId != entityAtTargetPosition.FractionId)
                    {
                        if (entity.TryGetAbility<MeleeAttack>(out var attackAbility))
                        {
                            return new AbilityApplyData(attackAbility, entityAtTargetPosition);
                        }
                    }
                }
                else
                {
                    var entityAtTargetDirection = GetNearestAttackTarget(entity, delta);
                    if (entityAtTargetDirection != null)
                    {
                        if (entity.TryGetAbility<MeleeAttack>(out var attackAbility))
                        {
                            return new AbilityApplyData(attackAbility, entityAtTargetDirection);
                        }
                    }
                    else if (entity.TryGetAbility<MoveTo>(out var moveToAbility))
                    {
                        return new AbilityApplyData(moveToAbility, targetPosition);
                    }
                }
            }

            return GetPassAbility(entity);
        }

        private Entity GetNearestAttackTarget(Entity searcher, IntPoint delta)
        {
            return level.Get(
                e => e.FractionId != searcher.FractionId &&
                    (e.Position - searcher.Position).Normilized.Equals(delta) &&
                    searcher.Position.MaxDistanceTo(e.Position) <= searcher.Stats.AttackDistance
            );
        }

        private IntPoint GetPointFromInput()
        {
            if (inputHandler.Left)
                return new IntPoint(-1, 0);
            else if (inputHandler.Right)
                return new IntPoint(1, 0);
            else if (inputHandler.Up)
                return new IntPoint(0, 1);
            else if (inputHandler.Down)
                return new IntPoint(0, -1);

            return new IntPoint(0, 0);
        }
    }
}