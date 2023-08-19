using UnityEngine;

namespace Game.Core
{
    public abstract class Ability : ScriptableObject, IAbility
    {
        [SerializeField] private float duration;

        public float Duration => duration;
        public string Name => name;

        public virtual void Apply(Entity owner, object data)
        {

        }
    }
}

