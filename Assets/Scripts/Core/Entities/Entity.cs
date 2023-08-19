using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;
using UnityEngine;

namespace Game.Core
{
    public class Entity
    {
        public IntPoint Position { private set; get; }
        public string Name { private set; get; }
        public Action<AbilityApplyData> AbilityApplied;

        private IAbilityResolver abilityResolver;
        private List<IAbility> abilities;

        public Entity(string name, List<IAbility> abilities, IAbilityResolver abilityResolver)
        {
            Name = name;
            this.abilities = abilities;
            this.abilityResolver = abilityResolver;
        }

        public async Task NextAction()
        {
            var applyData = await abilityResolver.GetAbility(this);

            if (applyData.Ability != null)
            {
                Debug.Log(Name + ": " + applyData.Ability.Name);

                applyData.Ability.Apply(this, applyData.Data);

                AbilityApplied?.Invoke(applyData);

                await Task.Delay((int)(applyData.Ability.Duration * 1000f));
            }
            else
            {
                Debug.Log(Name + ": no possible ability");
                await Task.Delay(200);
            }
        }

        public bool TryGetAbility<T>(out IAbility ability) where T : IAbility
        {
            ability = abilities.FirstOrDefault(a => a is T);
            return ability != null;
        }

        public void SetPosition(IntPoint position)
        {
            Position = position;
        }

        public void SetPosition(int x, int y)
        {
            Position = new IntPoint(x, y);
        }
    }
}
