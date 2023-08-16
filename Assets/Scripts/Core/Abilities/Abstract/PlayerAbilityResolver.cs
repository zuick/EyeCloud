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


            return new PassTurn();
        }
    }
}