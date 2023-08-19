using System;
using System.Threading.Tasks;
using UnityEngine;
using Game.Core;

namespace Game.Visual
{
    [CreateAssetMenu(fileName = "MeleeAttackAbilityVisual", menuName = "Data/Abilities Visual/MeleeAttackAbilityVisual", order = 1)]
    public class MeleeAttackAbilityVisual : MoveToAbilityVisual
    {
        public override void Perform(EntityActor actor, AbilityApplyData applyData)
        {
            if (applyData.Ability is MeleeAttack ability && applyData.Data is Entity target)
            {
                var initialPosition = actor.transform.position;
                DoPerform(
                    actor,
                    ability.Duration,
                    target.Position,
                    () => actor.transform.position = initialPosition
                );
            }
        }
    }
}