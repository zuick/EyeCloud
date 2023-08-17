using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace Game.Core
{
    public class Entity
    {
        public IntPoint Position { private set; get; }
        public string Name { private set; get; }
        public Action<IAbility> AbilityApplied;

        private IAbilityResolver abilityResolver;
        private List<IAbility> abilities;

        public Entity(string name, int x, int y, IAbilityResolver abilityResolver)
        {
            Name = name;
            Position = new IntPoint(x, y);
            this.abilityResolver = abilityResolver;
        }

        public async Task NextAction()
        {
            var ability = await abilityResolver.GetAbility(this, abilities);
            ability.Invoke(this);
            AbilityApplied?.Invoke(ability);
        }

        public void SetPosition(IntPoint position)
        {
            Position = position;
        }
    }
}
