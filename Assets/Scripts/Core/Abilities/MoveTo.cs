using UnityEngine;

namespace Game.Core
{
    public class MoveTo : IAbility
    {
        private IntPoint position;

        public MoveTo(IntPoint position)
        {
            this.position = position;
        }

        public void Invoke(Entity owner)
        {
            owner.SetPosition(position);
            Debug.Log($"{owner.Name}: Moved to: {owner.Position}");
        }
    }
}