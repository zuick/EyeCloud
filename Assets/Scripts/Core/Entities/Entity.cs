using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;
using UnityEngine;

namespace Game.Core
{
    public class Entity
    {
        public string Name { private set; get; }
        public int Id { private set; get; }
        public IntPoint Position { private set; get; }
        public EntityStats Stats { private set; get; }
        public bool IsDestroyed { private set; get; }

        public Action<AbilityApplyData> AbilityApplied;
        public Action<EntityStats, EntityStats> StatsChanged;

        private IAbilityResolver abilityResolver;
        private List<IAbility> abilities;

        public Entity(int id, string name, List<IAbility> abilities, IAbilityResolver abilityResolver)
        {
            Id = id;
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

        public void SetStats(EntityStats newValues)
        {
            var oldValues = Stats;
            newValues.HP = Mathf.Clamp(newValues.HP, 0, newValues.MaxHP);
            Stats = newValues;

            if (newValues.HP <= 0)
                IsDestroyed = true;

            StatsChanged?.Invoke(oldValues, newValues);
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
