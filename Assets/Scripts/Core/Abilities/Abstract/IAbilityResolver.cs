using System.Collections.Generic;
using System.Threading.Tasks;

namespace Game.Core
{
    public interface IAbilityResolver
    {
        public Task<IAbility> GetAbility(Entity entity, List<IAbility> abilities);
    }
}