using System;
using System.Threading.Tasks;
using UnityEngine;
using Game.Core;

namespace Game.Visual
{
    [CreateAssetMenu(fileName = "MeleeAttackAbilityVisual", menuName = "Data/Abilities Visual/MeleeAttackAbilityVisual", order = 1)]
    public class MoveToAbilityVisual : AbilityVisual
    {
        [SerializeField] protected AnimationCurve movingCurve;

        public override void Perform(EntityActor actor, AbilityApplyData applyData)
        {
            if (applyData.Ability is MoveTo moveTo && applyData.Data is IntPoint position)
            {
                DoPerform(actor, moveTo.Duration, position);
            }
        }

        protected async void DoPerform(EntityActor actor, float duration, IntPoint position, Action onFinish = null)
        {
            var initialPosition = actor.transform.localPosition;
            var targetPosition = VisualConfig.ToLevelLocal(position);
            var worldDelta = targetPosition - actor.transform.localPosition;
            var timer = 0f;

            actor.transform.rotation = Quaternion.LookRotation(worldDelta, Vector3.up);

            while (timer < duration)
            {
                var t = timer / duration;

                actor.transform.localPosition = initialPosition + worldDelta * movingCurve.Evaluate(t);

                timer += Time.deltaTime;

                await Task.Yield();
            }

            actor.transform.localPosition = targetPosition;

            onFinish?.Invoke();
        }
    }
}
