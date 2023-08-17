using UnityEngine;

namespace Game.Core
{
    public class MoveTo : IAbility
    {
        public readonly IntPoint Position;

        public MoveTo(IntPoint position)
        {
            Position = position;
        }

        public void Invoke(Entity owner)
        {
            owner.SetPosition(Position);
            Debug.Log($"{owner.Name}: Moved to: {owner.Position}");
        }
    }
}