using System;
using System.Threading.Tasks;
using UnityEngine;
using Game.Core;

namespace Game.Visual
{
    [CreateAssetMenu(fileName = "MoveToAbilityVisual", menuName = "ScriptableObjects/MoveToAbilityVisual", order = 1)]
    public class MoveToAbilityVisual : AbilityVisual
    {
        [SerializeField] private float duration = 1f;

        public override void Perform(EntityActor actor, object ability)
        {
            if (ability is MoveTo moveTo)
            {
                DoPerform(actor, moveTo);
            }
        }

        async void DoPerform(EntityActor actor, MoveTo moveTo)
        {
            var targetPosition = VisualConfig.ToWorld(moveTo.Position);
            var worldDelta = targetPosition - actor.transform.position;
            var timer = 0f;
            while (timer < duration)
            {
                actor.transform.position += worldDelta * Time.deltaTime / duration;
                timer += Time.deltaTime;
                await Task.Yield();
            }

            actor.transform.position = targetPosition;
        }
    }
}
