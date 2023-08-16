using UnityEngine;

namespace Game.Core
{
    public class PassTurn : IAbility
    {
        public void Invoke(Entity owner)
        {
            Debug.Log($"{owner.Name}: Pass!");
        }
    }
}