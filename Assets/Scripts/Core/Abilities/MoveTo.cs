using UnityEngine;

namespace Game.Core
{
    [CreateAssetMenu(fileName = "Ability", menuName = "Data/Abilities/MoveTo", order = 1)]
    public class MoveTo : Ability
    {
        public override void Apply(Entity owner, object data)
        {
            if(data is IntPoint position)
            {
                owner.SetPosition(position);
            }
        }
    }
}