using System;
using System.Threading.Tasks;
using UnityEngine;
using Game.Core;

namespace Game.Visual
{
    [CreateAssetMenu(fileName = "MoveToAbilityVisual", menuName = "Data/Abilities Visual/MoveToAbilityVisual", order = 1)]
    public class MoveToAbilityVisual : AbilityVisual
    {
        [SerializeField] private AnimationCurve movingCurve;

        public override void Perform(EntityActor actor, AbilityApplyData applyData)
        {
            if (applyData.Ability is MoveTo moveTo && applyData.Data is IntPoint position)
            {
                DoPerform(actor, moveTo, position);
            }
        }

        async void DoPerform(EntityActor actor, MoveTo moveTo, IntPoint position)
        {
            var initialPosition = actor.transform.position;
            var targetPosition = VisualConfig.ToWorld(position);
            var worldDelta = targetPosition - actor.transform.position;
            var timer = 0f;
            while (timer < moveTo.Duration)
            {
                var t = timer / moveTo.Duration;

                actor.transform.position = initialPosition + worldDelta * movingCurve.Evaluate(t);

                timer += Time.deltaTime;

                await Task.Yield();
            }

            actor.transform.position = targetPosition;
        }
    }
}
