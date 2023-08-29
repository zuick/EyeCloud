using UnityEngine;
using Game.Data;
using Game.Services;
using Game.Messages;

namespace Game.Core
{
    [CreateAssetMenu(fileName = "SpawnEntity", menuName = "Data/Abilities/SpawnEntity", order = 1)]
    public class SpawnEntity : Ability
    {
        [SerializeField] EntityData[] entities;

        public override void Apply(Entity owner, object data)
        {
            if (data is IntPoint position)
            {
                var randomData = entities[Random.Range(0, entities.Length)];
                MessagesService.Publish(new SpawnEntityMessage(position, randomData));
            }
        }
    }
}