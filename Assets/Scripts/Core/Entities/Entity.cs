using System.Collections.Generic;
using System.Threading.Tasks;

namespace Game.Core
{
    public class Entity
    {
        public IntPoint Position { private set; get; }
        public string Name { private set; get; }

        private IAbilityResolver abilityResolver;
        private List<IAbility> abilities;

        public Entity(string name, int x, int y, IAbilityResolver abilityResolver)
        {
            Name = name;
            Position = new IntPoint(x, y);
            this.abilityResolver = abilityResolver;
        }

        public async Task<IAbility> NextAction()
        {
            return await abilityResolver.GetAbility(this, abilities);
        }

        public void SetPosition(IntPoint position)
        {
            Position = position;
        }
    }
}
