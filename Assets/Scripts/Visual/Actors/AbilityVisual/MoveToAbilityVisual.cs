using System;
using System.Threading.Tasks;
using UnityEngine;
using Game.Core;

namespace Game.Visual
{
    [CreateAssetMenu(fileName = "MoveToAbilityVisual", menuName = "Data/Abilities Visual/MoveToAbilityVisual", order = 1)]
    public class MoveToAbilityVisual : AbilityVisual
    {
        public override void Perform(EntityActor actor, AbilityApplyData applyData)
        {
            if (applyData.Ability is MoveTo moveTo && applyData.Data is IntPoint position)
            {
                DoPerform(actor, moveTo, position);
            }
        }

        async void DoPerform(EntityActor actor, MoveTo moveTo, IntPoint position)
        {
            var targetPosition = VisualConfig.ToWorld(position);
            var worldDelta = targetPosition - actor.transform.position;
            var timer = 0f;
            while (timer < moveTo.Duration)
            {
                actor.transform.position += worldDelta * Time.deltaTime / moveTo.Duration;
                timer += Time.deltaTime;
                await Task.Yield();
            }

            actor.transform.position = targetPosition;
        }
    }
}
