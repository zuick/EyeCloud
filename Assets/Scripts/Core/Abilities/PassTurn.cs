using UnityEngine;

namespace Game.Core
{
    public class PassTurn : IAbility
    {
        public void Invoke()
        {
            Debug.Log("Pass turn");
        }
    }
}