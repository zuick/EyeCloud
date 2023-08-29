using System;
using System.Threading.Tasks;
using UnityEngine;
using Game.Core;

namespace Game.Visual
{
    [CreateAssetMenu(fileName = "SimpleVFXAbilityVisual", menuName = "Data/Abilities Visual/SimpleVFXAbilityVisual", order = 1)]
    public class SimpleVFXAbilityVisual : AbilityVisual
    {
        [SerializeField] private GameObject VFX;
        [SerializeField] private Vector3 offset;
        [SerializeField][Range(0, 1)] private float delayOffset = 0.5f;

        public override void Perform(EntityActor actor, AbilityApplyData applyData)
        {
            DoFX(
                (int)(applyData.Ability.Duration * 1000f * delayOffset),
                actor.transform.parent,
                actor.transform.localPosition
            );
        }

        private async void DoFX(int delayMs, Transform parent, Vector3 localPosition)
        {
            await Task.Delay(delayMs);
            var instance = Instantiate(VFX, parent, false);
            instance.transform.localPosition = localPosition + offset;
        }
    }
}
