using System;
using UnityEngine;
using Game.Core;

namespace Game.Visual
{
    public class EntityActor : MonoBehaviour
    {
        public IntPoint EntityPosition => entity.Position;

        private Entity entity;

        public virtual void Init(Entity entity)
        {
            this.entity = entity;
            transform.position = VisualConfig.ToWorld(entity.Position);

            entity.AbilityApplied += OnAbilityApplied;
        }

        protected virtual void OnAbilityApplied(IAbility ability)
        {
            if (AbilityVisualResolver.Instance.TryGet(ability, out var visual))
            {
                visual.Perform(this, ability);
            }
        }

        protected virtual void OnDestroy()
        {
            entity.AbilityApplied -= OnAbilityApplied;
        }
    }
}
