using UnityEngine;

namespace Game.Core
{
    [CreateAssetMenu(fileName = "MeleeAttack", menuName = "Data/Abilities/MeleeAttack", order = 1)]
    public class MeleeAttack : Ability
    {
        [SerializeField] private int hitPoints;

        public override void Apply(Entity owner, object data)
        {
            if(data is Entity target)
            {
                var stats = target.Stats;
                var finalHitPoints = (int)owner.Stats.AttackMultiplier * hitPoints;
                stats.HP -= finalHitPoints;
                target.SetStats(stats);
            }
        }
    }
}