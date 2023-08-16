using System.Collections.Generic;
using System.Threading.Tasks;

namespace Game.Core
{
    public class AIAbilityResolver : IAbilityResolver
    {
        private Level level;

        public AIAbilityResolver(Level level)
        {
            this.level = level;
        }

        public async Task<IAbility> GetAbility(Entity entity, List<IAbility> abilities)
        {
            await Task.Delay(1000);
            return new PassTurn();
        }
    }
}