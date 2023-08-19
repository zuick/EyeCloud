using System;
using System.Threading.Tasks;
using UnityEngine;
using Game.Core;

namespace Game.Visual
{
    [CreateAssetMenu(fileName = "MeleeAttackAbilityVisual", menuName = "Data/Abilities Visual/MeleeAttackAbilityVisual", order = 1)]
    public class MeleeAttackAbilityVisual : MoveToAbilityVisual
    {
        [SerializeField] private GameObject impactVFX;
        [SerializeField]
        [Range(0, 1)]
        private float impactVFXOffset = 0.5f;

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
                DoFX(
                    (int)(ability.Duration * 1000f * impactVFXOffset),
                    VisualConfig.ToWorld(target.Position)
                );
            }
        }

        private async void DoFX(int delayMs, Vector3 position)
        {
            await Task.Delay(delayMs);
            Instantiate(impactVFX, position, Quaternion.identity);
        }
    }
}