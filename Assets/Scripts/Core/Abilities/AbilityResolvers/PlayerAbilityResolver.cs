using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Game.Core
{
    public class PlayerAbilityResolver : IAbilityResolver
    {
        private Level level;
        private IInputHandler inputHandler;
        private bool inputPressed =>
            inputHandler.Left ||
            inputHandler.Right ||
            inputHandler.Up ||
            inputHandler.Down;

        public PlayerAbilityResolver(Level level, IInputHandler inputHandler)
        {
            this.level = level;
            this.inputHandler = inputHandler;
        }

        public async Task<IAbility> GetAbility(Entity entity, List<IAbility> abilities)
        {
            while (!inputPressed)
            {
                await Task.Yield();
            }

            var delta = GetPointFromInput();
            var targetPosition = entity.Position + delta;
            var entityAtTargetPosition = level.GetAt(targetPosition);
            if (entityAtTargetPosition != null)
            {
                return new PassTurn();
            }
            return new MoveTo(targetPosition);
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