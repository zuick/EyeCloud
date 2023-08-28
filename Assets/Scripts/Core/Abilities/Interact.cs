using UnityEngine;

namespace Game.Core
{
    [CreateAssetMenu(fileName = "Interact", menuName = "Data/Abilities/Interact", order = 1)]
    public class Interact : Ability
    {
        public override void Apply(Entity owner, object data)
        {
            if (data is Entity target && target.TryGetAbility<ProcessInteractionAbility>(out var processor))
            {
                processor.Apply(target, owner);
            }
        }
    }
}