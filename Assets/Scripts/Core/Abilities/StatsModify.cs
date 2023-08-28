using UnityEngine;

namespace Game.Core
{
    [CreateAssetMenu(fileName = "StatsModify", menuName = "Data/Abilities/StatsModify", order = 1)]
    public class StatsModify : ProcessInteractionAbility
    {
        [SerializeField] private Mode mode;
        [SerializeField] private EntityStats entityStats;

        public override void Apply(Entity owner, object data)
        {
            if (data is Entity target)
            {
                if (mode == Mode.Positive)
                {
                    target.SetStats(target.Stats + entityStats);
                }
                else
                {
                    target.SetStats(target.Stats - entityStats);
                }

                owner.SetStats(EntityStats.Empty); // destroy self
            }
        }

        private enum Mode { Positive, Negative }
    }
}